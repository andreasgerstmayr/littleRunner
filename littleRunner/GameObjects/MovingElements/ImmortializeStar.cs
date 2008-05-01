using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace littleRunner.GameObjects.MovingElements
{
    class ImmortializeStar : MovingImageElement
    {
        private GameDirection direction;
        private GameDirection flyDirection;
        private int flyTopCount;
        private int runs;

        public override bool canStandOn
        {
            get { return false; }
        }

        public override void Draw(Graphics g)
        {
            if (DateTime.Now.Millisecond % 4 == 0)
                base.Draw(g);
        }

        public ImmortializeStar(GameDirection direction, int top, int left)
            : base(Image.FromFile(Files.star),
            top - Image.FromFile(Files.star).Height,
            left)
        {
            this.direction = direction;
            flyDirection = GameDirection.Top;
            flyTopCount = 0;
            runs = 0;
        }

        public override void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
            base.onOver(geventhandler, who, direction);

            if (who == GameElement.MGO)
            {
                geventhandler(GameEvent.gotImmortialize, new Dictionary<GameEventArg, object>());
                World.MovingElements.Remove(this);
            }
        }


        public override void Check(out Dictionary<string, int> newpos)
        {
            base.Check(out newpos);
            int newtop = newpos["top"];
            int newleft = newpos["left"];

            switch (direction)
            {
                case GameDirection.Right: newleft += 5; break;
                case GameDirection.Left: newleft -= 5; break;
            }
            switch (flyDirection)
            {
                case GameDirection.Top: newtop -= 5; break;
                case GameDirection.Bottom: newtop += 5; break;
            }

            if (flyDirection == GameDirection.Top)
                flyTopCount++;

            if (flyTopCount > 20)
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

            if (newtop != 0)
                Top += newtop;
            else
                flyDirection = GameDirection.Top;


            if (newleft != 0)
                Left += newleft;
            else
            {
                switch (direction)
                {
                    case GameDirection.Right: direction = GameDirection.Left; break;
                    case GameDirection.Left: direction = GameDirection.Right; break;
                }
            }

            runs++;


            // away?
            if (runs >= 150 || Top > World.Settings.LevelHeight)
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
