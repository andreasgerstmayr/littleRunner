using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;


namespace littleRunner
{
    abstract class StickyElement : GameObject
    {
        [Browsable(false), Category("Object")]
        abstract public bool canStandOn { get; }

        public StickyElement()
        {
        }

        public virtual void onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)
        {
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
