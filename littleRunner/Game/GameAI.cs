using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace littleRunner
{
    enum GameEvent
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
    enum GameDirection
    {
        Left,
        Right,
        Top,
        Bottom,
        None
    }
    enum GameElement
    {
        MGO,
        Enemy,
        MovingElement,
        Unknown
    }
    enum GameEventArg
    {
        nextLevel
    }
    enum GameKey
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
    enum GameMainObjectMode
    {
        NormalFire,
        Normal,
        Small
    }


    delegate void GameEventHandler(GameEvent gevent, Dictionary<GameEventArg, object> args);
    delegate void GameCrashHandler(GameEvent gevent, GameDirection cidirection);

    class GameAI
    {
        private Form form;
        private Debug debug;
        private GameEventHandler forminteract;
        private Timer mainTimer;
        private MainGameObject mgo;
        internal World world;
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
            world.Draw(g);
            gameControlObj.Draw(g);
            mgo.Draw(g);
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
            this.mgo.aiEventHandler = getEvent;
            this.world = world;
            this.gameControlObj = gameControlObj;
            this.curkeys = new List<Keys>();
        }
        public GameAI(Form form, Debug debug, GameEventHandler forminteract, World world, MainGameObject maingameobject, GameControlObjects gameControlObj)
            : this(form, forminteract, world, maingameobject, gameControlObj)
        {
            this.debug = debug;
        }

        public void Check(object sender, EventArgs e)
        {
            // scrolling?
            if (mgo.Left < 100)
            {
                foreach (GameObject go in world.AllElements)
                {
                    go.Left += 15;
                }
                mgo.Left += 15;
            }
            else if (world.Settings.GameWindowWidth - mgo.Right < 100)
            {
                foreach (GameObject go in world.AllElements)
                {
                    go.Left -= 15;
                }
                mgo.Left -= 15;
            }


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
                world.Enemies[i].Check();
            }
            // check of all moving elements
            for (int i = 0; i < world.MovingElements.Count; i++)
            {
                world.MovingElements[i].Check();
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
                mainTimer.Enabled = false;
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
