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
        private GameDirection direction;
        private AnimateImage imgRunning, imgShell;
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
                        imgRunning = new AnimateImage(Files.turtle_green, 200, GameDirection.Left, GameDirection.Right);
                        imgShell = new AnimateImage(Files.turtle_green_down, 200, GameDirection.None);
                        break;
                }

                TurtleMode = turtleMode; // set curimg
            }
        }
        [Category("Turtle")]
        public GameDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        [Category("Turtle"),Browsable(false)]
        public TurtleMode TurtleMode
        {
            get { return turtleMode; }
            set
            {
                if (value == TurtleMode.Normal)
                {
                    curimg = imgRunning;
                    if (turtleMode == TurtleMode.Small || turtleMode == TurtleMode.SmallRunning)
                        Top -= curimg.CurImage(direction).Height - Height;

                    Width = curimg.CurImage(direction).Width;
                    Height = curimg.CurImage(direction).Height;
                }
                else if (value == TurtleMode.Small || value == TurtleMode.SmallRunning)
                {
                    curimg = imgShell;
                    Width = curimg.CurImage(GameDirection.None).Width;
                    Height = curimg.CurImage(GameDirection.None).Height;
                }

                turtleMode = value;
            }
        }

        public override void Draw(Graphics g)
        {
            GameDirection dir = turtleMode == TurtleMode.Normal ? direction : GameDirection.None;
            curimg.Draw(g, dir, Left, Top, curimg.CurImage(dir).Width, Height);
        }


        public Turtle()
            : base()
        {
            speed = 1;
            startSmall = DateTime.Now;
        }

        public Turtle(int top, int left, TurtleStyle style)
            : base()
        {
            Top = top;
            Left = left;

            Style = style;

            speed = 1;
            startSmall = DateTime.Now;

            Direction = GameDirection.Right;
            TurtleMode = TurtleMode.Normal;
        }

        public override void Remove()
        {
            base.Remove();

            Dictionary<GameEventArg, object> pointsArgs = new Dictionary<GameEventArg, object>();
            pointsArgs[GameEventArg.points] = 5;
            AiEventHandler(GameEvent.gotPoints, pointsArgs);
        }

        public override void Check(out Dictionary<string, int> newpos)
        {
            base.Check(out newpos);
            int newtop = newpos["top"];
            int newleft = newpos["left"];


            // falling?
            bool falling = GamePhysics.Falling(World.StickyElements, World.MovingElements, World.Enemies, newtop, newleft, this);

            if (falling)
                newtop += 6;

            if (turtleMode == TurtleMode.Small && (DateTime.Now - startSmall).Seconds >= 3)
            {
                speed = 1;
                TurtleMode = TurtleMode.Normal;
            }


            // direction
            if (!falling)
            {
                if (direction == GameDirection.Right)
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
                base.Remove(crashedInEnemy);
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
                    direction = direction == GameDirection.Left ? GameDirection.Right : GameDirection.Left;
            }


            // dead?
            if (Top > World.Settings.LevelHeight)
                base.Remove();
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
                        TurtleMode = TurtleMode.Small;
                        speed = 0;
                        startSmall = DateTime.Now;
                        break;
                    case TurtleMode.Small:
                        TurtleMode = TurtleMode.SmallRunning;
                        speed = 20;
                        startSmall = DateTime.Now;
                        break;
                    case TurtleMode.SmallRunning:
                        TurtleMode = TurtleMode.Small;
                        speed = 0;
                        startSmall = DateTime.Now;
                        break;
                }
                World.MGO.Move(MoveType.jumpTop, -1, GameInstruction.Nothing);

                return true;
            }
            else if (cidirection == GameDirection.Left || cidirection == GameDirection.Right)
            {
                if (turtleMode == TurtleMode.Small)
                {
                    turtleMode = TurtleMode.SmallRunning;
                    direction = cidirection == GameDirection.Left ? GameDirection.Right : GameDirection.Left;
                    speed = 20;

                    return true;
                }
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
            ser["TurtleStyle"] = style;
            ser["Direction"] = Direction;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            Style = (TurtleStyle)ser["TurtleStyle"];
            Direction = (GameDirection)ser["Direction"];
        }
    }
}
