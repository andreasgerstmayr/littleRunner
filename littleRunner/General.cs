using System;
using System.Drawing;


namespace littleRunner
{
    public class GamePoint
    {
        float x;
        float y;

        public GamePoint(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float X
        {
            get { return x; }
            set { x = value; }
        }
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        static public Rectangle GetRectangle(GamePoint start, GamePoint end)
        {
            float x = 0;
            float y = 0;
            float width = 0;
            float height = 0;

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

            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }
    }
}
