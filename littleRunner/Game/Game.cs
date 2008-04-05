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
        GameControlObjects gameControlObjs;
        bool editorOpened;


        public Game(ProgramSwitcher programSwitcher)
        {
            InitializeComponent();

            editorOpened = false;
            this.programSwitcher = programSwitcher;
            StartGame("Data/Levels/level1.lrl");
        }
        public Game(ProgramSwitcher programSwitcher, string filename)
        {
            InitializeComponent();

            editorOpened = true;
            this.programSwitcher = programSwitcher;
            StartGame(filename);
        }

        private void StartGame(string filename)
        {
            // check if file exists
            if (!File.Exists(filename))
            {
                MessageBox.Show("Can't load " + filename + "!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            else
            {
                // The world
                World world = new World(filename, Invalidate, true);

                Width = world.Settings.GameWindowWidth+5;
                Height = world.Settings.LevelHeight+29;

                // all enemys need special things
                foreach (Enemy enemie in world.Enemies)
                {
                    enemie.Init(world);
                }


                // Main game object
                Tux tux = new Tux(0, 100);

                tux.Init(world);
                world.Init(tux);


                // GameControls
                if (gameControlObjs == null) // first run
                {
                    GameControl_Points gameControlObjPoints = new GameControl_Points(18, Width - 55, "Verdana", 15);
                    GameControl_Sound gameControlObjSound = new GameControl_Sound();

                    gameControlObjs = new GameControlObjects(gameControlObjPoints, gameControlObjSound);
                }

                // main game AI
                ai = new GameAI(this, GameAIInteract, world, tux, gameControlObjs);
                ai.Init();
                gameControlObjs.Sound.Start();
            }
        }

        private void GameAIInteract(GameEvent gevent, Dictionary<GameEventArg, object> args)
        {
            if (gevent == GameEvent.dead || gevent == GameEvent.outOfRange)
            {
                DialogResult dr = MessageBox.Show("Play again?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (dr == DialogResult.Yes)
                {
                    ai.Quit();

                    StartGame(ai.world.fileName);
                }
                else if (dr == DialogResult.No)
                    Close();
            }
            else if (gevent == GameEvent.finishedLevel)
            {
                string nextLevel = (string)args[GameEventArg.nextLevel];
                if (nextLevel != null && nextLevel != "")
                {
                    ai.Stop();
 
                    StartGame((string)args[GameEventArg.nextLevel]);
                }
                else
                {
                    ai.Quit();

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
            ai.Draw(e.Graphics);
        }
    }
}