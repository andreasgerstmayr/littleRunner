using System;
using System.Collections.Generic;
using System.Text;

namespace littleRunner.GameObjects
{
    public enum MoveType
    {
        jumpLeft,
        jumpTop,
        jumpRight,
        goLeft,
        goRight,
        goTop,
        goBottom,
        Nothing
    }


    public abstract class MainGameObject : GameObject
    {
        public MainGameObject()
            : base()
        {
        }

        public virtual GameDirection Direction
        {
            get { return GameDirection.None; }
            set { }
        }
        public abstract MainGameObjectMode Mode { get; set; }


        public virtual Dictionary<string, float> Check(List<GameKey> pressedKeys)
        {
            return new Dictionary<string, float>() { { "newtop", 0 }, { "newleft", 0 } };
        }
        public abstract void Move(MoveType mtype, float value, GameInstruction instruction);
        
        public virtual void getEvent(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
            AiEventHandler(gevent, args);
        }
    }
}
