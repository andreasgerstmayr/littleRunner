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

        static public Rectangle GetRectangle(GamePoint start, GamePoint end)
        {
            int x = 0;
            int y = 0;
            int width = 0;
            int height = 0;

            if (start.X < end.X && start.Y < end.Y)
            {
                x = start.X;
                y = start.Y;
                width = end.X - start.X;
                height = end.Y - start.Y;
            }
            else if (start.X > end.X && start.Y > end.Y)
            {
                x = end.X;
                y = end.Y;
                width = start.X - end.X;
                height = start.Y - end.Y;
            }
            else if (start.Y < end.Y && end.X < start.X)
            {
                x = end.X;
                y = start.Y;
                width = start.X - x;
                height = end.Y - y;
            }
            else if (end.Y < start.Y && start.X < end.X)
            {
                x = start.X;
                y = end.Y;
                width = end.X - x;
                height = start.Y - y;
            }

            return new Rectangle(x, y, width, height);
        }
    }
}
