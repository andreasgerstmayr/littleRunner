using System;
using System.Windows.Forms;


namespace littleRunner
{
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
        }

        delegate void MessageInvoker(string s);
        public void Message(string s)
        {
            if (txt.InvokeRequired)
            {
                txt.Invoke(new MessageInvoker(Message), s);
                return;
            }

            txt.Text = s;
            Application.DoEvents();
        }
    }
}
