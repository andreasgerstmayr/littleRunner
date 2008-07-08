using System;
using System.Collections.Generic;
using System.Text;

using littleRunner.Drawing;
using littleRunner.Drawing.Helpers;


namespace littleRunner.GameObjects.MovingElements
{
    class Fire : MovingImageElement
    {
        private GameDirection direction;
        private GameDirection flyDirection;
        private int flyTopCount;
        private int runs;

        public override bool canStandOn
        {
            get { return false; }
        }

        public Fire(GameDirection direction, float top, float left)
            : base(GetDraw.Image(Files.fire), top, left)
        {
            this.direction = direction;
            flyDirection = GameDirection.Bottom;
            flyTopCount = 0;
            runs = 0;
        }

        public override void Check(out Dictionary<string, float> newpos)
        {
            base.Check(out newpos);
            float newtop = newpos["top"];
            float newleft = newpos["left"];

            switch (direction)
            {
                case GameDirection.Right: newleft += Globals.FireMove.X * GameAI.FrameFactor; break;
                case GameDirection.Left: newleft -= Globals.FireMove.X * GameAI.FrameFactor; break;
            }

            if (runs >= 50)
            {
                switch(flyDirection)
                {
                    case GameDirection.Bottom: newtop += Globals.FireMove.Y * GameAI.FrameFactor; break;
                    case GameDirection.Top: newtop -= Globals.FireMove.Y * GameAI.FrameFactor; break;
                }
            }
            runs++;


            if (flyDirection == GameDirection.Top)
                flyTopCount++;

            if (flyTopCount > 10)
            {
                flyDirection = GameDirection.Bottom;
                flyTopCount = 0;
            }


            // check if direction is ok
            GamePhysics.CrashDetection(this, World.StickyElements, World.MovingElements, getEvent, ref newtop, ref newleft);
            Enemy crashedInEnemy = GamePhysics.CrashEnemy(this, World.Enemies, getEvent, ref newtop, ref newleft);
            if (GamePhysics.SimpleCrashDetections(this, World.StickyElements, World.MovingElements, true, newtop, newleft))
            {
                World.MovingElements.Remove(this);
                return;
            }

            if (crashedInEnemy != null) // crashed in some enemy
            {
                if (crashedInEnemy.fireCanDelete) // fire can delete it?
                {
                    crashedInEnemy.Remove();
                    World.MovingElements.Remove(this);
                }
                else // crashed in enemy and fire can't delete it --> remove fire
                    World.MovingElements.Remove(this);
                return;
            }


            if (newtop != 0)
                Top += newtop;
            else if (runs >= 52) // 50 -> start moving, +1 -> nothing moved, so use +2!
                flyDirection = GameDirection.Top;

            if (newleft != 0)
                Left += newleft;
            else
                World.MovingElements.Remove(this);


            // away?
            if (runs >= 200 || Top > World.Settings.LevelHeight)
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
