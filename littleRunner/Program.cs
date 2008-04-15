using System;
using System.Collections.Generic;
using System.Windows.Forms;

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


            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            
            Application.Run(new ProgramSwitcher());
        }


        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            DebugInfo.WriteLine(e.Exception);
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