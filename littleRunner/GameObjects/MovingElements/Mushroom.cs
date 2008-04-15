using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


namespace littleRunner.GameObjects.MovingElements
{
    enum MushroomType
    {
        Good,
        Poison
    }

    class Mushroom : MovingImageElement
    {
        MushroomType mtype;
        int checks;
        GameDirection direction;

        public Mushroom(MushroomType mtype, int top, int left)
            : base(mtype == MushroomType.Good ? Image.FromFile(Files.mushroom_green) : Image.FromFile(Files.mushroom_poison), top - Image.FromFile(Files.mushroom_green).Height, left)
        {
            this.mtype = mtype;
            checks = 0;
            direction = GameDirection.Right;
        }

        public override bool canStandOn
        {
            get { return false; }
        }
        public override void Check(out Dictionary<string, int> newpos)
        {
            base.Check(out newpos);
            int newtop = newpos["top"];
            int newleft = newpos["left"];


            bool falling = false;
            if (checks < 20)
            {
                newtop -= 4;
                checks++;
            }
            else
            {
                // falling?
                falling = GamePhysics.Falling(World.StickyElements, World.MovingElements, this);

                if (falling)
                    newtop += 4;
            }

            if (direction == GameDirection.Left)
                newleft -= 3;
            else if (direction == GameDirection.Right)
                newleft += 3;


            // check if direction is ok
            GamePhysics.CrashDetection(this, World.StickyElements, World.MovingElements, getEvent, ref newtop, ref newleft);
            bool crashedInEnemy = GamePhysics.CrashEnemy(this, World.Enemies, getEvent, ref newtop, ref newleft) == null ? false : true;


            // run in standing mgo?
            GameDirection _direction;
            if (GamePhysics.SingleCrashDetection(this, World.MGO, out _direction, ref newtop, ref newleft, true))
            {
                switch (mtype)
                {
                    case MushroomType.Good:
                        World.MGO.getEvent(GameEvent.gotGoodMushroom, new Dictionary<GameEventArg, object>());
                        break;
                    case MushroomType.Poison:
                        World.MGO.getEvent(GameEvent.gotPoisonMushroom, new Dictionary<GameEventArg, object>());
                        break;
                }
                World.MovingElements.Remove(this);
            }


            if (newtop != 0)
                Top += newtop;
            if (newleft != 0)
                Left += newleft;

            if (!falling && (newleft == 0 || crashedInEnemy))
                direction = direction == GameDirection.Left ? GameDirection.Right : GameDirection.Left;


            // away?
            if (Top > World.Settings.LevelHeight)
                World.MovingElements.Remove(this);
        }

        public override void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
            base.onOver(geventhandler, who, direction);

            if (who == GameElement.MGO)
            {
                switch (mtype)
                {
                    case MushroomType.Good:
                        World.MGO.getEvent(GameEvent.gotGoodMushroom, new Dictionary<GameEventArg, object>());
                        break;
                    case MushroomType.Poison:
                        World.MGO.getEvent(GameEvent.gotPoisonMushroom, new Dictionary<GameEventArg, object>());
                        break;
                }
                World.MovingElements.Remove(this);
            }
        }

        public void getEvent(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
            AiEventHandler(gevent, args);
        }
    }
}
