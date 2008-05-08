using System;
using System.Collections.Generic;

using littleRunner.Drawing;


namespace littleRunner.GameObjects.StickyElements
{
    abstract class StickyImageElement : StickyElement
    {
        private Draw.Image curimg;
        private string curimgfn;

        public override void Update(Draw d)
        {
            if (curimgfn == Files.brick_invisible && World.PlayMode == PlayMode.Editor)
            {
                d.DrawRectangle(Draw.Pen.Black, Left, Top, Width, Height);
            }

            d.DrawImage(curimg, Left, Top, Width, Height);
        }
        protected string CurImgFilename
        {
            set
            {
                curimgfn = value;
                curimg = Draw.Image.Open(curimgfn);
                Width = curimg.Width;
                Height = curimg.Height;
            }
        }

        public StickyImageElement()
        {
        }
        public StickyImageElement(int top, int left)
        {
            Top = top;
            Left = left;
        }
        public StickyImageElement(int top, int left, string imgfn) : this(top, left)
        {
            CurImgFilename = imgfn;
        }
    }
}
