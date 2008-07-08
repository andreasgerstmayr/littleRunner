using System;
using System.Collections.Generic;

using littleRunner.Drawing;
using littleRunner.Drawing.Helpers;


namespace littleRunner.GameObjects.StickyElements
{
    abstract class StickyImageElement : StickyElement
    {
        private dImage curimg;
        private string curimgfn;

        public override void Update(Draw d)
        {
            if (curimgfn == Files.brick_invisible && World.PlayMode == PlayMode.Editor)
                d.DrawRectangle(dPen.Black, Left, Top, Width, Height);

            d.DrawImage(curimg, Left, Top, Width, Height);
        }
        protected string CurImgFilename
        {
            set
            {
                curimgfn = value;
                curimg = GetDraw.Image(curimgfn);
                Width = curimg.Width;
                Height = curimg.Height;
            }
        }

        public StickyImageElement()
        {
        }
        public StickyImageElement(float top, float left)
        {
            Top = top;
            Left = left;
        }
        public StickyImageElement(float top, float left, string imgfn)
            : this(top, left)
        {
            CurImgFilename = imgfn;
        }
    }
}
