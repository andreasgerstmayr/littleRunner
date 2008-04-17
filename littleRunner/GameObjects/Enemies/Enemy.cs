using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace littleRunner.GameObjects
{
    public abstract class Enemy : GameObject
    {
        public Enemy()
        {
        }

        public override bool canStandOn
        {
            get { return true; }
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

        virtual public void Check(out Dictionary<string, int> newpos)
        {
            newpos = new Dictionary<string, int>();
            newpos["top"] = 0;
            newpos["left"] = 0;

            if (base.Name != null && base.Name != "" && World.Script != null)
                World.Script.callFunction(base.Name, "Check", newpos);
        }
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
