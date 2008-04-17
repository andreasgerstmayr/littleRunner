using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;


using littleRunner.GameObjects;
using littleRunner.GameObjects.MainGameObjects;

namespace littleRunner
{
    public partial class Game : Form
    {
        ProgramSwitcher programSwitcher;
        GameAI ai;
        World world;
        MainGameObjectMode lastMode;
        bool lastModeIsNull;
        GameControlObjects gameControlObjs;
        bool editorOpened;
        bool ignoreSizeChange;
        int top, left;

        public GameAI AI
        {
            get { return ai; }
        }

        public Game()
        {
            ignoreSizeChange = false;
        }
        public Game(ProgramSwitcher programSwitcher) : this()
        {
            InitializeComponent();

            editorOpened = false;
            this.programSwitcher = programSwitcher;

            lastModeIsNull = true;
            StartGame("Data/Levels/level1.lrl", PlayMode.Game);
        }
        public Game(ProgramSwitcher programSwitcher, string filename, PlayMode playMode) : this()
        {
            InitializeComponent();

            editorOpened = true;
            this.programSwitcher = programSwitcher;

            lastModeIsNull = true;
            StartGame(filename, playMode);
        }
        public Game(ProgramSwitcher programSwitcher, string filename, PlayMode playMode, int top, int left)
            : this(programSwitcher, filename, playMode)
        {
            this.top = top;
            this.left = left;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void Game_Shown(object sender, EventArgs e)
        {
            if (world.PlayMode == PlayMode.GameInEditor)
            {
                this.Top = top+3;
                this.Left = left+4;
                this.Width++;
            }
        }


        private void StartGame(string filename, PlayMode playMode)
        {
            // check if file exists
            if (!File.Exists(filename))
            {
                MessageBox.Show("Can't load " + filename + "!", "Error - File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            else
            {
                /* set loading-label
                Label loading = new Label();
                loading.Top = this.Height/2 - 70;
                loading.Left = this.Width/2 + 40;
                loading.Text = "Loading ...";
                loading.Font = new Font("Verdana", 10, FontStyle.Bold);
                Controls.Add(loading);
                */


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
                ai = new GameAI(this, GameAIInteract);

                // The world
                world = new World(filename, Invalidate, ai.getEvent, playMode);

                // Main game object
                Tux tux = new Tux(0, 140);
                tux.Init(world, ai.getEvent); // can init

                // got MGO!
                world.Init(tux);


                // change window size
                ignoreSizeChange = true;
                Width = world.Settings.GameWindowWidth + 5;
                Height = world.Settings.LevelHeight + 29;
                ignoreSizeChange = false;


                // GameControls
                if (gameControlObjs == null) // first run or complete new run (after game over)
                {
                    GameControl_Points gameControlObjPoints = new GameControl_Points(18, Width - 140, "Verdana", 12);
                    GameControl_Lives gameControlObjLives = new GameControl_Lives(4, 40, Width - 140, "Verdana", 12);
                    GameControl_Sound gameControlObjSound = new GameControl_Sound();

                    gameControlObjs = new GameControlObjects(gameControlObjPoints, gameControlObjLives, gameControlObjSound);
                }

                // init AI with the world - now we have the GameControlObjects
                ai.Init(world, tux, gameControlObjs);


                if (!lastModeIsNull)
                   ai.world.MGO.Mode = lastMode;

                gameControlObjs.Sound.Start();

                // Controls.Remove(loading);
            }
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
                    string lastFileName = ai.world.fileName;
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
                        DialogResult dr = MessageBox.Show("Game Over.\n\nPlay again?", "Game Over", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        lastModeIsNull = true; // start normal.

                        if (dr == DialogResult.Yes)
                        {
                            ai.Quit();
                            ai = null;
                            gameControlObjs = null; // set new points+sound!

                            StartGame("Data/Levels/level1.lrl", world.PlayMode);
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
                        lastMode = ai.world.MGO.Mode;
                        lastModeIsNull = false; // it's set to the last mode

                        ai = null;
                        int nextLevelStartAt = (int)args[GameEventArg.nextLevelStartAt];
                        StartGame("Data/Levels/" + nextLevel, world.PlayMode);
                        if (ai != null)
                            ai.Scroll(-nextLevelStartAt, false);
                    }
                    else
                    {
                        ai.Quit();
                        ai = null;
                        lastModeIsNull = true;

                        MessageBox.Show("Congratulations!\nYou 've played all predefined littleRunner levels.\n\nNow start making your own level with the level-editor :-).", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!editorOpened)
                programSwitcher.Show();
        }

        private void Game_Paint(object sender, PaintEventArgs e)
        {
            if (ai != null)
                ai.Draw(e.Graphics);
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
    }
}