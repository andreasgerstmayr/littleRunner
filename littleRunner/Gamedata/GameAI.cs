using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;

using littleRunner.Drawing;
using littleRunner.Gamedata;
using littleRunner.Gamedata.Worlddata;
using littleRunner.GameObjects;


namespace littleRunner
{
    public delegate void GameEventHandler(GameEvent gevent, Dictionary<GameEventArg, object> args);
    public delegate void GameCrashHandler(GameEvent gevent, GameDirection cidirection);

    public class GameAI : IDisposable
    {
        private SynchronizationContext guiContext;
        private GameEventHandler guiinteract;
        private System.Windows.Forms.Timer mainTimer;
        public World World;
        private GameControlObjects gameControlObj;
        private List<Keys> curkeys;
        private List<GameKey> pressedKeys;
        float scrollTop;
        private static Stopwatch watch;
        private static Stopwatch tempwatch;
        private Thread checkThread;


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


        public static float FrameFactor
        {
            get { return Thread.CurrentThread.Name == null ? (float)watch.Elapsed.TotalSeconds : (float)tempwatch.Elapsed.TotalSeconds; }
        }
        public delegate float GetFrameFactorDelegate();
        public static float GetFrameFactor()
        {
            return FrameFactor;
        }
        public bool IsRunning
        {
            get { return mainTimer.Enabled; }
        }
        public int FullScore
        {
            get { return gameControlObj.Score + gameControlObj.Lives * 100; }
        }


        void IDisposable.Dispose()
        {
            if (checkThread.ThreadState == System.Threading.ThreadState.Running ||
                checkThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
                checkThread.Abort();
        }

        public void Pause(bool start)
        {
            if (start)
                mainTimer.Enabled = true;
            else
            {
                mainTimer.Enabled = false;
                watch.Reset();
                tempwatch.Reset();
            }
        }
        public void Quit()
        {
            Pause(false);
            gameControlObj.Score = 0;
            gameControlObj.Points = 0;
        }

        public GameAI(GameEventHandler forminteract)
        {
            guiContext = SynchronizationContext.Current;
            if (guiContext == null)
                guiContext = new SynchronizationContext();


            watch = new Stopwatch();
            tempwatch = new Stopwatch();

            scrollTop = 0;
            this.guiinteract = forminteract;
            this.curkeys = new List<Keys>();
            this.pressedKeys = new List<GameKey>();

            mainTimer = new System.Windows.Forms.Timer();
            mainTimer.Tick += new EventHandler(Check);
            mainTimer.Interval = 1;
            mainTimer.Enabled = false;
        }
        private void forminteract2(object state)
        {
            // now make the real call.
            object[] arr = (object[])state;
            GameEvent gevent = (GameEvent)arr[0];
            Dictionary<GameEventArg, object> args = (Dictionary<GameEventArg, object>)arr[1];

            guiinteract(gevent, args);
        }
        private void forminteract(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
            if (Thread.CurrentThread.Name == null)
            {
                guiinteract(gevent, args);
                return;
            }


            // maybe in other thread, so make data array and move to GUI-Thread!
            object[] state = new object[2];
            state[0] = gevent;
            state[1] = args;

            guiContext.Send(forminteract2, state);
        }

        public void Init(World world, GameControlObjects gameControlObj)
        {
            mainTimer.Enabled = true;

            this.gameControlObj = gameControlObj;
            this.World = world;

            Cheat.Init(this);
        }
        public void InitScript()
        {
            // Init script
            string msg = this.World.InitScript();

            // NOTE: if msg == null, the levelscript has closed the game [finished level/BonusLevel]
            if (msg != null && msg != "")
            {
                if (this.World.PlayMode == PlayMode.Game)
                    throw new littleRunnerScriptException(msg);
                else
                    MessageBox.Show("Can't load script.", "Script error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Check(object sender, EventArgs e)
        {
            Dictionary<string, float> newMGOpos = World.MGO.Check(pressedKeys);
            float changeY = Math.Abs(newMGOpos["newtop"]);
            float changeX = Math.Abs(newMGOpos["newleft"]);


            #region Scrolling
            bool scrolled = false;

            // scrolling Top/Bottom
            if (World.Viewport.Y + World.MGO.Top < Globals.Scroll.Top)
            {
                World.Viewport.Y += changeY; // scroll top
                scrollTop += changeY;
                scrolled = true;
            }
            else if (World.Settings.GameWindowHeight - World.Viewport.Y - World.MGO.Bottom < Globals.Scroll.Bottom)
            {
                World.Viewport.Y -= changeY; // scroll bottom
                scrolled = true;
            }

            // scrolling Left/Right
            if (World.Viewport.X + World.MGO.Left < Globals.Scroll.X)
                World.Viewport.X += changeX; // scroll left
            else if (World.Settings.GameWindowWidth - World.Viewport.X - World.MGO.Right < Globals.Scroll.X)
                World.Viewport.X -= changeX; // scroll right

            // if not scrolled
            //    and need to scroll bottom
            //    and don't need to scroll top
            //    and when scroll down don't need to scroll top
            // scroll down in pieces (SCROLL_CHANGE_Y).
            if (!scrolled && scrollTop > 0 &&
                !(World.Viewport.Y + World.MGO.Top < Globals.Scroll.Top) &&
                !(World.Viewport.Y - changeY + World.MGO.Top < Globals.Scroll.Top))
            {
                World.Viewport.Y -= changeY;
                scrollTop -= changeY;
            }
            #endregion


            if (checkThread == null || checkThread.ThreadState == System.Threading.ThreadState.Stopped)
            {
                checkThread = new Thread(CheckThread);
                checkThread.Name = "Checker2";
                checkThread.Start();
            }


            // Repaint
            World.DrawHandler.Update();


            if (FrameFactor != 0.0)
                gameControlObj.FPS = (int)(1.0 / FrameFactor);

            watch.Reset(); // reset current
            watch.Start(); // current frame calculating finished. now, start counting.
        }

        private void CheckThread()
        {
            try
            {
                if (World.Script != null)
                    World.Script.callFunction("AI", "Check");


                // check of all enemies
                for (int i = 0; i < World.Enemies.Count; i++)
                {
                    if (!World.Enemies[i].StartAtViewpoint || World.Enemies[i].Left < World.Settings.GameWindowWidth - World.Viewport.X)
                    {
                        Dictionary<string, float> newpos;
                        World.Enemies[i].Check(out newpos);
                    }
                }
                // check of all moving elements
                for (int i = 0; i < World.MovingElements.Count; i++)
                {
                    Dictionary<string, float> newpos;
                    World.MovingElements[i].Check(out newpos);
                }

                tempwatch.Reset(); // reset current
                tempwatch.Start(); // reset current
            }
            catch (ThreadAbortException)
            {
            }
        }


        public void Interact(Keys key, bool pressed)
        {
            if (pressed && !curkeys.Contains(key))
            {
                curkeys.Add(key);
                Cheat.Pressed(key);
            }
            else if (!pressed)
                curkeys.Remove(key);


            pressedKeys.Clear();

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
                gameControlObj.Points = 0;
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
