using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace littleRunner.GameObjects.Enemies
{
    enum GumbaColor
    {
        Brown
    }
    class Gumba : Enemy
    {
        private GameDirection direction;
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
                        curimg = new AnimateImage (Files.gumba_brown, 100);
                        break;

                }
                Width = curimg.CurImage(direction).Width;
                Height = curimg.CurImage(direction).Height;
            }
        }
        [Category("Gumba")]
        public GameDirection Direction
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

            curimg.Draw(g, direction, Left, Top, Width, Height);
        }


        public Gumba()
            : base()
        {
            Direction = GameDirection.Right;
            small = 1;
        }

        public Gumba(int top, int left)
            : base()
        {
            Top = top;
            Left = left;

            Color = GumbaColor.Brown;

            Direction = GameDirection.Right;
            small = 1;
        }

        public void FrameChanged(object o, EventArgs e)
        {
            if (World != null)
                World.Invalidate();
        }

        public override void Check(out Dictionary<string, int> newpos)
        {
            base.Check(out newpos);
            int newtop = newpos["top"];
            int newleft = newpos["left"];


            // falling?
            bool falling = GamePhysics.Falling(World.StickyElements, World.MovingElements, this);

            if (falling)
                newtop += 6;

            // direction
            if (!falling)
            {
                if (direction == GameDirection.Right)
                    newleft += 1;
                else
                    newleft -= 1;
            }


            // check if direction is ok
            GamePhysics.CrashDetection(this, World.StickyElements, World.MovingElements, getEvent, ref newtop, ref newleft);
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
                direction = direction == GameDirection.Left ? GameDirection.Right : GameDirection.Left;
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
            AiEventHandler(gevent, args);
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
            Direction = (GameDirection)ser["Direction"];
        }
    }
}
