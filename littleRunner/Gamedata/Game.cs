using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;

using littleRunner.Drawing;
using littleRunner.Drawing.Helpers;
using littleRunner.Gamedata.Worlddata;
using littleRunner.GameObjects;
using littleRunner.GameObjects.MainGameObjects;
using littleRunner.Highscoredata;


namespace littleRunner
{
    public partial class Game : Form
    {
        GameAI ai;
        Drawing.DrawHandler drawHandler;
        World world;
        MainGameObjectMode lastMode;
        bool lastModeIsNull;
        string levelpack;
        GameControlObjects gameControlObjs;
        GameSession session;
        bool ignoreSizeChange;
        int top, left;
        int mgoChangeTop, mgoChangeLeft;


        public Game(string filename, PlayMode playMode, int mgotop, int mgoleft)
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            drawHandler = GetDraw.DrawHandler(this, Update);
            AnimateImage.Refresh = true;
            ignoreSizeChange = false;

            lastModeIsNull = true;
            levelpack = "";

            mgoChangeTop = mgotop;
            mgoChangeLeft = mgoleft;

            StartGame(filename, playMode);
        }
        public Game(string levelpack, string filename)
            : this("Data/Levels/" + levelpack + "/" + filename, PlayMode.Game, 0, 0)
        {
            this.levelpack = levelpack + "/";
        }

        public Game(string filename, PlayMode playMode, int top, int left, int mgotop, int mgoleft)
            : this(filename, playMode, mgotop, mgoleft)
        {
            this.top = top;
            this.left = left;
            this.FormBorderStyle = FormBorderStyle.None;
        }



        private void setCurrentViewport()
        {
            world.MGO.Left += mgoChangeLeft;
            world.MGO.Top += mgoChangeTop;
            world.Viewport.X -= mgoChangeLeft;
            world.Viewport.Y -= mgoChangeTop;
        }

        private void Game_Shown(object sender, EventArgs e)
        {
            if (world.PlayMode == PlayMode.GameInEditor)
            {
                this.Top = top + 1;
                this.Left = left + 1;
                this.Width--;
                this.Height -= 2;
            }
        }


