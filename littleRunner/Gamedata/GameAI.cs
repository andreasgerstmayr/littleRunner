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
        private static Dictionary<string, StopwatchExtended> watch;
        private Dictionary<string, Thread> thread;
        private Dictionary<string, ThreadStart> threadDelegate;


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
            get
            {
                return (float)watch[Thread.CurrentThread.Name == null ? "" :
                                    Thread.CurrentThread.Name                ].Elapsed.TotalSeconds;
            }
        }
        public static StopwatchExtended CurWatch
        {
            get
            {
                return watch[Thread.CurrentThread.Name == null ? "" :
                             Thread.CurrentThread.Name                ];
            }
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
            Dispose(true);
        }
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (Thread t in thread.Values)
                {
                    if (t.ThreadState == System.Threading.ThreadState.Running ||
                        t.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
                        t.Abort();
                }
            }
        }

        public void Pause(bool start)
        {
            if (start)
                mainTimer.Enabled = true;
            else
            {
                mainTimer.Enabled = false;
                foreach (StopwatchExtended w in watch.Values)
                {
                    w.Reset();
                }
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


            // threads
            threadDelegate = new Dictionary<string, ThreadStart>();
            threadDelegate.Add("MGO", MGO_Thread);
            threadDelegate.Add("Paint", Paint_Thread);
            threadDelegate.Add("Checker1", Checker1_Thread);
            threadDelegate.Add("Checker2", Checker2_Thread);
            threadDelegate.Add("Checker3", Checker3_Thread);


            watch = new Dictionary<string, StopwatchExtended>();
            watch[""] = new StopwatchExtended(); // GUI-Thread

            thread = new Dictionary<string, Thread>();
            foreach (string s in threadDelegate.Keys)
            {
                watch[s] = new StopwatchExtended();
                thread[s] = null;
            }


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
            StartThreadCycle();


            foreach (KeyValuePair<string, ThreadStart> t in threadDelegate)
            {
                if (thread[t.Key] == null || thread[t.Key].ThreadState == System.Threading.ThreadState.Stopped)
                {
                    thread[t.Key] = new Thread(t.Value);
                    thread[t.Key].Name = t.Key;
                    thread[t.Key].Start();
                }
            }


            if (FrameFactor != 0.0)
                gameControlObj.FPS = (int)(1.0F / FrameFactor);


            EndThreadCycle();
        }

        #region Threads
        private void StartThreadCycle()
        {
            if (FrameFactor > Globals.MaxCycleDuration)
                CurWatch.Elapsed = new TimeSpan(0, 0, 0, 0, 1); // don't reset, otherwise eg gumbas
                                                                // will change direction
        }
        private void EndThreadCycle()
        {
            CurWatch.Reset(); // reset current
            CurWatch.Start(); // start current
        }
        private void MGO_Thread()
        {
            try
            {
                StartThreadCycle();


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


                EndThreadCycle();
            }
            catch (ThreadAbortException)
            {
            }
        }
        private void Paint_Thread()
        {
            try
            {
                World.DrawHandler.Update();
            }
            catch (ThreadAbortException)
            {
            }
        }
        private void Checker1_Thread()
        {
            try
            {
                StartThreadCycle();

               
                if (World.Script != null)
                    World.Script.callFunction("AI", "Check");


                EndThreadCycle();
            }
            catch (ThreadAbortException)
            {
            }
        }
        private void Checker2_Thread()
        {
            try
            {
                StartThreadCycle();


                // check of all enemies
                for (int i = 0; i < World.Enemies.Count; i++)
                {
                    if (!World.Enemies[i].StartAtViewpoint || World.Enemies[i].Left < World.Settings.GameWindowWidth - World.Viewport.X)
                    {
                        Dictionary<string, float> newpos;
                        World.Enemies[i].Check(out newpos);
                    }
                }


                EndThreadCycle();
            }
            catch (ThreadAbortException)
            {
            }
        }
        private void Checker3_Thread()
        {
            try
            {
                StartThreadCycle();


                // check of all moving elements
                for (int i = 0; i < World.MovingElements.Count; i++)
                {
                    Dictionary<string, float> newpos;
                    World.MovingElements[i].Check(out newpos);
                }


                EndThreadCycle();
            }
            catch (ThreadAbortException)
            {
            }
        }
        #endregion


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
