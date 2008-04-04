using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace littleRunner.GameObjects
{
    abstract class StickyImageElement : StickyElement
    {
        protected Image curimg;

        public override void Draw(Graphics g)
        {
            g.DrawImage(curimg, Left, Top, Width, Height);
        }

        public StickyImageElement()
        {
        }
        public StickyImageElement(int top, int left, Image img)
        {
            Top = top;
            Left = left;

            Width = img.Width;
            Height = img.Height;

            curimg = img;
        }


        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["Image"] = curimg;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            curimg = (Image)ser["Image"];
        }
    }


    class PlainFloor : StickyElement
    {
        Color color;
        Brush brush;

        public override void Draw(Graphics g)
        {
            g.FillRectangle(brush, Left, Top, Width, Height);
        }
        override public bool canStandOn
        {
            get { return true; }
        }
        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                brush = new SolidBrush(color);
            }
        }

        public PlainFloor()
        {
        }
        public PlainFloor(Color color, int top, int left, int width, int height)
        {
            Top = top;
            Left = left;

            Width = width;
            Height = height;

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
            Color = (Color)ser["Color"];
        }
    }

    class Floor : StickyElement
    {
        private Image imgL, imgM, imgR;

        public override void Draw(Graphics g)
        {
            // Ceiling (round to x % 64 == 0)
            int rest = Width % 64;
            if (rest != 0)
            {
                if (rest < 32)
                    Width -= rest;
                else
                    Width += 64-rest;
            }

            g.DrawImage(imgL, Left, Top, imgL.Width, Height);

            int occurences = Width / 64 - 2;
            for (int i = 0; i < occurences; i++)
            {
                g.DrawImage(imgM, Left + imgL.Width-2 + i*62, Top, imgM.Width, Height);
            }

            g.DrawImage(imgR, Left+imgL.Width - 2 + occurences*62, Top, imgR.Width, Height);
        }


        override public bool canStandOn
        {
            get { return true; }
        }

        public Floor()
            : base()
        {
        }
        public Floor(Image imgL, Image imgM, Image imgR, int top, int left)
            : base()
        {
            Top = top;
            Left = left;

            Width = imgL.Width+imgM.Width+imgR.Width;
            Width = 192;
            Height = imgM.Height;

            this.imgL = imgL;
            this.imgM = imgM;
            this.imgR = imgR;
        }

        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["imgL"] = imgL;
            ser["imgM"] = imgM;
            ser["imgR"] = imgR;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            imgL = (Image)ser["imgL"];
            imgM = (Image)ser["imgM"];
            imgR = (Image)ser["imgR"];
        }
    }


    class Star : StickyImageElement
    {
        override public bool canStandOn
        {
            get { return false; }
        }
        override public void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
            base.onOver(geventhandler, who, direction);

            geventhandler(GameEvent.gotPoint, new Dictionary<GameEventArg, object>());
            World.StickyElements.Remove(this);
        }


        public Star() : base()
        {
        }
        public Star(int top, int left, Image img)
            : base(top, left, img)
        {
        }
    }

    class FireFlower : StickyImageElement
    {
        override public bool canStandOn
        {
            get { return false; }
        }
        override public void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
            base.onOver(geventhandler, who, direction);

            if (who == GameElement.MGO)
            {
                geventhandler(GameEvent.gotFireFlower, new Dictionary<GameEventArg, object>());
                World.StickyElements.Remove(this);
            }
        }


        public FireFlower()
            : base()
        {
        }
        public FireFlower(int top, int left)
            : base(top-Image.FromFile(Properties.Resources.fire_flower).Height, left,
                   Image.FromFile(Properties.Resources.fire_flower))
        {
        }
    }


    class Brick : StickyImageElement
    {
        override public bool canStandOn
        {
            get { return true; }
        }

        public Brick()
            : base()
        {
        }
        public Brick(int top, int left, Image img)
            : base(top, left, img)
        {
        }
    }


    enum BoxType
    {
        GoodMushroom,
        PoisonMushroom,
        FireFlower
    }

    class Box : StickyImageElement
    {
        BoxType btype;
        bool got;

        override public bool canStandOn
        {
            get { return true; }
        }

        public BoxType BoxType
        {
            get { return btype; }
            set { btype = value; }
        }

        public override void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
            base.onOver(geventhandler, who, direction);
            if (!got && direction == GameDirection.Bottom && who == GameElement.MGO)
            {
                if (btype == BoxType.GoodMushroom)
                {
                    Mushroom m = new Mushroom(MushroomType.Good, Top, Left);
                    m.Init(World);
                    World.MovingElements.Add(m);
                }
                else if (btype == BoxType.PoisonMushroom)
                {
                    Mushroom m = new Mushroom(MushroomType.Poison, Top, Left);
                    m.Init(World);
                    World.MovingElements.Add(m);
                }
                else if (btype == BoxType.FireFlower)
                {
                    FireFlower f = new FireFlower(Top, Left);
                    f.Init(World);
                    World.StickyElements.Add(f);
                }
                got = true;
            }
        }

        public Box()
            : base()
        {
            got = false;
        }
        public Box(int top, int left, Image img)
            : base(top, left, img)
        {
            got = false;
        }


        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["BoxType"] = btype;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            btype = (BoxType)ser["BoxType"];
        }
    }


    class Pipe : StickyImageElement
    {
        string nextWorld;
        override public bool canStandOn
        {
            get { return true; }
        }
        public string NextWorld
        {
            get { return nextWorld; }
            set { nextWorld = value; }
        }

        public Pipe() : base()
        {
        }
        public Pipe(int top, int left, Image img)
            : base(top, left, img)
        {
        }


        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
        }
    }


    class DesignElement : StickyImageElement
    {
        override public bool canStandOn
        {
            get { return false; }
        }

        public DesignElement() : base()
        {
        }
        public DesignElement(int top, int left, Image img)
            : base(top, left, img)
        {
        }
    }


    class LevelEnd : StickyImageElement
    {
        string nextWorld;
        public string NextWorld
        {
            get { return nextWorld; }
            set { nextWorld = value; }
        }
        public override bool canStandOn
        {
            get { return false; }
        }

        override public void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
            base.onOver(geventhandler, who, direction);

            if (who == GameElement.MGO)
            {
                Dictionary<GameEventArg, object> args = new Dictionary<GameEventArg, object>();
                args[GameEventArg.nextLevel] = nextWorld;

                geventhandler(GameEvent.finishedLevel, args);
            }
        }

        public LevelEnd() : base()
        {
        }
        public LevelEnd(int top, int left, Image img)
            : base(top, left, img)
        {
        }


        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["NextWorld"] = nextWorld;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            nextWorld = (string)ser["NextWorld"];
        }
    }


    class House : LevelEnd
    {
        public House() : base()
        {
        }
        public House(int top, int left, Image img)
            : base(top, left, img)
        {
        }
    }
}
