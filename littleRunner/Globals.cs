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
        fire,
        runFast
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


    public enum VideoRenderMode
    {
        GDI,
        Irrlicht
    }

    sealed class Globals
    {
        public static VideoRenderMode VideoRenderMode;

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
