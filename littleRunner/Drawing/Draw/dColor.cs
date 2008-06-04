using System;
using System.Collections.Generic;
using System.Text;


namespace littleRunner.Drawing
{
    public class dColor
    {
        public int A, R, G, B;
        public dColor(int a, int r, int g, int b)
        {
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
        }
        public dColor(int r, int g, int b)
            : this(255, r, g, b)
        {
        }
        public dColor(System.Drawing.Color c)
        {
            A = c.A;
            R = c.R;
            G = c.G;
            B = c.B;
        }

        public System.Drawing.Color ToGDI()
        {
            return System.Drawing.Color.FromArgb(A, R, G, B);
        }
    }
}
