using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace littleRunner
{
    public class GamePoint
    {
        int x;
        int y;

        public GamePoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
    }
}
