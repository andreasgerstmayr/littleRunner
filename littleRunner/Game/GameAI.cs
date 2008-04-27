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


    public delegate void GameEventHandler(GameEvent gevent, Dictionary<GameEventArg, object> args);
    public delegate void GameCrashHandler(GameEvent gevent, GameDirection cidirection);

    public class GameAI
    {
        private Form form;
        private GameEventHandler forminteract;
        private Timer mainTimer;
        public World World;
        private GameControlObjects gameControlObj;
        private List<Keys> curkeys;


        public void Draw(Graphics g)
        {
            World.Draw(g, true);
            gameControlObj.Draw(g);
            g.TranslateTransform(World.Viewport, 0);
            World.MGO.Draw(g);
            g.TranslateTransform(-World.Viewport, 0);
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

            mainTimer = new Timer();
            mainTimer.Tick += new EventHandler(Check);
            mainTimer.Interval = 1;
            mainTimer.Enabled = false;
        }
        public void Init(World world, GameControlObjects gameControlObj)
        {
            mainTimer.Enabled = true;

            this.gameControlObj = gameControlObj;
            this.World = world;

            this.InitScript();
        }
        private void InitScript()
        {
            // Init script
            string msg = this.World.InitScript();

            if (msg != "")
            {
                if (this.World.PlayMode == PlayMode.Game)
                    throw new littleRunnerScriptException(msg);
                else
                    MessageBox.Show("Can't load script.", "Script error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Check(object sender, EventArgs e)
        {
            if (World.Script != null)
                World.Script.callFunction("AI", "Check");


            // scrolling?
            if (World.Viewport+World.MGO.Left < 140)
                World.Viewport += 15; // scroll left
            else if (World.Settings.GameWindowWidth-World.Viewport - World.MGO.Right < 140)
                World.Viewport -= 15; // scroll right


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


            World.MGO.Check(pressedKeys);

            // check of all enemies
            for (int i = 0; i < World.Enemies.Count; i++)
            {
                if (!World.Enemies[i].StartAtViewpoint || World.Enemies[i].Left < World.Settings.GameWindowWidth - World.Viewport)
                {
                    Dictionary<string, int> newpos;
                    World.Enemies[i].Check(out newpos);
                }
            }
            // check of all moving elements
            for (int i = 0; i < World.MovingElements.Count; i++)
            {
                Dictionary<string, int> newpos;
                World.MovingElements[i].Check(out newpos);
            }

            // mgo out of range?
            if (World.MGO.Top > form.Height)
            {
                mainTimer.Enabled = false;
                forminteract(GameEvent.outOfRange, new Dictionary<GameEventArg, object>());
            }

            // Repaint
            World.Invalidate();
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

                if (gameControlObj.Points >= 1000)
                {
                    gameControlObj.Lives++;
                    gameControlObj.Points -= 1000;
                }
            }
            else if (gevent == GameEvent.outOfRange || gevent == GameEvent.dead)
            {
                Pause(false);
                forminteract(gevent, args);
            }
            else if (gevent == GameEvent.finishedLevel)
            {
                Pause(false);
                forminteract(gevent, args);
            }
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
