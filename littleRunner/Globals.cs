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
        GDI
    }
    

    sealed class Globals
    {
        public static VideoRenderMode VideoRenderMode;
        public static float Approximation = 0.5F;

        public struct MGOMove
        {
            public const int GO_X = 300;
            public const int Jump = 620;
            public const int Falling = 400;
        }
        public struct FireMove
        {
            public const int X = 300;
            public const int Y = 200;
        }
        public struct ImmortializeStarMove
        {
            public const int X = 200;
            public const int Y = 500;
        }
        public struct JumpingStarMove
        {
            public const int Y = 70;
        }
        public struct MushroomMove
        {
            public const int GO_X = 180;
            public const int GO_Y = 250;
        }

        public const int ObjFalling = 350;
        public const int Gumba_X = 50;
        public struct Turtle
        {
            public const int Normal = 80;
            public const int Fast = 1200;
        }

        public struct Scroll
        {
            public const int Top = 20;
            public const int Bottom = 70;

            public const int X = 170;

            public struct Change
            {
                public const int Y = 10;
                public const int X = 7;
            }
        }


        public static void NullDelegate()
        {
        }
    }
}
