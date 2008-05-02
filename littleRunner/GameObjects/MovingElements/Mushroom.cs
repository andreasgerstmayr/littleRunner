using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


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
        int checks;
        GameDirection direction;

        static Image GetMushroomImage(MushroomType mtype)
        {
            switch (mtype)
            {
                case MushroomType.Good: return Image.FromFile(Files.mushroom_good);
                case MushroomType.Poison: return Image.FromFile(Files.mushroom_poison);
                case MushroomType.Live: return Image.FromFile(Files.mushroom_live);
            }
            return new Bitmap(0, 0);
        }

        public Mushroom(MushroomType mtype, GameDirection direction, int top, int left)
            : base(Mushroom.GetMushroomImage(mtype),
            top - Mushroom.GetMushroomImage(mtype).Height,
            left)
        {
            this.mtype = mtype;
            checks = 0;
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
                falling = GamePhysics.Falling(World.StickyElements, World.MovingElements, World.Enemies, newtop, newleft, this);

                if (falling)
                    newtop += 4;
            }

            if (direction == GameDirection.Left)
                newleft -= 3;
            else if (direction == GameDirection.Right)
                newleft += 3;


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
