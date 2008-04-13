using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using littleRunner.GameObjects.Objects;


namespace littleRunner.GameObjects.MovingElements
{
    class Bricks : MovingElement
    {
        Image image;
        BrickColor color;

        public BrickColor Color
        {
            get { return color; }
            set
            {
                color = value;
                switch (color)
                {
                    case BrickColor.Blue: image = Image.FromFile(Files.f[gFile.brick_blue]); break;
                    case BrickColor.Ice: image = Image.FromFile(Files.f[gFile.brick_ice]); break;
                    case BrickColor.Red: image = Image.FromFile(Files.f[gFile.brick_red]); break;
                    case BrickColor.Yellow: image = Image.FromFile(Files.f[gFile.brick_yellow]); break;
                    case BrickColor.Invisible: image = Image.FromFile(Files.f[gFile.brick_invisible]); break;
                }
            }
        }
        public override bool canStandOn
        {
            get { return true; }
        }
        public override void Draw(Graphics g)
        {
            // Ceiling (round to x % 42 == 0)
            int rest = Width % 43;
            if (rest != 0)
            {
                if (rest < 21)
                    Width -= rest;
                else
                    Width += 43 - rest;
            }

            int occurences = Width / 43;
            int width = image.Width;
            for (int i = 0; i < occurences; i++)
            {
                g.DrawImage(image, Left + i * 43, Top, width, Height);
            }


            if (color == BrickColor.Invisible && World.PlayMode == PlayMode.Editor)
                g.DrawRectangle(Pens.Black, Left, Top, Width, Height);
        }

        public Bricks()
        {
        }
        public Bricks(int top, int left, BrickColor color)
        {
            Top = top;
            Left = left;

            Color = color;

            Width = image.Width * 5;
            Height = image.Height;
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
