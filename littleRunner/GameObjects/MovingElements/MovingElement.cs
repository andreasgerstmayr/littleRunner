using System;
using System.Collections.Generic;
using System.Text;

namespace littleRunner
{
    public abstract class MovingElement : StickyElement
    {
        public override bool canStandOn
        {
            get { return false; }
        }

        virtual public void Check()
        {
        }
        virtual public void Check(out int[] newpos)
        {
            newpos = new int[2] { 0, 0 };

            if (base.Name != null && base.Name != "" && World.Script != null)
                World.Script.callFunction(base.Name, "Check", newpos);
        }
    }
}
