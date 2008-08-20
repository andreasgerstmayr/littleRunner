using System;
using System.Collections.Generic;
using System.Text;

using littleRunner.Drawing;
using littleRunner.Drawing.Helpers;
using littleRunner.Gamedata.Worlddata;


namespace littleRunner.GameObjects.MovingElements
{
    class JumpingStar : MovingImageElement
    {
        float distance;

        public JumpingStar(float top, float left)
            : base(GetDraw.Image(Files.star),
            top - GetDraw.Image(Files.star).Height - 10,
            left)
        {
            distance = 0;
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
        public override void Check(out Dictionary<string, float> newpos)
        {
            base.Check(out newpos);
            float newtop = newpos["top"];
            float newleft = newpos["left"];


            int speedStep = 1;

            if (distance < 50)
                speedStep = 1;
            else if (distance < 70)
                speedStep = 2;
            else if (distance < 90)
                speedStep = 3;
            else if (distance <= 110)
                speedStep = 4;
            else if (distance <= 130)
                speedStep = 5;

            float move = Globals.JumpingStarMove.Y * speedStep * GameAI.FrameFactor;
            newtop -= move;
            distance += move;
            Top += newtop;


            // gone
            if (distance > 130)
                World.MovingElements.Remove(this);
        }
    }
}
