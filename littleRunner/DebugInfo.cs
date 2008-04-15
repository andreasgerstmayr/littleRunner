using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Diagnostics;

namespace littleRunner
{
    class DebugInfo
    {
        private static string text = "";


        public static void WriteLine(object text)
        {
            DebugInfo.text = text + "\n" + DebugInfo.text;
        }

        public static void ShowLog()
        {
            string file = Path.GetTempFileName();
            File.WriteAllText(file, text.Replace("\n", Environment.NewLine));

            Process.Start(file).WaitForExit();
        }
    }
}
