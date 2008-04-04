using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

namespace littleRunner
{
    enum TurtleMode
    {
        Normal,
        Small,
        SmallRunning
    }

    class Turtle : Enemy
    {
        private GameRunDirection direction;
        private int jumping;
        private Image imgL, imgR;
        private Image curimg;
        private TurtleMode turtleMode;
        private int speed;
        private DateTime startSmall;

        public override bool canFire
        {
            get { return true; }
        }
        public override void Draw(Graphics g)
        {
            g.DrawImage(curimg, Left, Top, Width, Height);
        }

        public GameRunDirection Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                if (value == GameRunDirection.Left)
                    curimg = imgL;
                else if (value == GameRunDirection.Right)
                    curimg = imgR;
            }
        }

        public Turtle()
            : base()
        {
            curimg = imgR;

            jumping = 0;
            speed = 1;
            startSmall = DateTime.Now;

            Direction = GameRunDirection.Right;
            turtleMode = TurtleMode.Normal;
        }

        public Turtle(int top, int left, Image imgL, Image imgR)
            : base()
        {
            Top = top;
            Left = left;

            Width = imgR.Width;
            Height = imgR.Height;

            this.imgL = imgL;
            this.imgR = imgR;
            curimg = imgR;

            jumping = 0;
            speed = 1;
            startSmall = DateTime.Now;

            Direction = GameRunDirection.Right;
            turtleMode = TurtleMode.Normal;
        }

        public override void Init(World world)
        {
            base.Init(world);
            if (world.playMode)
            {
                ImageAnimator.Animate(imgL, new EventHandler(FrameChanged));
                ImageAnimator.Animate(imgR, new EventHandler(FrameChanged));
            }
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

            if (turtleMode == TurtleMode.Small && (DateTime.Now - startSmall).Seconds >= 3)
            {
                speed = 1;
                turtleMode = TurtleMode.Normal;

                if (direction == GameRunDirection.Left)
                    curimg = imgL;
                else if (direction == GameRunDirection.Right)
                    curimg = imgR;
            }


            // direction
            if (!falling)
            {
                if (direction == GameRunDirection.Right)
                    newleft += speed;
                else
                    newleft -= speed;
            }

            // jumping?
            GamePhysics.Jumping(ref jumping, ref newtop, ref newleft);

            // check if direction is ok
            GamePhysics.CrashDetection(this, World.MovingElements, World.StickyElements, getEvent, ref newtop, ref newleft);
            Enemy crashedInEnemy = GamePhysics.CrashEnemy(this, World.Enemies, getEvent, ref newtop, ref newleft);

            bool removedEnemy = false;
            if (turtleMode == TurtleMode.SmallRunning && crashedInEnemy != null)
            {
                World.Enemies.Remove(crashedInEnemy);
                removedEnemy = true;
            }


            // run in standing mgo?
            GameDirection _direction;
            if (GamePhysics.SingleCrashDetection(this, World.MGO, out _direction, ref newtop, ref newleft, true) && !getCrashEvent(this, _direction))
                World.MGO.getEvent(GameEvent.crashInEnemy, new Dictionary<GameEventArg, object>());


            if (newtop != 0)
                Top += newtop;
            if (newleft != 0)
                Left += newleft;

            if (!falling && (newleft == 0 || (crashedInEnemy!=null && !removedEnemy)))
            {
                if (turtleMode == TurtleMode.Normal || turtleMode == TurtleMode.SmallRunning)
                {
                    direction = direction == GameRunDirection.Left ? GameRunDirection.Right : GameRunDirection.Left;
                    if (turtleMode == TurtleMode.Normal)
                    {
                        if (direction == GameRunDirection.Left)
                            curimg = imgL;
                        else if (direction == GameRunDirection.Right)
                            curimg = imgR;
                    }
                }
            }


            // dead?
            if (Top > World.Height)
                World.Enemies.Remove(this);
        }

        public override bool getCrashEvent(GameObject go, GameDirection cidirection)
        {
            if ((go is Enemy && go != this) || go is Mushroom)
               return true;

            if (cidirection == GameDirection.Top)
            {
                if (turtleMode == TurtleMode.Normal)
                {
                    curimg = Image.FromFile(Properties.Resources.turtle_green_down);
                    turtleMode = TurtleMode.Small;
                    World.MGO.Move(true);
                    speed = 0;
                    startSmall = DateTime.Now;
                }
                else if (turtleMode == TurtleMode.Small)
                {
                    turtleMode = TurtleMode.SmallRunning;
                    World.MGO.Move(true);
                    speed = 20;
                    startSmall = DateTime.Now;
                }
                else if (turtleMode == TurtleMode.SmallRunning)
                {
                    turtleMode = TurtleMode.Small;
                    World.MGO.Move(true);
                    speed = 0;
                    startSmall = DateTime.Now;
                }

                return true;
            }
            else if (cidirection == GameDirection.Left || cidirection == GameDirection.Right)
            {
                if (turtleMode == TurtleMode.Small)
                {
                    turtleMode = TurtleMode.SmallRunning;
                    direction = cidirection==GameDirection.Left ? GameRunDirection.Right: GameRunDirection.Left;
                    speed = 20;

                    return true;
                }
            }

            return false;
        }

        public void getEvent(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
        }


        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["imgL"] = imgL;
            ser["imgR"] = imgR;
            ser["Direction"] = Direction;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            imgL = (Image)ser["imgL"];
            imgR = (Image)ser["imgR"];
            Direction = (GameRunDirection)ser["Direction"];
        }
    }
}
