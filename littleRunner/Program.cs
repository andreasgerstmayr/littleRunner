using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

using littleRunner.Highscoredata;

namespace littleRunner
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.Run(new StartScreen());
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            DebugInfo.WriteException(e.Exception);
            DebugInfo.WriteLine("\n" +
            "=== Game crashed ===\n\n" +
            "Please send the contents of this file to <littleRunner@andihit.net>!\n" +
            "That helps me to solve that bug.\n\n\n");

            DebugInfo.ShowLog();

            Application.ExitThread();
            Application.Exit();
        }
    }
}