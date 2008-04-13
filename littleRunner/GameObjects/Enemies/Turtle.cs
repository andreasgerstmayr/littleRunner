using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

using littleRunner.GameObjects.MovingElements;


namespace littleRunner.GameObjects.Enemies
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
        private AnimateImage imgL, imgR, imgD;
        private AnimateImage curimg;
        private TurtleMode turtleMode;
        private int speed;
        private DateTime startSmall;
        private TurtleStyle style;

        public override bool fireCanDelete
        {
            get { return true; }
        }
        public override bool turtleCanRemove
        {
            get { return true; }
        }

        [Category("Turtle")]
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
        [Category("Turtle")]
        public GameRunDirection Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                switch (direction)
                {
                    case GameRunDirection.Left: curimg = imgL; break;
                    case GameRunDirection.Right: curimg = imgR; break;
                }
            }
        }

        public override void Draw(Graphics g)
        {
            curimg.Draw(g, Left, Top, curimg.CurImage.Width, Height);
        }


        public Turtle()
            : base()
        {
            //curimg = imgR;

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

            speed = 1;
            startSmall = DateTime.Now;

            Direction = GameRunDirection.Right;
            turtleMode = TurtleMode.Normal;
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

            if (turtleMode == TurtleMode.Small && (DateTime.Now - startSmall).Seconds >= 3)
            {
                speed = 1;
                turtleMode = TurtleMode.Normal;

                switch (direction)
                {
                    case GameRunDirection.Left: curimg = imgL; break;
                    case GameRunDirection.Right: curimg = imgR; break;
                }
            }


            // direction
            if (!falling)
            {
                if (direction == GameRunDirection.Right)
                    newleft += speed;
                else
                    newleft -= speed;
            }


            // check if direction is ok
            GamePhysics.CrashDetection(this, World.StickyElements, World.MovingElements, getEvent, ref newtop, ref newleft);
            Enemy crashedInEnemy = GamePhysics.CrashEnemy(this, World.Enemies, getEvent, ref newtop, ref newleft);

            bool removedEnemy = false;
            if (turtleMode == TurtleMode.SmallRunning && crashedInEnemy != null && crashedInEnemy.turtleCanRemove)
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

            if (!falling && (newleft == 0 || (crashedInEnemy != null && !removedEnemy)))
            {
                if (turtleMode == TurtleMode.Normal || turtleMode == TurtleMode.SmallRunning)
                {
                    direction = direction == GameRunDirection.Left ? GameRunDirection.Right : GameRunDirection.Left;
                    if (turtleMode == TurtleMode.Normal)
                    {
                        switch (direction)
                        {
                            case GameRunDirection.Left: curimg = imgL; break;
                            case GameRunDirection.Right: curimg = imgR; break;
                        }
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
                switch (turtleMode)
                {
                    case TurtleMode.Normal:
                        curimg = imgD;
                        turtleMode = TurtleMode.Small;
                        speed = 0;
                        startSmall = DateTime.Now;
                        break;
                    case TurtleMode.Small:
                        turtleMode = TurtleMode.SmallRunning;
                        speed = 20;
                        startSmall = DateTime.Now;
                        break;
                    case TurtleMode.SmallRunning:
                        turtleMode = TurtleMode.Small;
                        speed = 0;
                        startSmall = DateTime.Now;
                        break;
                }
                World.MGO.Move(MoveType.Jump);

                return true;
            }
            else if (cidirection == GameDirection.Left || cidirection == GameDirection.Right)
            {
                if (turtleMode == TurtleMode.Small)
                {
                    turtleMode = TurtleMode.SmallRunning;
                    direction = cidirection == GameDirection.Left ? GameRunDirection.Right : GameRunDirection.Left;
                    speed = 20;

                    return true;
                }
            }

            return false;
        }

        public void getEvent(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
            base.aiEventHandler(gevent, args);
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
