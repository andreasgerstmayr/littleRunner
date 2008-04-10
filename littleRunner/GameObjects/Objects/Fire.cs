using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace littleRunner
{
    delegate void DeleteFire(Fire fire);

    class Fire : MovingElement
    {
        private GameRunDirection direction;
        private bool onBottom;
        private float jumps;
        private int curJump;
        private int runs;
        private bool checkIfBottom;
        private Image curimg;

        public override void Draw(Graphics g)
        {
            g.DrawImage(curimg, Left, Top, Width, Height);
        }

        public Fire(GameRunDirection direction, int top, int left)
        {
            curimg = Image.FromFile(Files.f[gFile.fire]);
            this.direction = direction;
            onBottom = false;
            checkIfBottom = false;
            jumps = 0.0F;
            curJump = 0;
            runs = 0;

            Top = top;
            Left = left;

            Width = curimg.Width;
            Height = curimg.Height;
        }

        public override void Check()
        {
            base.Check();

            int newtop = 0;
            int newleft = 0;

            newleft += direction == GameRunDirection.Right ? 5 : -5;

            if (!onBottom)
            {
                if (runs >= 50)
                {
                    newtop += 3;
                    checkIfBottom = true;
                }
                else
                    runs++;
            }
            else
            {
                newleft += direction == GameRunDirection.Right ? 1 : -1;

                if (curJump <= 5)
                    newtop += 5;
                else
                    newtop -= 5;
                curJump++;

                if (curJump >= 10)
                    curJump = 0;
                jumps += 0.1F;

                if (jumps >= 4.5F)
                    World.MovingElements.Remove(this);
            }


            // check if direction is ok
            GamePhysics.CrashDetection(this, World.MovingElements, World.StickyElements, getEvent, ref newtop, ref newleft);
            Enemy crashedInEnemy = GamePhysics.CrashEnemy(this, World.Enemies, getEvent, ref newtop, ref newleft);
            if (GamePhysics.SimpleCrashDetections(this, World.StickyElements, true, newtop, newleft))
            {
                World.MovingElements.Remove(this);
                return;
            }

            if (crashedInEnemy != null && crashedInEnemy.fireCanDelete)
            {
                World.Enemies.Remove(crashedInEnemy);
                World.MovingElements.Remove(this);
                return;
            }

            if (newtop != 0)
                Top += newtop;
            else if (checkIfBottom)
                onBottom = true;

            if (newleft != 0)
                Left += newleft;
            else
                World.MovingElements.Remove(this);

            // away?
            if (Top > World.Settings.LevelHeight)
            {
                World.MovingElements.Remove(this);
            }
        }

        public void getEvent(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
            base.aiEventHandler(gevent, args);
        }
    }
}
