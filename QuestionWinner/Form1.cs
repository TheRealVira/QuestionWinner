#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: QuestionWinner
// Project: QuestionWinner
// Filename: Form1.cs
// Date - created: 2016.05.15 - 10:24
// Date - current: 2016.05.15 - 21:54

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

#endregion

namespace QuestionWinner
{
    public partial class Form1 : Form
    {
        private const string SOURCE = "[Q] Yes or no?\n[A] Yes!\n\n[Q] You see?\n[A] Oh wow great!\n[A] Ez";

        private readonly Dictionary<string, List<string>> Data;
        readonly SolidBrush drawBrush = new SolidBrush(Color.Black);
        private readonly Font font;
        private readonly IKeyboardMouseEvents m_GlobalHook;
        private bool HideMe;


        public Form1()
        {
            InitializeComponent();

            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
            HideMe = false;
            DoubleBuffered = true;

            Bounds = Screen.PrimaryScreen.Bounds;
            var initialStyle = GetWindowLong(Handle, -20);
            SetWindowLong(Handle, -20, initialStyle | 0x80000 | 0x20);

            Data = CreateDic(SOURCE);
            MessageBox.Show("Finished Loading");

            font = new Font("Arial", 8);

            timer1.Interval = 1;
            timer1.Tick += Timer1_Tick;
            timer1.Start();
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            font.Dispose();
            drawBrush.Dispose();
            m_GlobalHook.Dispose();

            base.OnClosing(e);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            if (HideMe || font == null || drawBrush == null || Data == null ||
                !Clipboard.ContainsText(TextDataFormat.Text)) return;

            try
            {
                //SendKeys.Send("^c");
                var cTex = Clipboard.GetText(TextDataFormat.Text);
                if (!Data.ContainsKey(cTex))
                {
                    return;
                }

                //MessageBox.Show(this.Data[cTex].Aggregate(string.Empty,
                //    (current, VARIABLE) => current + ("->" + VARIABLE + "\n")));
                //graphics.DrawString(this.Data[cTex].Aggregate(string.Empty, (current, VARIABLE) => current + ("->" + VARIABLE + "\n")), font, drawBrush, Cursor.Position);

                var drawString = Data[cTex].Aggregate(string.Empty,
                    (current, VARIABLE) => current + ("->" + VARIABLE + "\n"));
                var drawFormat = new StringFormat();
                e.Graphics.DrawString(drawString, font, drawBrush, Cursor.Position, drawFormat);
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        private Dictionary<string, List<string>> CreateDic(string source, char splitt = '\n', string question = "[Q] ",
            string answers = "[A] ")
        {
            var toRet = new Dictionary<string, List<string>>();
            var splittedSource = source.Split(splitt);

            for (var i = 0; i < splittedSource.Length; i++)
            {
                if (splittedSource[i] == string.Empty) continue;

                var q = string.Empty;
                var ans = new List<string>();

                while (i < splittedSource.Length && splittedSource[i] != string.Empty)
                {
                    if (splittedSource[i].StartsWith(question))
                    {
                        splittedSource[i] = splittedSource[i].Remove(0, question.Length);
                        q += splittedSource[i];
                    }
                    else
                    {
                        splittedSource[i] = splittedSource[i].Remove(0, answers.Length);
                        ans.Add(splittedSource[i]);
                    }

                    i++;
                }

                if (toRet.ContainsKey(q)) continue;

                toRet.Add(q, ans);
            }

            return toRet;
        }


        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'h')
            {
                HideMe = !HideMe;
            }

            if (e.KeyChar == 'e')
            {
                Close();
            }
        }
    }
}