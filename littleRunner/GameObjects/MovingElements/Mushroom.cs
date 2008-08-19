using System;
using System.Collections.Generic;
using System.Text;

using littleRunner.Drawing;
using littleRunner.Drawing.Helpers;


namespace littleRunner.GameObjects.MovingElements
{
    enum MushroomType
    {
        Good,
        Poison,
        Live
    }

    class Mushroom : MovingImageElement
    {
        MushroomType mtype;
        float distance;
        GameDirection direction;

        static dImage GetMushroomImage(MushroomType mtype)
        {
            switch (mtype)
            {
                case MushroomType.Good: return GetDraw.Image(Files.mushroom_good);
                case MushroomType.Poison: return GetDraw.Image(Files.mushroom_poison);
                case MushroomType.Live: return GetDraw.Image(Files.mushroom_live);
            }
            return null;
        }

        public Mushroom(MushroomType mtype, GameDirection direction, float top, float left)
            : base(Mushroom.GetMushroomImage(mtype),
            top - Mushroom.GetMushroomImage(mtype).Height,
            left)
        {
            this.mtype = mtype;
            distance = 0;
            this.direction = direction;
        }

        public override bool canStandOn
        {
            get { return false; }
        }

        public void GotMushroom()
        {
            switch (mtype)
            {
                case MushroomType.Good:
                    World.MGO.getEvent(GameEvent.gotGoodMushroom, new Dictionary<GameEventArg, object>());
                    break;
                case MushroomType.Poison:
                    World.MGO.getEvent(GameEvent.gotPoisonMushroom, new Dictionary<GameEventArg, object>());
                    break;
                case MushroomType.Live:
                    World.MGO.getEvent(GameEvent.gotLive, new Dictionary<GameEventArg, object>());
                    break;
            }

            World.MovingElements.Remove(this);
        }

        public override void Check(out Dictionary<string, float> newpos)
        {
            base.Check(out newpos);
            float newtop = newpos["top"];
            float newleft = newpos["left"];


            bool falling = false;
            if (distance < 120)
            {
                newtop -= Globals.MushroomMove.GO_Y * GameAI.FrameFactor;
                distance += Globals.MushroomMove.GO_Y * GameAI.FrameFactor;
            }
            else
            {
                // falling?
                falling = GamePhysics.Falling(World.StickyElements, World.MovingElements, World.Enemies, newtop, newleft, this);

                if (falling)
                    newtop += Globals.MushroomMove.GO_Y * GameAI.FrameFactor;
            }

            if (direction == GameDirection.Left)
                newleft -= Globals.MushroomMove.GO_X * GameAI.FrameFactor;
            else if (direction == GameDirection.Right)
                newleft += Globals.MushroomMove.GO_X * GameAI.FrameFactor;


            // check if direction is ok
            GamePhysics.CrashDetection(this, World.StickyElements, World.MovingElements, getEvent, ref newtop, ref newleft);


            // run in standing mgo?
            GameDirection _direction;
            if (GamePhysics.SingleCrashDetection(this, World.MGO, out _direction, ref newtop, ref newleft, true))
                GotMushroom();


            if (newtop != 0)
                Top += newtop;
            if (newleft != 0)
                Left += newleft;

            if (!falling && newleft == 0)
                direction = direction == GameDirection.Left ? GameDirection.Right : GameDirection.Left;


            // away?
            if (Top > World.Settings.LevelHeight)
                World.MovingElements.Remove(this);
        }

        public override void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
            base.onOver(geventhandler, who, direction);

            if (who == GameElement.MGO)
                GotMushroom();
        }

        public void getEvent(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
            AiEventHandler(gevent, args);
        }
    }
}
