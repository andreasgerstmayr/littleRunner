using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace littleRunner
{
    enum GumbaColor
    {
        Brown
    }
    class Gumba : Enemy
    {
        private GameRunDirection direction;
        private int jumping;
        private AnimateImage curimg;
        private int small;
        private GumbaColor color;

        public override bool fireCanDelete
        {
            get { return true; }
        }
        public override bool turtleCanRemove
        {
            get { return true; }
        }
        [Category("Gumba")]
        public GumbaColor Color
        {
            get { return color; }
            set
            {
                color = value;
                switch (color)
                {
                    case GumbaColor.Brown:
                        curimg = new AnimateImage (Files.f[gFile.gumba_brown], 100, GameDirection.Left);
                        break;

                }
                Width = curimg.CurImage.Width;
                Height = curimg.CurImage.Height;
            }
        }
        [Category("Gumba")]
        public GameRunDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        public override void Draw(Graphics g)
        {
            if (small > 1)
            {
                Top += small*2;
                Height -= small*2;
                small++;
            }

            if (Height <= 2)
                World.Enemies.Remove(this);

            curimg.Draw(g, Left, Top, Width, Height);
        }


        public Gumba()
            : base()
        {
            jumping = 0;
            Direction = GameRunDirection.Right;
            small = 1;
        }

        public Gumba(int top, int left)
            : base()
        {
            Top = top;
            Left = left;

            Color = GumbaColor.Brown;

            jumping = 0;
            Direction = GameRunDirection.Right;
            small = 1;
        }

        public void FrameChanged(object o, EventArgs e)
        {
            if (World != null)
                World.Invalidate();
        }

        public override void Check()
        {
            int newtop = 0;
            int newleft = 0;

            // falling?
            bool falling = GamePhysics.Falling(World.StickyElements, this);

            if (falling)
                newtop += 6;

            // direction
            if (!falling)
            {
                if (direction == GameRunDirection.Right)
                    newleft += 1;
                else
                    newleft -= 1;
            }

            // jumping?
            GamePhysics.Jumping(ref jumping, ref newtop, ref newleft);

            // check if direction is ok
            GamePhysics.CrashDetection(this, World.MovingElements, World.StickyElements, getEvent, ref newtop, ref newleft);
            Enemy crashedInEnemy = GamePhysics.CrashEnemy(this, World.Enemies, getEvent, ref newtop, ref newleft);


            // run in standing mgo?
            GameDirection _direction;
            if (GamePhysics.SingleCrashDetection(this, World.MGO, out _direction, ref newtop, ref newleft, true) && !getCrashEvent(this, _direction))
                World.MGO.getEvent(GameEvent.crashInEnemy, new Dictionary<GameEventArg, object>());


            if (newtop != 0)
                Top += newtop;
            if (newleft != 0)
                Left += newleft;

            if (!falling && newleft == 0)
            {
                direction = direction == GameRunDirection.Left ? GameRunDirection.Right : GameRunDirection.Left;
            }


            // dead?
            if (Top > World.Settings.LevelHeight)
                World.Enemies.Remove(this);
        }

        public override bool getCrashEvent(GameObject go, GameDirection cidirection)
        {
            if (go is MainGameObject && cidirection == GameDirection.Top)
            {
                small = 2;
                return true;
            }

            return false;
        }

        public void getEvent(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
        }


        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["Color"] = color;
            ser["Direction"] = Direction;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            Color = (GumbaColor)ser["Color"];
            Direction = (GameRunDirection)ser["Direction"];
        }
    }
}
