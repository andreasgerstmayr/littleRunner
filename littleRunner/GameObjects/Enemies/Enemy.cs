using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace littleRunner
{
    abstract class Enemy : GameObject
    {
        public Enemy()
        {
        }

        [Browsable(false), Category("Enemy configuration")]
        public abstract bool fireCanDelete { get; }
        [Browsable(false), Category("Enemy configuration")]
        public abstract bool turtleCanRemove { get; }

        virtual public void Check() { }
        abstract public bool getCrashEvent(GameObject go, GameDirection cidirection); // true: survived, false: loose one livepoint
    }
}
