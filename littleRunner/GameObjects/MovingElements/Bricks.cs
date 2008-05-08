using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using littleRunner.Drawing;
using littleRunner.GameObjects.StickyElements;


namespace littleRunner.GameObjects.MovingElements
{
    class Bricks : MovingElement
    {
        Draw.Image image;
        BrickColor color;
        int imgWidth;

        [Category("Bricks")]
        public BrickColor Color
        {
            get { return color; }
            set
            {
                color = value;
                switch (color)
                {
                    case BrickColor.Blue: image = Draw.Image.Open(Files.brick_blue); break;
                    case BrickColor.Ice: image = Draw.Image.Open(Files.brick_ice); break;
                    case BrickColor.Red: image = Draw.Image.Open(Files.brick_red); break;
                    case BrickColor.Yellow: image = Draw.Image.Open(Files.brick_yellow); break;
                    case BrickColor.Brown: image = Draw.Image.Open(Files.brick_brown); break;
                    case BrickColor.Invisible: image = Draw.Image.Open(Files.brick_invisible); break;
                }
                imgWidth = image.Width;

                Height = image.Height;
            }
        }
        public override bool canStandOn
        {
            get { return true; }
        }
        public override void Update(Draw d)
        {
            // Ceiling (round to x % width+1 == 0)
            if (Width < imgWidth + 1)
                Width = imgWidth+1;

            int rest = Width % (imgWidth + 1);
            if (rest != 0)
            {
                if (rest < imgWidth / 2)
                    Width -= rest;
                else
                    Width += imgWidth + 1 - rest;
            }

            int occurences = Width / (imgWidth+1);
            int width = image.Width;
            for (int i = 0; i < occurences; i++)
                d.DrawImage(image, Left + i * (imgWidth + 1), Top, width, Height);


            if (color == BrickColor.Invisible && World.PlayMode == PlayMode.Editor)
                d.DrawRectangle(Draw.Pen.Black, Left, Top, Width, Height);
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
                        for (int i = 0; i < 10; i++)
                        {
                            fillMgoMoveQueue(MoveType.goLeft);
                        }
                        break;
                    case GameDirection.Right:
                        for (int i = 0; i < 10; i++)
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
        public Bricks(int top, int left, BrickColor color) : this()
        {
            Top = top;
            Left = left;

            Color = color;

            Width = image.Width * 5;
            // Height in Color-Property
        }

        public override void Check(out Dictionary<string, int> newpos)
        {
            base.Check(out newpos);
            int newtop = newpos["top"];
            int newleft = newpos["left"];


            if (newtop != 0)
                Top += newtop;
            if (newleft != 0)
                Left += newleft;
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
