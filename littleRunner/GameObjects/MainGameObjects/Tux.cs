using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using littleRunner.GameObjects.MovingElements;


namespace littleRunner.GameObjects.MainGameObjects
{
    class Tux : MainGameObject
    {
        private AnimateImage curimg;
        private GameDirection direction;
        private int jumping;
        private bool firePressed;
        private MainGameObjectMode mode;
        private DateTime immortializeStart;
        private bool immortialize;
        private int blink;
        private AnimateImage imgNormal, imgSmall;
        private MoveType wantNextMove;
        private int wantNextMoveLength;
        private GameInstruction moveDoThen;


        public override void Draw(Graphics g)
        {
            if (!immortialize || blink % 8 == 0)
                curimg.Draw(g, direction, Left, Top, Width, Height);

            if (immortialize)
                blink++;
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

                blink = 0;
            }
        }
        public GameDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public Tux(int top, int left)
        {
            Top = top;
            Left = left;

            imgNormal = new AnimateImage(Files.tux_normal, 200);
            imgSmall = new AnimateImage(Files.tux_small, 200);


            Width = imgNormal.CurImage(direction).Width;
            Height = imgNormal.CurImage(direction).Height;

            curimg = imgNormal;
            direction = GameDirection.Right;

            firePressed = false;
            mode = MainGameObjectMode.Normal;
            immortializeStart = DateTime.Now;
            immortialize = false;
            jumping = -1;

            blink = 0;
            wantNextMove = MoveType.Nothing;
        }


        public override void Check(List<GameKey> pressedKeys)
        {
            int newtop = 0;
            int newleft = 0;


            if (wantNextMove != MoveType.Nothing)
            {
                switch (wantNextMove)
                {
                    case MoveType.Jump:
                        jumping = 100;
                        break;
                    case MoveType.goLeft:
                        newleft -= wantNextMoveLength;
                        break;
                    case MoveType.goRight:
                        newleft += wantNextMoveLength;
                        break;
                    case MoveType.goTop:
                        newtop -= wantNextMoveLength;
                        break;
                    case MoveType.goBottom:
                        newtop += wantNextMoveLength;
                        break;
                }

                moveDoThen.Do();
                wantNextMove = MoveType.Nothing;
            }


            if (pressedKeys.Contains(GameKey.jumpTop))
            {
            }

            // falling? (need for jumping-if)
            bool falling = GamePhysics.Falling(World.StickyElements, World.MovingElements, newtop, newleft, this);


            // immortialize end?
            if (immortialize && (DateTime.Now - immortializeStart).Seconds >= 1)
            {
                immortialize = false;
                blink = 0;
            }


            // key pressed?
            if (pressedKeys.Contains(GameKey.goLeft) && (jumping == -1 || (jumping >= 100 && jumping <= 140)))
            {
                newleft -= 7;
                if (direction != GameDirection.Left)
                    Direction = GameDirection.Left;
            }
            if (pressedKeys.Contains(GameKey.goRight) && (jumping == -1 || (jumping >= 100 && jumping <= 140)))
            {
                newleft += 7;
                if (direction != GameDirection.Right)
                    Direction = GameDirection.Right;
            }
            if (pressedKeys.Contains(GameKey.jumpLeft) && !falling)
            {
                jumping = 0;
                if (direction != GameDirection.Left)
                    Direction = GameDirection.Left;
            }
            if (pressedKeys.Contains(GameKey.jumpRight) && !falling)
            {
                jumping = 200;
                if (direction != GameDirection.Right)
                    Direction = GameDirection.Right;
            }
            if (pressedKeys.Contains(GameKey.jumpTop) && !falling)
            {
                jumping = 100;
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

            // jumping?
            GamePhysics.Jumping(ref jumping, ref newtop, ref newleft);


            // save jumping state
            int jumpingTop = newtop;
            int jumpingLeft = newleft;


            // now we can fall (if we don't jump)
            if (falling && jumping == -1)
                newtop += 7;


            // check if direction is ok
            GamePhysics.CrashDetection(this, World.StickyElements, World.MovingElements, getEvent, ref newtop, ref newleft);
            bool crashedInEnemy = GamePhysics.CrashEnemy(this, World.Enemies, getEvent, ref newtop, ref newleft) == null ? false : true;

            if (crashedInEnemy)
                return;


            // check if want-Jump-Top/Left = current (jump in box etc)
            if (newtop != jumpingTop || newleft != jumpingLeft)
                jumping = -1;


            if (newtop != 0)
                Top += newtop;
            if (newleft != 0)
                Left += newleft;
        }

        public override void Move(MoveType mtype, int length, GameInstruction doThen)
        {
            wantNextMove = mtype;
            wantNextMoveLength = length;
            this.moveDoThen = doThen;
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
                immortialize = true;
                immortializeStart = DateTime.Now;
            }
            else if (gevent == GameEvent.gotGoodMushroom)
            {
                if (mode == MainGameObjectMode.Small)
                    Mode = MainGameObjectMode.Normal;
            }
            else if (gevent == GameEvent.gotPoisonMushroom)
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
            else if (gevent == GameEvent.finishedLevel)
            {
                AiEventHandler(gevent, args);
            }
        }
    }
}
