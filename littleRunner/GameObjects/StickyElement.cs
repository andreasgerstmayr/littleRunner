using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace littleRunner
{
    public abstract class StickyElement : GameObject
    {
        [Browsable(false), Category("Object")]
        abstract public bool canStandOn { get; }

        public StickyElement()
        {
        }

        public virtual void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
            if (base.Name != null && base.Name != "" && World.Script != null)
            {
                World.Script.callFunction(base.Name, "onOver", this, geventhandler,
                    who, direction);
            }
        }

        public override Dictionary<string, object> Serialize()
        {
            return base.Serialize();
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
        }
    }
}
