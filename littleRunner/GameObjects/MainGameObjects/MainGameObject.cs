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

        public virtual GameDirection Direction
        {
            get { return GameDirection.None; }
            set { }
        }

        public abstract void Move(MoveType mtype, int value, GameInstruction instruction);
        public abstract MainGameObjectMode Mode { get; set; }

        public virtual void getEvent(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
            AiEventHandler(gevent, args);
        }
    }
}
