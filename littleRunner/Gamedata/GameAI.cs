using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using littleRunner.Drawing;
using littleRunner.Gamedata.Worlddata;
using littleRunner.GameObjects;


namespace littleRunner
{
    public delegate void GameEventHandler(GameEvent gevent, Dictionary<GameEventArg, object> args);
    public delegate void GameCrashHandler(GameEvent gevent, GameDirection cidirection);

    public class GameAI
    {
        private GameEventHandler forminteract;
        private Timer mainTimer;
        public World World;
        private GameControlObjects gameControlObj;
        private List<Keys> curkeys;
        int scrollTop;


        public void Update(Draw d)
        {
            if (World == null)
                return;

            World.Update(d);
            gameControlObj.Update(d);
            d.MoveCoords(World.Viewport.X, World.Viewport.Y);
            World.MGO.Update(d);
            d.MoveCoords(-World.Viewport.X, -World.Viewport.Y);
        }
        public bool IsRunning
        {
            get { return mainTimer.Enabled; }
        }

        public int FullScore
        {
            get { return gameControlObj.Score + gameControlObj.Lives * 100; }
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
            gameControlObj.Score = 0;
            gameControlObj.Points = 0;
        }

        public GameAI(GameEventHandler forminteract)
        {
            scrollTop = 0;
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
        }
        public void InitScript()
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

            List<GameKey> pressedKeys = new List<GameKey>();

            #region check if key pressed
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
            if (curkeys.Contains(Keys.ControlKey))
                pressedKeys.Add(GameKey.runFast);
            #endregion


            Dictionary<string, int> newMGOpos = World.MGO.Check(pressedKeys);
            int changeY = Math.Abs(newMGOpos["newtop"]);
            int changeX = Math.Abs(newMGOpos["newleft"]);

            #region Scrolling
            bool scrolled = false;

            // scrolling Top/Bottom
            if (World.Viewport.Y + World.MGO.Top < Globals.SCROLL_TOP)
            {
                World.Viewport.Y += changeY; // scroll top
                scrollTop += changeY;
                scrolled = true;
            }
            else if (World.Settings.GameWindowHeight - World.Viewport.Y - World.MGO.Bottom < Globals.SCROLL_BOTTOM)
            {
                World.Viewport.Y -= changeY; // scroll bottom
                scrolled = true;
            }

            // scrolling Left/Right
            if (World.Viewport.X + World.MGO.Left < Globals.SCROLL_X)
                World.Viewport.X += changeX; // scroll left
            else if (World.Settings.GameWindowWidth - World.Viewport.X - World.MGO.Right < Globals.SCROLL_X)
                World.Viewport.X -= changeX; // scroll right

            // if not scrolled
            //    and need to scroll bottom
            //    and don't need to scroll top
            //    and when scroll down don't need to scroll top
            // scroll down in pieces (SCROLL_CHANGE_Y).
            if (!scrolled && scrollTop > 0 &&
                !(World.Viewport.Y + World.MGO.Top < Globals.SCROLL_TOP) &&
                !(World.Viewport.Y - changeY + World.MGO.Top < Globals.SCROLL_TOP))
            {
                World.Viewport.Y -= changeY;
                scrollTop -= changeY;
            }
            #endregion


            // check of all enemies
            for (int i = 0; i < World.Enemies.Count; i++)
            {
                if (!World.Enemies[i].StartAtViewpoint || World.Enemies[i].Left < World.Settings.GameWindowWidth - World.Viewport.X)
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

            // Repaint
            World.DrawHandler.Update();
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
                gameControlObj.Score += (int)args[GameEventArg.points]; 
                gameControlObj.Points += (int)args[GameEventArg.points];

                if (gameControlObj.Points >= 100)
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
            {
                Pause(false);
                forminteract(gevent, args);
            }
            else if (gevent == GameEvent.gotLive)
                gameControlObj.Lives++;
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
