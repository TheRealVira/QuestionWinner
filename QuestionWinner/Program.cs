#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: QuestionWinner
// Project: QuestionWinner
// Filename: Program.cs
// Date - created: 2016.05.14 - 23:59
// Date - current: 2016.05.15 - 21:54

#endregion

#region Usings

using System;
using System.Windows.Forms;

#endregion

namespace QuestionWinner
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}