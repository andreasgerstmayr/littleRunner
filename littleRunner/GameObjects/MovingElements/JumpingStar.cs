using System;
using System.Collections.Generic;
using System.Text;

using littleRunner.Drawing;
using littleRunner.Gamedata.Worlddata;


namespace littleRunner.GameObjects.MovingElements
{
    class JumpingStar : MovingImageElement
    {
        int count;

        public JumpingStar(int top, int left)
            : base(Draw.Image.Open(Files.star),
            top - Draw.Image.Open(Files.star).Height - 10,
            left)
        {
            count = 0;
        }

        public override void Init(World world, GameEventHandler aiEventHandler)
        {
            base.Init(world, aiEventHandler);

            Dictionary<GameEventArg, object> args = new Dictionary<GameEventArg, object>();
            args[GameEventArg.points] = 1;

            World.MGO.getEvent(GameEvent.gotPoints, args);
        }


        public override bool canStandOn
        {
            get { return false; }
        }
        public override void Check(out Dictionary<string, int> newpos)
        {
            base.Check(out newpos);
            int newtop = newpos["top"];
            int newleft = newpos["left"];

            newtop -= (int)Math.Pow(1.3, count+5);
            Top += newtop;

            count++;


            // gone
            if (count > 10)
                World.MovingElements.Remove(this);
        }
    }
}
