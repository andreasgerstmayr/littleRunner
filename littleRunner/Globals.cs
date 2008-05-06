using System;


namespace littleRunner
{
    public enum GameEvent
    {
        crashInEnemy,
        outOfRange,
        dead,
        gotPoints,
        gotGoodMushroom,
        gotPoisonMushroom,
        gotLive,
        gotFireFlower,
        gotImmortialize,
        finishedLevel
    }
    public enum GameDirection
    {
        Left,
        Right,
        Top,
        Bottom,
        None
    }
    public enum GameElement
    {
        MGO,
        Enemy,
        MovingElement,
        Unknown
    }
    public enum GameEventArg
    {
        nextLevel,
        nextLevelStartAt,
        points
    }
    public enum GameKey
    {
        goLeft,
        goRight,
        jumpLeft,
        jumpTop,
        jumpRight,
        fire
    }
    public enum MainGameObjectMode
    {
        NormalFire,
        Normal,
        Small
    }
    public enum PlayMode
    {
        Game,
        GameInEditor,
        Editor
    }


    sealed class Globals
    {
        public const int SCROLL_TOP = 20;
        public const int SCROLL_BOTTOM = 70;

        public const int SCROLL_X = 140;
        public const int SCROLL_CHANGE_Y = 10;
        public const int SCROLL_CHANGE_X = 7;


        public static void NullDelegate()
        {
        }
    }
}
