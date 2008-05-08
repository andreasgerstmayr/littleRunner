using System;
using System.Collections.Generic;
using System.Text;

using littleRunner.Drawing;


namespace littleRunner.GameObjects.StickyElements
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
                        pointsArgs[GameEventArg.points] = 10;
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
            : base(top - Draw.Image.Open(Files.fire_flower).Height, left,
                   Files.fire_flower)
        {
        }
    }
}
