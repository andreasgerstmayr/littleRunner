using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using littleRunner.GameObjects;


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
        gotFireFlower,
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


    public delegate void GameEventHandler(GameEvent gevent, Dictionary<GameEventArg, object> args);
    public delegate void GameCrashHandler(GameEvent gevent, GameDirection cidirection);

    public class GameAI
    {
        private Form form;
        private GameEventHandler forminteract;
        private Timer mainTimer;
        private MainGameObject mgo;
        public World world;
        private GameControlObjects gameControlObj;
        private List<Keys> curkeys;


        public void Draw(Graphics g)
        {
            world.Draw(g, true);
            gameControlObj.Draw(g);
            mgo.Draw(g);
        }
        public bool IsRunning
        {
            get { return mainTimer.Enabled; }
        }

        public void Pause(bool start)
        {
            if (start)
                mainTimer.Enabled = true;
            else
                mainTimer.Enabled = false;
        }
        public void Quit()
        {
            Pause(false);
            gameControlObj.Points = 0;
        }

        public GameAI(Form form, GameEventHandler forminteract)
        {
            this.form = form;
            this.forminteract = forminteract;
            this.curkeys = new List<Keys>();
        }
        public void Init(World world, MainGameObject maingameobject, GameControlObjects gameControlObj)
        {
            mainTimer = new Timer();
            mainTimer.Tick += new EventHandler(Check);
            mainTimer.Interval = 1;
            mainTimer.Enabled = true;

            this.gameControlObj = gameControlObj;
            this.mgo = maingameobject;
            this.world = world;
        }


        public void Scroll(int value, bool moveMGO)
        {
            foreach (GameObject go in world.AllElements)
            {
                go.Left += value;
            }
            if (moveMGO)
                mgo.Left += value;
        }

        public void Check(object sender, EventArgs e)
        {
            if (world.Script != null)
                world.Script.callFunction("AI", "Check");


            // scrolling?
            if (mgo.Left < 140)
                Scroll(15, true); // scroll left
            else if (world.Settings.GameWindowWidth - mgo.Right < 140)
                Scroll(-15, true); // scroll right


            List<GameKey> pressedKeys = new List<GameKey>();

            // key pressed?
            if (curkeys.Contains(Keys.A) || curkeys.Contains(Keys.Left))
                pressedKeys.Add(GameKey.goLeft);
            if (curkeys.Contains(Keys.D) || curkeys.Contains(Keys.Right))
                pressedKeys.Add(GameKey.goRight);
            if (curkeys.Contains(Keys.Space))
                pressedKeys.Add(GameKey.fire);
            if (curkeys.Contains(Keys.Q))
                pressedKeys.Add(GameKey.jumpLeft);
            if (curkeys.Contains(Keys.W) || curkeys.Contains(Keys.Up))
                pressedKeys.Add(GameKey.jumpTop);
            if (curkeys.Contains(Keys.E))
                pressedKeys.Add(GameKey.jumpRight);


            mgo.Check(pressedKeys);

            // check of all enemies
            for (int i = 0; i < world.Enemies.Count; i++)
            {
                if (!world.Enemies[i].StartAtViewpoint || world.Enemies[i].Left < world.Settings.GameWindowWidth)
                {
                    Dictionary<string, int> newpos;
                    world.Enemies[i].Check(out newpos);
                }
            }
            // check of all moving elements
            for (int i = 0; i < world.MovingElements.Count; i++)
            {
                Dictionary<string, int> newpos;
                world.MovingElements[i].Check(out newpos);
            }

            // mgo out of range?
            if (mgo.Top > form.Height)
            {
                mainTimer.Enabled = false;
                forminteract(GameEvent.outOfRange, new Dictionary<GameEventArg, object>());
            }

            // Repaint
            world.Invalidate();
        }

        public void Interact(Keys key, bool pressed)
        {
            if (pressed && !curkeys.Contains(key))
                curkeys.Add(key);
            else if (!pressed)
                curkeys.Remove(key);
        }

        public void getEvent(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
            if (gevent == GameEvent.gotPoints)
            {
                gameControlObj.Points += (int)args[GameEventArg.points];

                if (gameControlObj.Points > 100)
                {
                    gameControlObj.Lives++;
                    gameControlObj.Points -= 100;
                }
            }
            else if (gevent == GameEvent.outOfRange || gevent == GameEvent.dead)
            {
                Pause(false);
                forminteract(gevent, args);
            }
            else if (gevent == GameEvent.finishedLevel)
                forminteract(gevent, args);
        }


        // static methods
        static public GameElement WhoIsIt(GameObject go)
        {
            Type go_type = go.GetType();

            if (typeof(MainGameObject).IsAssignableFrom(go_type))
                return GameElement.MGO;
            else if (typeof(Enemy).IsAssignableFrom(go_type))
                return GameElement.Enemy;
            else if (typeof(MovingElement).IsAssignableFrom(go_type))
                return GameElement.MovingElement;
            else
                return GameElement.Unknown;
        }


        static public void NullAiEventHandlerMethod(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
        }
    }
}
