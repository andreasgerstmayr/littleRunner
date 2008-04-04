using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

namespace littleRunner
{
    class Tux : MainGameObject
    {
        private Image imgT, imgB, imgL, imgR;
        private Image curimg;
        private GameRunDirection direction;
        private int jumping;
        private bool firePressed;
        private GameMainObjectMode mode;
        private DateTime immortializeStart; 
        private bool immortialize;
        private int blink;

        public override void Draw(Graphics g)
        {
            if (!immortialize || blink % 8 == 0)
                g.DrawImage(curimg, Left, Top, Width, Height);

            if (immortialize)
                blink++;
        }

        private GameMainObjectMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;
                if (value == GameMainObjectMode.NormalFire)
                {
                    Height = imgL.Height;
                }
                else if (value == GameMainObjectMode.Normal)
                {
                    Height = imgL.Height;
                    blink = 0;
                }
                else if (value == GameMainObjectMode.Small)
                {
                    Top += imgL.Height - 44;
                    Height = 44;
                    blink = 0;
                }
            }
        }

        public Tux(int top, int left)
        {
            Top = top;
            Left = left;

            imgT = Image.FromFile(Properties.Resources.tux_left);
            imgB = Image.FromFile(Properties.Resources.tux_right);
            imgL = Image.FromFile(Properties.Resources.tux_left);
            imgR = Image.FromFile(Properties.Resources.tux_right);


            Width = imgR.Width;
            Height = imgR.Height;

            curimg = imgR;
            direction = GameRunDirection.Right;

            firePressed = false;
            mode = GameMainObjectMode.Normal;
            immortializeStart = DateTime.Now;
            immortialize = false;

            blink = 0;

            ImageAnimator.Animate(imgT, new EventHandler(FrameChanged));
            ImageAnimator.Animate(imgB, new EventHandler(FrameChanged));
            ImageAnimator.Animate(imgL, new EventHandler(FrameChanged));
            ImageAnimator.Animate(imgR, new EventHandler(FrameChanged));
        }

        public void FrameChanged(object o, EventArgs e)
        {
            World.Invalidate();
        }

        public override void Check(List<GameKey> pressedKeys)
        {
            int newtop = 0;
            int newleft = 0;


            // falling? (need for jumping-if)
            bool falling = GamePhysics.Falling(World.StickyElements, this);

            if (falling)
                newtop += 6;


            // immortialize end?
            if (immortialize && (DateTime.Now - immortializeStart).Seconds >= 1)
            {
                immortialize = false;
                blink = 0;
            }


            // key pressed?
            if (pressedKeys.Contains(GameKey.goLeft))
            {
                newleft -= 7;
                if (direction != GameRunDirection.Left)
                {
                    direction = GameRunDirection.Left;
                    curimg = imgL;
                }
            }
            if (pressedKeys.Contains(GameKey.goRight))
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
                jumping = 1;
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

            if (pressedKeys.Contains(GameKey.fire) && mode == GameMainObjectMode.NormalFire)
            {
                if (!firePressed)
                {
                    firePressed = true;
                    int startFireLeft = direction == GameRunDirection.Right ? Right + 5 : Left - 5;

                    Fire f = new Fire(direction, Top + 20, startFireLeft);
                    f.Init(World);
                    World.MovingElements.Add(f);
                }
            }
            else
                firePressed = false;


            // jumping?
            GamePhysics.Jumping(ref jumping, ref newtop, ref newleft);


            // check if direction is ok
            GamePhysics.CrashDetection(this, World.MovingElements, World.StickyElements, getEvent, ref newtop, ref newleft);
            bool crashedInEnemy = GamePhysics.CrashEnemy(this, World.Enemies, getEvent, ref newtop, ref newleft) == null ? false : true;

            if (crashedInEnemy)
                return;

            if (newtop != 0)
                Top += newtop;
            else if (jumping >= 100 && jumping <= 120)
                jumping = 121;

            if (newleft != 0)
                Left += newleft;
        }

        public override void Move(bool jump)
        {
            if (jump)
                jumping = 100;
        }

        public override void getEvent(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
            base.getEvent(gevent, args);
            if (gevent == GameEvent.crashInEnemy && !immortialize)
            {
                if (mode == GameMainObjectMode.NormalFire)
                    Mode = GameMainObjectMode.Normal;
                else if (mode == GameMainObjectMode.Normal)
                    Mode = GameMainObjectMode.Small;
                else if (mode == GameMainObjectMode.Small)
                    aiEventHandler(GameEvent.dead, args);
                immortialize = true;
                immortializeStart = DateTime.Now;
            }
            else if (gevent == GameEvent.gotGoodMushroom)
            {
                if (mode == GameMainObjectMode.Small)
                    Mode = GameMainObjectMode.Normal;
            }
            else if (gevent == GameEvent.gotPoisonMushroom)
            {
                if (mode == GameMainObjectMode.NormalFire || mode == GameMainObjectMode.Normal)
                    Mode = GameMainObjectMode.Small;
                else if (mode == GameMainObjectMode.Small)
                    aiEventHandler(GameEvent.dead, args);
            }
            else if (gevent == GameEvent.gotFireFlower)
            {
                Mode = GameMainObjectMode.NormalFire;
            }
            else if (gevent == GameEvent.finishedLevel)
            {
                aiEventHandler(gevent, args);
            }
            else
                aiEventHandler(gevent, args);
        }
    }
}
