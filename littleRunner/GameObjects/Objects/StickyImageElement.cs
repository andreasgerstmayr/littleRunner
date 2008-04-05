using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace littleRunner.GameObjects.Objects
{
    abstract class StickyImageElement : StickyElement
    {
        protected Image curimg;
        protected string curimgfn;

        public override void Draw(Graphics g)
        {
            g.DrawImage(curimg, Left, Top, Width, Height);
        }
        protected string CurImgFilename
        {
            set
            {
                curimgfn = value;
                curimg = Image.FromFile(curimgfn);
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
            curimg = new Bitmap(1, 1);
        }
        public StickyImageElement(int top, int left, string imgfn) : this(top, left)
        {
            CurImgFilename = imgfn;
        }


        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["ImageFilename"] = curimgfn;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            CurImgFilename = (string)ser["ImageFilename"];
        }
    }
}
