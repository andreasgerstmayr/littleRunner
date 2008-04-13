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
        gotPoint,
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
        nextLevel
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
    enum GameRunDirection
    {
        Left,
        Right
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
        private SimpleDebug debug;
        private GameEventHandler forminteract;
        private Timer mainTimer;
        private MainGameObject mgo;
        public World world;
        private GameControlObjects gameControlObj;
        private List<Keys> curkeys;

        public void Init()
        {
            mainTimer = new Timer();
            mainTimer.Tick += new EventHandler(Check);
            mainTimer.Interval = 1;
            mainTimer.Enabled = true;
        }

        public void Draw(Graphics g)
        {
            world.Draw(g, true);
            gameControlObj.Draw(g);
            mgo.Draw(g);
        }

        public void PlayAgain()
        {
            mainTimer.Enabled = true;
        }
        public void Stop()
        {
            mainTimer.Enabled = false;
        }
        public void Quit()
        {
            Stop();
            gameControlObj.Points = 0;
        }

        public GameAI(Form form, GameEventHandler forminteract, World world, MainGameObject maingameobject, GameControlObjects gameControlObj)
        {
            this.form = form;
            this.forminteract = forminteract;
            this.mgo = maingameobject;
            this.world = world;
            this.gameControlObj = gameControlObj;
            this.curkeys = new List<Keys>();

            // add GameEventHandler
            foreach (GameObject go in world.AllElements)
            {
                go.aiEventHandler = getEvent;
            }
            this.mgo.aiEventHandler = getEvent;
        }
        public GameAI(Form form, SimpleDebug debug, GameEventHandler forminteract, World world, MainGameObject maingameobject, GameControlObjects gameControlObj)
            : this(form, forminteract, world, maingameobject, gameControlObj)
        {
            this.debug = debug;
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
            // scrolling?
            if (mgo.Left < 100)
                Scroll(15, true); // scroll left
            else if (world.Settings.GameWindowWidth - mgo.Right < 100)
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
            if (gevent == GameEvent.gotPoint)
            {
                gameControlObj.Points++;
            }
            else if (gevent == GameEvent.outOfRange || gevent == GameEvent.dead)
            {
                Stop();
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
    }
}
