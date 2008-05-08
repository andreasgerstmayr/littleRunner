using System;
using System.Collections.Generic;
using System.Text;

using littleRunner.Drawing;


namespace littleRunner.GameObjects.MovingElements
{
    abstract class MovingImageElement : MovingElement
    {
        protected Draw.Image curimg;

        public override void Update(Draw d)
        {
            d.DrawImage(curimg, Left, Top, Width, Height);
        }

        public MovingImageElement()
        {
        }
        public MovingImageElement(Draw.Image img, int top, int left)
        {
            Top = top;
            Left = left;

            Width = img.Width;
            Height = img.Height;

            curimg = img;
        }
    }
}
