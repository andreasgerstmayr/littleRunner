using System;
using System.Collections.Generic;
using System.Text;

namespace littleRunner.GameObjects
{
    public enum MoveType
    {
        Jump,
        goLeft,
        goRight,
        goTop,
        goBottom,
        Nothing
    }
    public abstract class MainGameObject : GameObject
    {
        public virtual void Check(List<GameKey> pressedKeys)
        {
        }

        public abstract void Move(MoveType mtype);
        public abstract MainGameObjectMode currentMode { get; set; }

        public virtual void getEvent(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
            base.aiEventHandler(gevent, args);
        }
    }
}
