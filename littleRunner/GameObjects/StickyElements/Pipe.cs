using System;
using System.Collections.Generic;
using System.ComponentModel;

using littleRunner.Drawing;
using littleRunner.Drawing.Helpers;
using littleRunner.Editordata;


namespace littleRunner.GameObjects.StickyElements
{
    enum PipeColor
    {
        Green
    }
    class Pipe : StickyElement
    {
        private PipeColor color;
        private dImage imgU, imgM;
        int blocks;


        public int Blocks
        {
            get { return blocks; }
            set
            {
                blocks = value;
                height = imgU.Height + blocks * imgM.Height;
            }
        }
        public override int Height
        {
            set { Editor.ShowErrorBox(this, "You have to set the 'blocks' property."); }
        }
        public override void Update(Draw d)
        {
            d.DrawImage(imgU, Left, Top, Width, imgU.Height);

            for (int i = 0; i < blocks; i++)
            {
                d.DrawImage(imgM, Left, Top + imgU.Height + i * imgM.Height, Width, imgM.Height);
            }
        }


        override public bool canStandOn
        {
            get { return true; }
        }
        [Category("Pipe")]
        public PipeColor Color
        {
            get { return color; }
            set
            {
                color = value;
                switch (color)
                {
                    case PipeColor.Green:
                        imgU = GetDraw.Image(Files.pipe_green_up);
                        imgM = GetDraw.Image(Files.pipe_green_main);
                        break;
                }
            }
        }

        public Pipe()
            : base()
        {
        }
        public Pipe(float top, float left, PipeColor color)
            : this()
        {
            Top = top;
            Left = left;

            Color = color;

            Width = imgU.Width;
            Blocks = 1;
        }

        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["PipeColor"] = color;
            ser["Blocks"] = blocks;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            Color = (PipeColor)ser["PipeColor"];
            Blocks = (int)ser["Blocks"];
        }
    }
}
