using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace littleRunner
{
    enum SpikaColor
    {
        Orange,
        Green,
        Grey
    }
    class Spika : Enemy
    {
        private Image curimg;
        private SpikaColor color;
        public override bool fireCanDelete
        {
            get { return false; }
        }
        public SpikaColor Color
        {
            get { return color; }
            set
            {
                color = value;
                switch (color)
                {
                    case SpikaColor.Orange: curimg = Image.FromFile(Files.f[gFile.spika_orange]); break;
                    case SpikaColor.Green: curimg = Image.FromFile(Files.f[gFile.spika_green]); break;
                    case SpikaColor.Grey: curimg = Image.FromFile(Files.f[gFile.spika_grey]); break;
                }
            }
        }

        public override void Draw(Graphics g)
        {
            g.DrawImage(curimg, Left, Top, Width, Height);
        }

        public Spika()
            : base()
        {
            Color = SpikaColor.Green;
        }

        public Spika(int top, int left)
            : base()
        {
            Top = top;
            Left = left;

            Color = SpikaColor.Green;

            Width = curimg.Width;
            Height = curimg.Height; 
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
            ser["Color"] = color;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            Color = (SpikaColor)ser["Color"];
        }
    }
}