        private void StartGame(string filename, PlayMode playMode)
        {
            // check if file exists
            if (!File.Exists(filename))
            {
                MessageBox.Show("Can't load " + filename + "!", "Error - File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            LoadingForm f = new LoadingForm();
            f.Show();

            f.Message("Set title");
            // set form title
            string title = "";

            if (filename.StartsWith(Path.GetTempPath()))
            {
                Text = "littleRunner";
            }
            else
            {
                int lastBackslash = filename.LastIndexOf("/");
                if (lastBackslash == -1)
                    title = filename.Substring(0);
                else
                    title = filename.Substring(lastBackslash + 1);

                int lastDot = title.LastIndexOf(".");
                if (lastDot != -1)
                {
                    title = title.Substring(0, lastDot);
                }

                Text = "littleRunner - " + title;
            }

            // main game AI - world neets AiEventHandler
            f.Message("Creating Game AI");
            ai = new GameAI(GameAIInteract);


            if (session == null) // first run or complete new run (after game over)
            {
                session = new GameSession();
            }


            // The world
            f.Message("Creating World");
            world = new World(filename, drawHandler, ai.getEvent, session, playMode);


            // Main game object
            f.Message("Creating MGO");
            Tux tux = new Tux(Globals.Scroll.Top, Globals.Scroll.X);
            tux.Init(world, ai.getEvent); // can init


            // got MGO!
            f.Message("Initializing World");
            world.Init(tux);


            // change MGO if needed
            if (mgoChangeTop != 0 || mgoChangeLeft != 0)
                setCurrentViewport();


            // change window size
            ignoreSizeChange = true;
            Width = world.Settings.GameWindowWidth + 5;
            Height = world.Settings.GameWindowHeight + 29;
            ignoreSizeChange = false;


            // GameControls
            f.Message("Creating GameControlObjects");
            if (gameControlObjs == null) // first run or complete new run (after game over)
            {
                GameControl_Score gameControlObjScore = new GameControl_Score(15, 20, "Verdana", 12);
                GameControl_Points gameControlObjPoints = new GameControl_Points(15, Width / 2 - 120 / 2, "Verdana", 12);
                GameControl_Lives gameControlObjLives = new GameControl_Lives(15, Width - 140, 4, "Verdana", 12);
                GameControl_FPS gameControlObjFPS = new GameControl_FPS(40, Width - 140, "Verdana", 12);
                GameControl_Sound gameControlObjSound = new GameControl_Sound();

                gameControlObjs = new GameControlObjects(gameControlObjScore, gameControlObjPoints, gameControlObjLives, gameControlObjFPS, gameControlObjSound);
                gameControlObjs.OnKeyPress('f');
            }


            // init AI with the world - now we have the GameControlObjects
            f.Message("Initializing Game AI");
            ai.Init(world, gameControlObjs);

            // init Script Engine
            f.Message("Initializing ScriptEngine");
            ai.InitScript();


            if (!lastModeIsNull)
                ai.World.MGO.Mode = lastMode;

            f.Message("Initializing Sound");
            gameControlObjs.Sound.Start();
            f.Close();
        }

        private void CloseGame()
        {
            ai.Quit();
            ai = null;
            Close();
        }

        private void GameAIInteract(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
            if (ai == null)
                return;

            if (gevent == GameEvent.dead || gevent == GameEvent.outOfRange)
            {
                if (gameControlObjs.Lives > 0)
                {
                    ai.Pause(false);
                    string lastFileName = ai.World.fileName;
                    ai = null;
                    lastModeIsNull = true; // start with standard mode after death

                    gameControlObjs.Lives--;
                    if (world.PlayMode == PlayMode.Game || world.PlayMode == PlayMode.Editor)
                        StartGame(lastFileName, world.PlayMode);
                    else if (world.PlayMode == PlayMode.GameInEditor)
                        Close();
                }
                else
                {
                    if (world.PlayMode == PlayMode.Game || world.PlayMode == PlayMode.Editor)
                    {
                        MessageBox.Show("Game Over.", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        HighscoreForm hForm = new HighscoreForm(ai.FullScore);
                        hForm.ShowDialog();

                        DialogResult dr = MessageBox.Show("Play again?", "Game Over", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        lastModeIsNull = true; // start normal.

                        if (dr == DialogResult.Yes)
                        {
                            ai.Quit();
                            ai = null;
                            gameControlObjs = null; // set new points+sound!
                            session = null; // new session

                            StartGame("Data/Levels/" + levelpack + "start.lrl", world.PlayMode);
                        }
                        else if (dr == DialogResult.No)
                            CloseGame();
                    }
                    else if (world.PlayMode == PlayMode.GameInEditor)
                        CloseGame();
                }
            }
            else if (gevent == GameEvent.finishedLevel)
            {
                if (world.PlayMode == PlayMode.Game || world.PlayMode == PlayMode.Editor)
                {
                    string nextLevel = (string)args[GameEventArg.nextLevel];
                    if (nextLevel != null && nextLevel != "")
                    {
                        ai.Pause(false); // play again, so save last MGO mode
                        lastMode = ai.World.MGO.Mode;
                        lastModeIsNull = false; // it's set to the last mode

                        ai = null;
                        int nextLevelStartAt = (int)args[GameEventArg.nextLevelStartAt];
                        StartGame("Data/Levels/" + levelpack + nextLevel, world.PlayMode);
                        if (ai != null)
                        {
                            ai.World.MGO.Left += nextLevelStartAt;
                            ai.World.Viewport.X -= nextLevelStartAt;
                        }
                    }
                    else
                    {
                        int gotScore = ai.FullScore;
                        ai.Quit();
                        ai = null;
                        lastModeIsNull = true;

                        MessageBox.Show("Congratulations!\nYou 've played all predefined littleRunner levels.\n\nNow start making your own level with the level-editor :-).", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        HighscoreForm hForm = new HighscoreForm(gotScore);
                        hForm.ShowDialog();

                        Close();
                    }
                }
                else if (world.PlayMode == PlayMode.GameInEditor)
                    CloseGame();
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Left:
                case Keys.Down:
                case Keys.Right:
                    return true;
            }

            return base.IsInputKey(keyData);
        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            if (ai != null)
                ai.Interact(e.KeyCode, true);
        }

        private void Game_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ai != null && world != null)
            {
                if (world.PlayMode == PlayMode.GameInEditor
                && e.KeyChar == (char)Keys.Escape)
                    Close();

                if (e.KeyChar == (char)Keys.Return)
                {
                    if (ai.IsRunning)
                        ai.Pause(false);
                    else
                        ai.Pause(true);
                }
            }

            if (gameControlObjs != null)
                gameControlObjs.OnKeyPress(e.KeyChar);
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            if (ai != null)
                ai.Interact(e.KeyCode, false);
        }


        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ai != null)
                ai.Quit();

            if (gameControlObjs != null)
                gameControlObjs.Sound.Stop();
        }

        private void Game_SizeChanged(object sender, EventArgs e)
        {
            if (!ignoreSizeChange)
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    if (ai != null)
                        ai.Pause(false);
                }
                else if (WindowState == FormWindowState.Normal)
                {
                    if (ai != null)
                        ai.Pause(true);
                }
            }
        }

        private void Update(Draw d)
        {
            if (ai != null)
                ai.Update(d);
        }
    }
}