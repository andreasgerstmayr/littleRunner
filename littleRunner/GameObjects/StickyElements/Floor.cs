using System;
using System.Collections.Generic;
using System.ComponentModel;

using littleRunner.Drawing;
using littleRunner.Drawing.Helpers;
using littleRunner.Editordata;


namespace littleRunner.GameObjects.StickyElements
{
    enum FloorColor
    {
        Green
    }
    class Floor : StickyElement
    {
        private FloorColor color;
        private dImage imgL, imgM, imgR;
        int blocks;


        public int Blocks
        {
            get { return blocks; }
            set
            {
                blocks = value;
                width = (blocks + 2) * (imgM.Width - 1);
            }
        }
        public override int Width
        {
            set { Editor.ShowErrorBox(this, "You have to set the 'blocks' property."); }
        }
        public override void Update(Draw d)
        {
            for (int i = 0; i < blocks + 2; i++)
            {
                dImage paint = i == 0 ? imgL : (i + 1 == blocks + 2 ? imgR : imgM);
                d.DrawImage(paint, Left + i * (paint.Width - 1), Top, paint.Width, Height);
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
                        imgL = GetDraw.Image(Files.floor_left);
                        imgM = GetDraw.Image(Files.floor_middle);
                        imgR = GetDraw.Image(Files.floor_right);
                        break;
                }
            }
        }

        public Floor()
            : base()
        {
        }
        public Floor(float top, float left, FloorColor style)
            : this()
        {
            Top = top;
            Left = left;

            Color = style;

            Blocks = 1;
            Height = imgL.Height;
        }

        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["FloorStyle"] = color;
            ser["Blocks"] = blocks;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            Color = (FloorColor)ser["FloorStyle"];
            Blocks = (int)ser["Blocks"];
        }
    }
}
