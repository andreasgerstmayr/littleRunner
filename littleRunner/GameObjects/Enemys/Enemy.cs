using System;
using System.Collections.Generic;
using System.Text;

namespace littleRunner
{
    abstract class Enemy : GameObject
    {
        public Enemy()
        {
        }

        abstract public bool canFire { get; }
        virtual public void Check() { }
        abstract public bool getCrashEvent(GameObject go, GameDirection cidirection);

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
