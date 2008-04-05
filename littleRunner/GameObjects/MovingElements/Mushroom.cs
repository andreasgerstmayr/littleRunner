using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace littleRunner
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
        GameRunDirection direction;

        public Mushroom(MushroomType mtype, int top, int left)
            : base(mtype == MushroomType.Good ? Image.FromFile(Files.f[gFile.mushroom_green]) : Image.FromFile(Files.f[gFile.mushroom_poison]), top - Image.FromFile(Files.f[gFile.mushroom_green]).Height, left)
        {
            this.mtype = mtype;
            checks = 0;
            direction = GameRunDirection.Right;
        }


        public override void Check()
        {
            int newtop = 0;
            int newleft = 0;

            bool falling = false;
            if (checks < 20)
            {
                newtop -= 4;
                checks++;
            }
            else
            {
                // falling?
                falling = GamePhysics.Falling(World.StickyElements, this);

                if (falling)
                    newtop += 4;
            }

            if (direction == GameRunDirection.Left)
                newleft -= 3;
            else if (direction == GameRunDirection.Right)
                newleft += 3;


            // check if direction is ok
            GamePhysics.CrashDetection(this, World.MovingElements, World.StickyElements, getEvent, ref newtop, ref newleft);
            bool crashedInEnemy = GamePhysics.CrashEnemy(this, World.Enemies, getEvent, ref newtop, ref newleft) == null ? false : true;


            // run in standing mgo?
            GameDirection _direction;
            if (GamePhysics.SingleCrashDetection(this, World.MGO, out _direction, ref newtop, ref newleft, true))
            {
                if (mtype == MushroomType.Good)
                    World.MGO.getEvent(GameEvent.gotGoodMushroom, new Dictionary<GameEventArg, object>());
                else if (mtype == MushroomType.Poison)
                    World.MGO.getEvent(GameEvent.gotPoisonMushroom, new Dictionary<GameEventArg, object>());
                World.MovingElements.Remove(this);
            }


            if (newtop != 0)
                Top += newtop;
            if (newleft != 0)
                Left += newleft;

            if (!falling && (newleft == 0 || crashedInEnemy))
                direction = direction == GameRunDirection.Left ? GameRunDirection.Right : GameRunDirection.Left;


            // away?
            if (Top > World.Settings.LevelHeight)
                World.MovingElements.Remove(this);
        }

        public override void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
            base.onOver(geventhandler, who, direction);

            if (who == GameElement.MGO)
            {
                if (mtype == MushroomType.Good)
                    World.MGO.getEvent(GameEvent.gotGoodMushroom, new Dictionary<GameEventArg, object>());
                else if (mtype == MushroomType.Poison)
                    World.MGO.getEvent(GameEvent.gotPoisonMushroom, new Dictionary<GameEventArg, object>());
                World.MovingElements.Remove(this);
            }
        }

        public void getEvent(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
        }
    }
}
