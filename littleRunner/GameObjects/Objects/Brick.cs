using System;
using System.Collections.Generic;
using System.Text;

namespace littleRunner.GameObjects.Objects
{
    enum BrickColor
    {
        Blue,
        Ice,
        Red,
        Yellow
    }
    class Brick : StickyImageElement
    {
        BrickColor color;
        public BrickColor Color
        {
            get { return color; }
            set
            {
                color = value;
                switch (color)
                {
                    case BrickColor.Blue: CurImgFilename = Files.f[gFile.brick_blue]; break;
                    case BrickColor.Ice: CurImgFilename = Files.f[gFile.brick_ice]; break;
                    case BrickColor.Red: CurImgFilename = Files.f[gFile.brick_red]; break;
                    case BrickColor.Yellow: CurImgFilename = Files.f[gFile.brick_yellow]; break;
                }
            }
        }
        override public bool canStandOn
        {
            get { return true; }
        }


        public Brick()
            : base()
        {
            Color = BrickColor.Blue;
        }
        public Brick(int top, int left, BrickColor color)
            : base(top, left)
        {
            Color = color;
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
            Color = (BrickColor)ser["Color"];
        }
    }
}
