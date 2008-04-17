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
                switch (World.MGO.Mode)
                {
                    case MainGameObjectMode.Small:
                    case MainGameObjectMode.Normal:
                        geventhandler(GameEvent.gotFireFlower, new Dictionary<GameEventArg, object>());
                        break;
                    case MainGameObjectMode.NormalFire:
                        Dictionary<GameEventArg, object> pointsArgs = new Dictionary<GameEventArg,object>();
                        pointsArgs[GameEventArg.points] = 100;
                        geventhandler(GameEvent.gotPoints, pointsArgs);
                        break;
                }

                World.StickyElements.Remove(this);
            }
        }


        public FireFlower()
            : base()
        {
        }
        public FireFlower(int top, int left)
            : base(top - Image.FromFile(Files.fire_flower).Height, left,
                   Files.fire_flower)
        {
        }
    }
}
