using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using littleRunner.Editordata;
using littleRunner.Drawing;
using littleRunner.Drawing.Helpers;
using littleRunner.GameObjects.StickyElements;


namespace littleRunner.GameObjects.MovingElements
{
    class Bricks : MovingElement
    {
        dImage image;
        BrickColor color;
        int imgWidth;
        int blocks;

        [Category("Bricks")]
        public BrickColor Color
        {
            get { return color; }
            set
            {
                color = value;
                switch (color)
                {
                    case BrickColor.Blue: image = GetDraw.Image(Files.brick_blue); break;
                    case BrickColor.Ice: image = GetDraw.Image(Files.brick_ice); break;
                    case BrickColor.Red: image = GetDraw.Image(Files.brick_red); break;
                    case BrickColor.Yellow: image = GetDraw.Image(Files.brick_yellow); break;
                    case BrickColor.Brown: image = GetDraw.Image(Files.brick_brown); break;
                    case BrickColor.Invisible: image = GetDraw.Image(Files.brick_invisible); break;
                }
                imgWidth = image.Width;


                if (Blocks != 0)
                    Blocks = Blocks; // set width
                Height = image.Height;
            }
        }
        public override bool canStandOn
        {
            get { return true; }
        }
        public int Blocks
        {
            get { return blocks; }
            set
            {
                if (value == 0)
                    Editor.ShowErrorBox(this, "Can't set 0 blocks!");
                else
                {
                    blocks = value;
                    width = blocks * (imgWidth+1) - 1; // one px distance between blocks
                }
            }
        }
        public override int Width
        {
            set { Editor.ShowErrorBox(this, "You have to set the 'blocks' property."); }
        }

        public override void Update(Draw d)
        {
            int width = image.Width;
            for (int i = 0; i < blocks; i++)
                d.DrawImage(image, Left + i * (imgWidth + 1), Top, width, Height);

            if (color == BrickColor.Invisible && World.PlayMode == PlayMode.Editor)
                d.DrawRectangle(dPen.Black, Left, Top, Width, Height);
        }



        private void fillMgoMoveQueue(MoveType type)
        {
            World.MGO.Move(type, 700, true, GameInstruction.Nothing);
        }

        public override void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
            base.onOver(geventhandler, who, direction);

            if (color == BrickColor.Ice && who == GameElement.MGO && direction == GameDirection.Top)
            {
                switch (World.MGO.Direction)
                {
                    case GameDirection.Left:
                        for (int i = 0; i < Blocks * 2; i++)
                        {
                            fillMgoMoveQueue(MoveType.goLeft);
                        }
                        break;
                    case GameDirection.Right:
                        for (int i = 0; i < Blocks * 2; i++)
                        {
                            fillMgoMoveQueue(MoveType.goRight);
                        }
                        break;
                }
            }
        }


        public Bricks()
        {
            imgWidth = 0;
        }
        public Bricks(float top, float left, BrickColor color)
            : this()
        {
            Top = top;
            Left = left;

            Color = color;

            Blocks = 5;
            // Height in Color-Property
        }

        public override void Check(out Dictionary<string, float> newpos)
        {
            base.Check(out newpos);
            float newtop = newpos["top"];
            float newleft = newpos["left"];


            if (newtop != 0)
                Top += newtop;
            if (newleft != 0)
                Left += newleft;
        }


        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["Color"] = color;
            ser["Blocks"] = blocks;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            Color = (BrickColor)ser["Color"];
            Blocks = (int)ser["Blocks"];
        }
    }
}
