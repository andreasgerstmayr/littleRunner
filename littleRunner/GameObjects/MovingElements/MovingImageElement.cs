using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;


namespace littleRunner.GameObjects.MovingElements
{
    abstract class MovingImageElement : MovingElement
    {
        protected Image curimg;

        public override void Draw(Graphics g)
        {
            g.DrawImage(curimg, Left, Top, Width, Height);
        }

        public MovingImageElement()
        {
        }
        public MovingImageElement(Image img, int top, int left)
        {
            Top = top;
            Left = left;

            Width = img.Width;
            Height = img.Height;

            curimg = img;
        }
    }
}
