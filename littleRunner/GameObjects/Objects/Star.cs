using System;
using System.Collections.Generic;
using System.Text;

namespace littleRunner.GameObjects.Objects
{
    enum StarStyle
    {
        Yellow
    }
    class Star : StickyImageElement
    {
        StarStyle style;

        override public bool canStandOn
        {
            get { return false; }
        }
        public StarStyle Style
        {
            get { return style; }
            set
            {
                style = value;
                switch (style)
                {
                    case StarStyle.Yellow:
                        CurImgFilename = Files.f[gFile.star];
                        break;
                }
            }
        }
        override public void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
            base.onOver(geventhandler, who, direction);

            geventhandler(GameEvent.gotPoint, new Dictionary<GameEventArg, object>());
            World.StickyElements.Remove(this);
        }


        public Star()
            : base()
        {
        }
        public Star(int top, int left)
            : base(top, left)
        {
            Style = StarStyle.Yellow;
        }


        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["StarStyle"] = style;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            Style = (StarStyle)ser["StarStyle"];
        }
    }
}
