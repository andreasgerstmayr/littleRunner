using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace littleRunner
{
    public partial class Scripteditor : Form
    {
        public Scripteditor()
        {
            InitializeComponent();
        }

        public string ScriptText
        {
            get { return script.Text; }
            set
            {
                script.Text = value;
                script.timer_Tick(new object(), new EventArgs());
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            script.Stop();
        }
    }
}
