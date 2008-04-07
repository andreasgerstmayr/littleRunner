using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace littleRunner.GameObjects.Objects
{
    enum LevelEndImg
    {
        House
    }
    class LevelEnd : StickyImageElement
    {
        LevelEndImg image;
        string nextLevel;
        [Category("Next level")]
        public string NextLevel
        {
            get { return nextLevel; }
            set { nextLevel = value; }
        }
        public override bool canStandOn
        {
            get { return false; }
        }
        [Category("LevelEnd image")]
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
                args[GameEventArg.nextLevel] = nextLevel;

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
            Image = img;
        }


        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["NextLevel"] = nextLevel;
            ser["Image"] = image;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            nextLevel = (string)ser["NextLevel"];
            Image = (LevelEndImg)ser["Image"];
        }
    }
}
