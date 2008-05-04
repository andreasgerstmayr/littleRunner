using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using littleRunner.GameObjects.MovingElements;


namespace littleRunner.GameObjects.MainGameObjects
{
    struct WantNext
    {
        public MoveType type;
        public int value;
        public GameInstruction instruction;
    }

    class Tux : MainGameObject
    {
        private AnimateImage curimg;
        private GameDirection direction;
        private GamePhysics.JumpData jumping;
        private bool firePressed;
        private MainGameObjectMode mode;
        private DateTime immortializeEnd;
        private bool immortialize;
        private AnimateImage imgNormal, imgSmall;

        private Queue<WantNext> wantNext;


        public override void Draw(Graphics g)
        {
            if (!immortialize || DateTime.Now.Millisecond % 4 == 0)
                curimg.Draw(g, direction, Left, Top, Width, Height);
        }

        public override MainGameObjectMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;
                Direction = direction;

                int lastHeight = curimg.CurImage(direction).Height;

                switch (mode)
                {
                    case MainGameObjectMode.NormalFire:
                    case MainGameObjectMode.Normal:
                        curimg = imgNormal;
                        break;
                    case MainGameObjectMode.Small:
                        curimg = imgSmall;
                        break;
                }
                Top -= curimg.CurImage(direction).Height - lastHeight;
                Height = curimg.CurImage(direction).Height;
            }
        }
        public override GameDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public Tux(int top, int left)
            : base()
        {
            Top = top;
            Left = left;

            imgNormal = new AnimateImage(Files.tux_normal, 200, GameDirection.Left, GameDirection.Right);
            imgSmall = new AnimateImage(Files.tux_small, 200, GameDirection.Left, GameDirection.Right);


            Width = imgNormal.CurImage(direction).Width;
            Height = imgNormal.CurImage(direction).Height;

            curimg = imgNormal;
            direction = GameDirection.Right;

            firePressed = false;
            mode = MainGameObjectMode.Normal;

            immortializeEnd = DateTime.Now;
            immortialize = false;
            
            GamePhysics.JumpData jumping = new GamePhysics.JumpData();
            jumping.direction = GameDirection.None;
            jumping.value = 1;
            this.jumping = jumping;

            wantNext = new Queue<WantNext>();
        }

        public override Dictionary<string,int> Check(List<GameKey> pressedKeys)
        {
            int newtop = 0;
            int newleft = 0;


            if (wantNext.Count > 0)
            {
                WantNext next = wantNext.Peek();

                switch (next.type)
                {
                    case MoveType.goLeft:
                        newleft -= next.value;
                        break;
                    case MoveType.goRight:
                        newleft += next.value;
                        break;
                    case MoveType.goTop:
                        newtop -= next.value;
                        break;
                    case MoveType.goBottom:
                        newtop += next.value;
                        break;
                    case MoveType.jumpLeft:
                        jumping.direction = GameDirection.Left;
                        jumping.value = 1;
                        break;
                    case MoveType.jumpTop:
                        jumping.direction = GameDirection.Top;
                        jumping.value = 1;
                        break;
                    case MoveType.jumpRight:
                        jumping.direction = GameDirection.Right;
                        jumping.value = 1;
                        break;
                }

                wantNext.Dequeue();
                next.instruction.Do();
            }


            // falling? (need for jumping-if)
            bool falling = GamePhysics.Falling(World.StickyElements, World.MovingElements, World.Enemies, newtop, newleft, this);


            // immortialize end?
            if (immortialize && DateTime.Now > immortializeEnd)
                immortialize = false;



            #region key pressed?
            if (pressedKeys.Contains(GameKey.goLeft) &&
                (jumping.direction == GameDirection.None || jumping.direction == GameDirection.Top))
            {
                newleft -= 7;
                if (direction != GameDirection.Left)
                    Direction = GameDirection.Left;
            }
            if (pressedKeys.Contains(GameKey.goRight) &&
                (jumping.direction == GameDirection.None || jumping.direction == GameDirection.Top))
            {
                newleft += 7;
                if (direction != GameDirection.Right)
                    Direction = GameDirection.Right;
            }
            if (pressedKeys.Contains(GameKey.jumpLeft) && !falling)
            {
                jumping.direction = GameDirection.Left;
                jumping.value = 1;
                if (direction != GameDirection.Left)
                    Direction = GameDirection.Left;
            }
            if (pressedKeys.Contains(GameKey.jumpRight) && !falling)
            {
                jumping.direction = GameDirection.Right;
                jumping.value = 1;
                if (direction != GameDirection.Right)
                    Direction = GameDirection.Right;
            }
            if (pressedKeys.Contains(GameKey.jumpTop) && !falling)
            {
                jumping.direction = GameDirection.Top;
                jumping.value = 1;
            }

            if (pressedKeys.Contains(GameKey.fire) && mode == MainGameObjectMode.NormalFire)
            {
                if (!firePressed)
                {
                    firePressed = true;
                    int startFireLeft = direction == GameDirection.Right ? Right + 5 : Left - 5;

                    Fire f = new Fire(direction, Top + 20, startFireLeft);
                    f.Init(World, AiEventHandler);
                    World.MovingElements.Add(f);
                }
            }
            else
                firePressed = false;
            #endregion

            // jumping?
            GamePhysics.Jumping(ref jumping, ref newtop, ref newleft);

            // now we can fall (if we don't jump)
            if (falling && jumping.direction == GameDirection.None)
                newtop += 7;


            // check if direction is ok
            GamePhysics.CrashDetection(this, World.StickyElements, World.MovingElements, getEvent, ref newtop, ref newleft);
            bool crashedInEnemy = GamePhysics.CrashEnemy(this, World.Enemies, getEvent, ref newtop, ref newleft) == null ? false : true;


            // check if jump in box etc.
            if (jumping.direction != GameDirection.None &&
                (newtop == 0 && jumping.direction == GameDirection.Top)
                )
                jumping.direction = GameDirection.None;


            if (newtop != 0)
                Top += newtop;
            if (newleft != 0)
                Left += newleft;


            // out of range?
            if (Top > World.Settings.LevelHeight)
                AiEventHandler(GameEvent.outOfRange, new Dictionary<GameEventArg, object>());

            return new Dictionary<string, int>() { { "newtop", newtop }, { "newleft", newleft } };
        }

        public override void Move(MoveType mtype, int value, GameInstruction instruction)
        {
            WantNext next = new WantNext();
            next.type = mtype;
            next.value = value;
            next.instruction = instruction;

            wantNext.Enqueue(next);
        }

        public override void getEvent(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
            base.getEvent(gevent, args); // calls implizit AiEventHandler!


            if (gevent == GameEvent.crashInEnemy && !immortialize)
            {
                switch (mode)
                {
                    case MainGameObjectMode.NormalFire:
                    case MainGameObjectMode.Normal:
                        Mode = MainGameObjectMode.Small;
                        break;
                    case MainGameObjectMode.Small:
                        AiEventHandler(GameEvent.dead, args);
                        break;
                }
                immortializeEnd = DateTime.Now.AddSeconds(1);
                immortialize = true;
            }
            else if (gevent == GameEvent.gotGoodMushroom)
            {
                if (mode == MainGameObjectMode.Small)
                    Mode = MainGameObjectMode.Normal;
            }
            else if (gevent == GameEvent.gotPoisonMushroom && !immortialize)
            {
                if (mode == MainGameObjectMode.NormalFire || mode == MainGameObjectMode.Normal)
                    Mode = MainGameObjectMode.Small;
                else if (mode == MainGameObjectMode.Small)
                    AiEventHandler(GameEvent.dead, args);
            }
            else if (gevent == GameEvent.gotFireFlower)
            {
                Mode = MainGameObjectMode.NormalFire;
            }
            else if (gevent == GameEvent.gotImmortialize)
            {
                immortializeEnd = DateTime.Now.AddSeconds(5);
                immortialize = true;
            }
        }
    }
}
