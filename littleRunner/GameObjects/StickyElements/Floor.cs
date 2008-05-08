using System;
using System.Collections.Generic;
using System.ComponentModel;

using littleRunner.Drawing;


namespace littleRunner.GameObjects.StickyElements
{
    enum FloorColor
    {
        Green
    }
    class Floor : StickyElement
    {
        private FloorColor color;
        private Draw.Image imgL, imgM, imgR;

        public override void Update(Draw d)
        {
            // Ceiling (round to x % 64 == 0)
            if (Width < 63)
                Width = 63;

            int rest = Width % 63;
            if (rest != 0)
            {
                if (rest < 31)
                    Width -= rest;
                else
                    Width += 63 - rest;
            }

            int occurences = Width / 63 ;
            for (int i = 0; i < occurences; i++)
            {
                Draw.Image paint = i==0?imgL: (i+1==occurences ? imgR : imgM);
                d.DrawImage(paint, Left + i * 63, Top, paint.Width, Height);
            }
        }


        override public bool canStandOn
        {
            get { return true; }
        }
        [Category("Floor")]
        public FloorColor Color
        {
            get { return color; }
            set
            {
                color = value;
                switch (color)
                {
                    case FloorColor.Green:
                        imgL = Draw.Image.Open(Files.floor_left);
                        imgM = Draw.Image.Open(Files.floor_middle);
                        imgR = Draw.Image.Open(Files.floor_right);
                        break;
                }
            }
        }

        public Floor()
            : base()
        {
        }
        public Floor(int top, int left, FloorColor style)
            : this()
        {
            Top = top;
            Left = left;

            Color = style;

            Width = (imgM.Width-1)*3;
            Height = imgL.Height;
        }

        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["FloorStyle"] = color;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            Color = (FloorColor)ser["FloorStyle"];
        }
    }
}
