using System;
using System.Collections.Generic;
using System.Text;

namespace littleRunner
{
    abstract class MainGameObject : GameObject
    {
        public GameEventHandler aiEventHandler;

        public virtual void Check(List<GameKey> pressedKeys)
        {
        }

        public abstract void Move(bool jump);

        public virtual void getEvent(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
        }
    }
}
