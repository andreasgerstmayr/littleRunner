using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;


namespace littleRunner
{
    public class Debug
    {
        private Label element;

        public string Message
        {
            set
            {
                element.Text = value;
                Application.DoEvents();
            }
        }

        public Debug(Label element)
        {
            this.element = element;
        }

        public static void PrintDict(Dictionary<object, object> dict)
        {
            string msg = "";

            foreach (KeyValuePair<object,object> kp in dict)
            {
                msg += "['" + kp.Key + "'] = '" + kp.Value + "'\n";
            }
            MessageBox.Show(msg);
        }
        public static void PrintDict(Dictionary<string, object> dict)
        {
            string msg = "";

            foreach (KeyValuePair<string, object> kp in dict)
            {
                msg += "['" + kp.Key + "'] = '" + kp.Value + "'\n";
            }
            MessageBox.Show(msg);
        }
    }
}
