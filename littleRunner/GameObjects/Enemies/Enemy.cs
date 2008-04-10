using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace littleRunner
{
    public abstract class Enemy : GameObject
    {
        public Enemy()
        {
        }

        [Browsable(false), Category("Enemy configuration")]
        public abstract bool fireCanDelete { get; }
        [Browsable(false), Category("Enemy configuration")]
        public abstract bool turtleCanRemove { get; }

        protected bool startAtViewpoint = true;
        [Category("Enemy configuration")]
        public virtual bool StartAtViewpoint
        {
            get { return startAtViewpoint; }
            set { startAtViewpoint = value; }
        }


        virtual public void Check() { }
        abstract public bool getCrashEvent(GameObject go, GameDirection cidirection); // true: survived, false: loose one livepoint


        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> ser = new Dictionary<string, object>(base.Serialize());
            ser["StartAtViewpoint"] = StartAtViewpoint;
            return ser;
        }
        public override void Deserialize(Dictionary<string, object> ser)
        {
            base.Deserialize(ser);
            StartAtViewpoint = (bool)ser["StartAtViewpoint"];
        }
    }
}
