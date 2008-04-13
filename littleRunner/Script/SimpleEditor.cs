using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;


namespace littleRunner
{
    class SimpleEditor : RichTextBox
    {
        string source, output;
        Timer timer;

        public SimpleEditor()
            : base()
        {
            source = Path.GetTempFileName();
            output = Path.GetTempFileName();

            timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += new EventHandler(timer_Tick);
        }

        internal void Stop()
        {
            timer.Enabled = false;
        }

        internal void timer_Tick(object sender, EventArgs e)
        {
            timer.Enabled = false;
            this.ReadOnly = true;

            // generate
            File.WriteAllText(source, this.Text);

            Process p = new Process();
            p.StartInfo.FileName = "Script/start.bat";

            p.StartInfo.Arguments = "\"" + source + "\" \"" + output + "\"";

            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.UseShellExecute = true;

            p.Start();
            p.WaitForExit();


            // write in control
            int cursorpos = this.SelectionStart;

            this.Rtf = File.ReadAllText(output);
            this.SelectionStart = cursorpos;
            this.ScrollToCaret();

            ZoomFactor = 0.8F;
            this.ReadOnly = false;
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            timer.Enabled = false;
            timer.Enabled = true;

            ZoomFactor = 0.799F;
        }
    }
}