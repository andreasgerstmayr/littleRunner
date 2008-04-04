using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace littleRunner
{
    class Gumba : Enemy
    {
        private GameRunDirection direction;
        private int jumping;
        private Image curimg;
        private int small;

        public override bool canFire
        {
            get { return true; }
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

            g.DrawImage(curimg, Left, Top, Width, Height);
        }

        public GameRunDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public Gumba()
            : base()
        {
            curimg = new Bitmap(1, 1);

            jumping = 0;
            Direction = GameRunDirection.Right;
            small = 1;
        }

        public Gumba(int top, int left, Image img)
            : base()
        {
            Top = top;
            Left = left;

            Width = img.Width;
            Height = img.Height;

            curimg = img;

            jumping = 0;
            Direction = GameRunDirection.Right;
            small = 1;
        }

        public override void Init(World world)
        {
            base.Init(world);
            if (world.playMode)
                ImageAnimator.Animate(curimg, new EventHandler(FrameChanged));
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
            if (Top > World.Height)
                World.Enemies.Remove(this);
        }

        public override bool getCrashEvent(GameObject go, GameDirection cidirection)
        {
            if (cidirection == GameDirection.Top)
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
            ser["img"] = curimg;
            ser["Direction"] = Direction;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            curimg = (Image)ser["img"];
            Direction = (GameRunDirection)ser["Direction"];
        }
    }
}
