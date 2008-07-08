using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace littleRunner.GameObjects.StickyElements
{
    enum BrickColor
    {
        Blue,
        Ice,
        Red,
        Yellow,
        Brown,
        Invisible
    }
    class Brick : StickyImageElement
    {
        BrickColor color;
        [Category("Brick")]
        public BrickColor Color
        {
            get { return color; }
            set
            {
                color = value;
                switch (color)
                {
                    case BrickColor.Blue: CurImgFilename = Files.brick_blue; break;
                    case BrickColor.Ice: CurImgFilename = Files.brick_ice; break;
                    case BrickColor.Red: CurImgFilename = Files.brick_red; break;
                    case BrickColor.Yellow: CurImgFilename = Files.brick_yellow; break;
                    case BrickColor.Brown: CurImgFilename = Files.brick_brown; break;
                    case BrickColor.Invisible: CurImgFilename = Files.brick_invisible; break;
                }
            }
        }
        override public bool canStandOn
        {
            get { return true; }
        }


        private void fillMgoMoveQueue(MoveType type)
        {
            World.MGO.Move(type, 10, GameInstruction.Nothing);
        }

        public override void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
            base.onOver(geventhandler, who, direction);

            if (color == BrickColor.Ice && who == GameElement.MGO && direction == GameDirection.Top)
            {
                switch (World.MGO.Direction)
                {
                    case GameDirection.Left:
                        for (int i = 0; i < 2; i++)
                        {
                            fillMgoMoveQueue(MoveType.goLeft);
                        }
                        break;
                    case GameDirection.Right:
                        for (int i = 0; i < 2; i++)
                        {
                            fillMgoMoveQueue(MoveType.goRight);
                        }
                        break;
                }
            }
        }


        public Brick()
            : base()
        {
            Color = BrickColor.Blue;
        }
        public Brick(float top, float left, BrickColor color)
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
