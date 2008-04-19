using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace littleRunner.GameObjects.MovingElements
{
    delegate void DeleteFire(Fire fire);

    class Fire : MovingElement
    {
        private GameDirection direction;
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
        public override bool canStandOn
        {
            get { return false; }
        }

        public Fire(GameDirection direction, int top, int left)
        {
            curimg = Image.FromFile(Files.fire);
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

        public override void Check(out Dictionary<string, int> newpos)
        {
            base.Check(out newpos);
            int newtop = newpos["top"];
            int newleft = newpos["left"];


            newleft += direction == GameDirection.Right ? 5 : -5;

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
                newleft += direction == GameDirection.Right ? 1 : -1;

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
            GamePhysics.CrashDetection(this, World.StickyElements, World.MovingElements, getEvent, ref newtop, ref newleft);
            Enemy crashedInEnemy = GamePhysics.CrashEnemy(this, World.Enemies, getEvent, ref newtop, ref newleft);
            if (GamePhysics.SimpleCrashDetections(this, World.StickyElements, World.MovingElements, true, newtop, newleft))
            {
                World.MovingElements.Remove(this);
                return;
            }

            if (crashedInEnemy != null && crashedInEnemy.fireCanDelete)
            {
                crashedInEnemy.Remove();
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
            AiEventHandler(gevent, args);
        }
    }
}
