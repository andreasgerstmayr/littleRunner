using System;
using System.Collections.Generic;
using System.Text;

namespace littleRunner.GameObjects.Objects
{
    enum LevelEndImg
    {
        House
    }
    class LevelEnd : StickyImageElement
    {
        LevelEndImg image;
        string nextWorld;
        public string NextWorld
        {
            get { return nextWorld; }
            set { nextWorld = value; }
        }
        public override bool canStandOn
        {
            get { return false; }
        }
        public LevelEndImg Image
        {
            get { return image; }
            set
            {
                image = value;
                switch (image)
                {
                    case LevelEndImg.House:
                        CurImgFilename = Files.f[gFile.levelend_house];
                        break;
                }
            }
        }

        override public void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
            base.onOver(geventhandler, who, direction);

            if (who == GameElement.MGO)
            {
                Dictionary<GameEventArg, object> args = new Dictionary<GameEventArg, object>();
                args[GameEventArg.nextLevel] = nextWorld;

                geventhandler(GameEvent.finishedLevel, args);
            }
        }

        public LevelEnd()
            : base()
        {
        }
        public LevelEnd(int top, int left, LevelEndImg img)
            : base(top, left)
        {
            Image = image;
        }


        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["NextWorld"] = nextWorld;
            ser["Image"] = image;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            nextWorld = (string)ser["NextWorld"];
            Image = (LevelEndImg)ser["Image"];
        }
    }
}
