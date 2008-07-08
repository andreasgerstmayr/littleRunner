using System;
using System.Collections.Generic;
using System.Text;


namespace littleRunner.GameObjects
{
    public abstract class MovingElement : StickyElement
    {

        virtual public void Check(out Dictionary<string, float> newpos)
        {
            newpos = new Dictionary<string, float>();
            newpos["top"] = 0;
            newpos["left"] = 0;

            if (base.Name != null && base.Name != "" && World.Script != null)
                World.Script.callFunction(base.Name, "Check", newpos);
        }

    }
}
