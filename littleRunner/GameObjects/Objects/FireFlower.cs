using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace littleRunner.GameObjects.Objects
{
    class FireFlower : StickyImageElement
    {
        override public bool canStandOn
        {
            get { return false; }
        }
        override public void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
            base.onOver(geventhandler, who, direction);

            if (who == GameElement.MGO)
            {
                geventhandler(GameEvent.gotFireFlower, new Dictionary<GameEventArg, object>());
                World.StickyElements.Remove(this);
            }
        }


        public FireFlower()
            : base()
        {
        }
        public FireFlower(int top, int left)
            : base(top - Image.FromFile(Files.f[gFile.fire_flower]).Height, left,
                   Files.f[gFile.fire_flower])
        {
        }
    }
}
