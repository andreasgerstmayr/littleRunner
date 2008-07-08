using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace littleRunner.GameObjects.StickyElements
{
    enum LevelEndImg
    {
        House,
        Igloo
    }
    class LevelEnd : StickyImageElement
    {
        LevelEndImg image;
        string nextLevel;
        int startAt;
        [Category("Next level")]
        public string NextLevel
        {
            get { return nextLevel; }
            set { nextLevel = value; }
        }
        [Category("Next level")]
        public int StartAt
        {
            get { return startAt; }
            set { startAt = value; }
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
                    case LevelEndImg.House: CurImgFilename = Files.levelend_house; break;
                    case LevelEndImg.Igloo: CurImgFilename = Files.levelend_igloo; break;
                }
            }
        }

        override public void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
            base.onOver(geventhandler, who, direction);

            if (who == GameElement.MGO)
            {
                if (base.Name != null && base.Name != "" && World.Script != null)
                    World.Script.callFunction(base.Name, "finishedLevel", geventhandler);


                Dictionary<GameEventArg, object> args = new Dictionary<GameEventArg, object>();
                args[GameEventArg.nextLevel] = nextLevel;
                args[GameEventArg.nextLevelStartAt] = startAt;

                geventhandler(GameEvent.finishedLevel, args);
            }
        }

        public LevelEnd()
            : base()
        {
        }
        public LevelEnd(float top, float left, LevelEndImg img)
            : base(top, left)
        {
            Image = img;
        }


        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["NextLevel"] = nextLevel;
            ser["StartAt"] = startAt;
            ser["Image"] = image;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            nextLevel = (string)ser["NextLevel"];
            startAt = (int)ser["StartAt"];
            Image = (LevelEndImg)ser["Image"];
        }
    }
}
