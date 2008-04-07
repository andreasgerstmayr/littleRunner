using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace littleRunner
{
    public partial class Game : Form
    {
        ProgramSwitcher programSwitcher;
        GameAI ai;
        MainGameObjectMode lastMode;
        bool lastModeIsNull;
        GameControlObjects gameControlObjs;
        bool editorOpened;


        internal GameAI AI
        {
            get { return ai; }
        }

        public Game(ProgramSwitcher programSwitcher)
        {
            InitializeComponent();

            editorOpened = false;
            this.programSwitcher = programSwitcher;

            lastModeIsNull = true;
            StartGame("Data/Levels/level1.lrl");
        }
        public Game(ProgramSwitcher programSwitcher, string filename)
        {
            InitializeComponent();

            editorOpened = true;
            this.programSwitcher = programSwitcher;

            lastModeIsNull = true;
            StartGame(filename);
        }

        private void StartGame(string filename)
        {
            // check if file exists
            if (!File.Exists(filename))
            {
                MessageBox.Show("Can't load " + filename + "!", "Error - File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            else
            {
                // set form title
                string title = "";

                int lastBackslash = filename.LastIndexOf("/");
                if (lastBackslash == -1)
                    title = filename.Substring(0);
                else
                    title = filename.Substring(lastBackslash+1);

                int lastDot = title.LastIndexOf(".");
                if (lastDot != -1)
                {
                    title = title.Substring(0, lastDot);
                }

                Text = "littleRunner - " + title;


                // The world
                World world = new World(filename, Invalidate, true);

                Width = world.Settings.GameWindowWidth+5;
                Height = world.Settings.LevelHeight+29;

                // all enemies need special things
                foreach (Enemy enemie in world.Enemies)
                {
                    enemie.Init(world);
                }


                // Main game object
                Tux tux = new Tux(0, 100);

                tux.Init(world);
                world.Init(tux);


                // GameControls
                if (gameControlObjs == null) // first run or complete new run (after game over)
                {
                    GameControl_Points gameControlObjPoints = new GameControl_Points(18, Width - 140, "Verdana", 12);
                    GameControl_Lives gameControlObjLives = new GameControl_Lives(4, 40, Width - 140, "Verdana", 12);
                    GameControl_Sound gameControlObjSound = new GameControl_Sound();

                    gameControlObjs = new GameControlObjects(gameControlObjPoints, gameControlObjLives, gameControlObjSound);
                }

                // main game AI
                ai = new GameAI(this, GameAIInteract, world, tux, gameControlObjs);
                ai.Init();

                if (!lastModeIsNull)
                   ai.world.MGO.currentMode = lastMode;

                gameControlObjs.Sound.Start();
            }
        }

        private void GameAIInteract(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
            if (gevent == GameEvent.dead || gevent == GameEvent.outOfRange)
            {
                if (gameControlObjs.Lives > 0)
                {
                    ai.Stop();
                    string lastFileName = ai.world.fileName;
                    ai = null;
                    lastModeIsNull = true; // start with standard mode after death

                    gameControlObjs.Lives--;
                    StartGame(lastFileName);
                }
                else
                {
                    DialogResult dr = MessageBox.Show("Game Over.\n\nPlay again?", "Game Over", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    lastModeIsNull = true; // start normal.

                    if (dr == DialogResult.Yes)
                    {
                        ai.Quit();
                        ai = null;
                        gameControlObjs = null; // set new points+sound!

                        StartGame("Data/Levels/level1.lrl");
                    }
                    else if (dr == DialogResult.No)
                    {
                        ai = null;
                        Close();
                    }
                }
            }
            else if (gevent == GameEvent.finishedLevel)
            {
                string nextLevel = (string)args[GameEventArg.nextLevel];
                if (nextLevel != null && nextLevel != "")
                {
                    ai.Stop(); // play again, so save last MGO mode
                    lastMode = ai.world.MGO.currentMode;
                    lastModeIsNull = false; // it's set to the last mode

                    ai = null;

                    StartGame("Data/Levels/" + nextLevel);
                }
                else
                {
                    ai.Quit();
                    ai = null;
                    lastModeIsNull = true;

                    MessageBox.Show("Congratulations!\nYou 've played all predefined littleRunner levels.\n\nNow start making your own level with the level-editor :-).", "Congratulations");
                    Close();
                }
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
            if (WindowState == FormWindowState.Minimized)
            {
                if (ai != null)
                    ai.Stop();
            }
            else if (WindowState == FormWindowState.Normal)
            {
                if (ai != null)
                    ai.PlayAgain();
            }
        }
    }
}