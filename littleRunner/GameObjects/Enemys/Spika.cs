using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace littleRunner
{
    class Spika : Enemy
    {
        private Image curimg;

        public override bool canFire
        {
            get { return false; }
        }
        public override void Draw(Graphics g)
        {
            g.DrawImage(curimg, Left, Top, Width, Height);
        }

        public Spika()
            : base()
        {
            curimg = new Bitmap(1, 1);
        }

        public Spika(int top, int left, Image img)
            : base()
        {
            Top = top;
            Left = left;

            Width = img.Width;
            Height = img.Height;

            curimg = img;
        }

        public override void Init(World world)
        {
            base.Init(world);
        }


        public override bool getCrashEvent(GameObject go, GameDirection cidirection)
        {
            return false;
        }

        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["img"] = curimg;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            curimg = (Image)ser["img"];
        }
    }
}
