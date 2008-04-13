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
        private GameRunDirection direction;
        private int jumping;
        private bool firePressed;
        private MainGameObjectMode mode;
        private DateTime immortializeStart;
        private bool immortialize;
        private int blink;
        private AnimateImage imgL, imgR;
        private MoveType wantNextMove;


        public override void Draw(Graphics g)
        {
            if (!immortialize || blink % 8 == 0)
                curimg.Draw(g, Left, Top, Width, Height);

            if (immortialize)
                blink++;
        }

        private MainGameObjectMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;
                switch (mode)
                {
                    case MainGameObjectMode.NormalFire:
                        Height = curimg.CurImage.Height;
                        break;
                    case MainGameObjectMode.Normal:
                        if (Height == 44)
                            Top -= curimg.CurImage.Height - 44;
                        Height = curimg.CurImage.Height;
                        break;
                    case MainGameObjectMode.Small:
                        Top += curimg.CurImage.Height - 44;
                        Height = 44;
                        break;
                }
                blink = 0;
            }
        }
        public override MainGameObjectMode currentMode
        {
            get { return Mode; }
            set { Mode = value; }
        } 

        public Tux(int top, int left)
        {
            Top = top;
            Left = left;

            imgL = new AnimateImage(Files.f[gFile.tux], 200, GameDirection.Left);
            imgR = new AnimateImage(Files.f[gFile.tux], 200, GameDirection.Right);


            Width = imgR.CurImage.Width;
            Height = imgR.CurImage.Height;

            curimg = imgR;
            direction = GameRunDirection.Right;

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
                        newleft = -20;
                        break;
                    case MoveType.goRight:
                        newleft = 20;
                        break;
                    case MoveType.goTop:
                        newtop -= 20;
                        break;
                    case MoveType.goBottom:
                        newtop = 20;
                        break;
                }

                wantNextMove = MoveType.Nothing;
            }

            // falling? (need for jumping-if)
            bool falling = GamePhysics.Falling(World.StickyElements, World.MovingElements, this);

            if (falling)
                newtop += 7;


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
                if (direction != GameRunDirection.Left)
                {
                    direction = GameRunDirection.Left;
                    curimg = imgL;
                }
            }
            if (pressedKeys.Contains(GameKey.goRight) && (jumping == -1 || (jumping >= 100 && jumping <= 140)))
            {
                newleft += 7;
                if (direction != GameRunDirection.Right)
                {
                    direction = GameRunDirection.Right;
                    curimg = imgR;
                }
            }
            if (pressedKeys.Contains(GameKey.jumpLeft) && !falling)
            {
                jumping = 0;
                if (direction != GameRunDirection.Left)
                {
                    direction = GameRunDirection.Left;
                    curimg = imgL;
                }
            }
            if (pressedKeys.Contains(GameKey.jumpRight) && !falling)
            {
                jumping = 200;
                if (direction != GameRunDirection.Right)
                {
                    direction = GameRunDirection.Right;
                    curimg = imgR;
                }
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
                    int startFireLeft = direction == GameRunDirection.Right ? Right + 5 : Left - 5;

                    Fire f = new Fire(direction, Top + 20, startFireLeft);
                    f.Init(World);
                    f.aiEventHandler = aiEventHandler;
                    World.MovingElements.Add(f);
                }
            }
            else
                firePressed = false;

            // jumping?
            GamePhysics.Jumping(ref jumping, ref newtop, ref newleft);


            // falling & jumping? no! balance it.
            if (falling && jumping != -1)
                newtop -= 7;


            // check if direction is ok
            GamePhysics.CrashDetection(this, World.StickyElements, World.MovingElements, getEvent, ref newtop, ref newleft);
            bool crashedInEnemy = GamePhysics.CrashEnemy(this, World.Enemies, getEvent, ref newtop, ref newleft) == null ? false : true;

            if (crashedInEnemy)
                return;

            if (newtop != 0)
                Top += newtop;
            if (newleft != 0)
                Left += newleft;
        }

        public override void Move(MoveType mtype)
        {
            wantNextMove = mtype;
        }

        public override void getEvent(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
            base.getEvent(gevent, args);

            if (gevent == GameEvent.crashInEnemy && !immortialize)
            {
                switch (mode)
                {
                    case MainGameObjectMode.NormalFire:
                        Mode = MainGameObjectMode.Normal;
                        break;
                    case MainGameObjectMode.Normal:
                        Mode = MainGameObjectMode.Small;
                        break;
                    case MainGameObjectMode.Small:
                        aiEventHandler(GameEvent.dead, args);
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
                    aiEventHandler(GameEvent.dead, args);
            }
            else if (gevent == GameEvent.gotFireFlower)
            {
                Mode = MainGameObjectMode.NormalFire;
            }
            else if (gevent == GameEvent.finishedLevel)
            {
                aiEventHandler(gevent, args);
            }
            else
                base.aiEventHandler(gevent, args);
        }
    }
}
