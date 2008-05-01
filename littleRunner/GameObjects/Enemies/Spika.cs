using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace littleRunner.GameObjects.Enemies
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
        [Category("Spika")]
        public SpikaColor Color
        {
            get { return color; }
            set
            {
                color = value;
                switch (color)
                {
                    case SpikaColor.Orange: curimg = Image.FromFile(Files.spika_orange); break;
                    case SpikaColor.Green: curimg = Image.FromFile(Files.spika_green); break;
                    case SpikaColor.Grey: curimg = Image.FromFile(Files.spika_grey); break;
                }
            }
        }

        public override bool turtleCanRemove
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
            Color = SpikaColor.Green;
        }

        public Spika(int top, int left)
            : this()
        {
            Top = top;
            Left = left;

            Width = curimg.Width;
            Height = curimg.Height; 
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
