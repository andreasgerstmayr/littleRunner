using System;
using System.Collections.Generic;
using System.Text;

namespace littleRunner
{
    abstract class MovingElement : StickyElement
    {
        public override bool canStandOn
        {
            get { return false; }
        }

        abstract public void Check();
    }
}
