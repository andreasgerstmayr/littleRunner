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

        public abstract bool fireCanDelete { get; }

        virtual public void Check() { }
        abstract public bool getCrashEvent(GameObject go, GameDirection cidirection); // true: survived, false: loose one livepoint
    }
}
