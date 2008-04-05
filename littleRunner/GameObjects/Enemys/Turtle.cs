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
    enum TurtleStyle
    {
        Green
    }
    class Turtle : Enemy
    {
        private GameRunDirection direction;
        private int jumping;
        private AnimateImage imgL, imgR, imgD;
        private AnimateImage curimg;
        private TurtleMode turtleMode;
        private int speed;
        private DateTime startSmall;
        private TurtleStyle style;

        public TurtleStyle Style
        {
            get { return style; }
            set
            {
                style = value;
                switch (style)
                {
                    case TurtleStyle.Green:
                        imgL = new AnimateImage(Files.f[gFile.turtle_green], 200, GameDirection.Left);
                        imgR = new AnimateImage(Files.f[gFile.turtle_green], 200, GameDirection.Right);
                        imgD = new AnimateImage(Files.f[gFile.turtle_green_down], 200, GameDirection.None);
                        break;
                }

                Width = imgR.CurImage.Width;
                Height = imgR.CurImage.Height;
            }
        }

        public override bool canFire
        {
            get { return true; }
        }
        public override void Draw(Graphics g)
        {
            curimg.Draw(g, Left, Top, curimg.CurImage.Width, Height);
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
            //curimg = imgR;

            jumping = 0;
            speed = 1;
            startSmall = DateTime.Now;

            Direction = GameRunDirection.Right;
            turtleMode = TurtleMode.Normal;
        }

        public Turtle(int top, int left)
            : base()
        {
            Top = top;
            Left = left;

            Style = TurtleStyle.Green;
            curimg = imgR;

            jumping = 0;
            speed = 1;
            startSmall = DateTime.Now;

            Direction = GameRunDirection.Right;
            turtleMode = TurtleMode.Normal;
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
            if (Top > World.Settings.LevelHeight)
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
                    curimg = imgD;
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
            ser["TurtleStyle"] = style;
            ser["Direction"] = Direction;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            Style = (TurtleStyle)ser["TurtleStyle"];
            Direction = (GameRunDirection)ser["Direction"];
        }
    }
}
