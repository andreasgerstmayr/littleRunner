using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using littleRunner.Gamedata.Worlddata;
using littleRunner.Editordata;
using littleRunner.GameObjects;


namespace littleRunner
{
    public partial class ProgramSwitcher : Form
    {
        Game game;
        Editor editor;

        public ProgramSwitcher()
        {
            InitializeComponent();

            Globals.VideoRenderMode = VideoRenderMode.GDI;
            Highscore.FileName = "Highscore.lhs";
        }

        private void ProgramSwitcher_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();

            // check all files
            foreach (string filename in Files.All())
            {
                List<string> files = AnimateImage.getFiles(filename);

                foreach (string file in files)
                {
                    if (!File.Exists(file))
                    {
                        MessageBox.Show("File " + file + " not found!\n\nClosing ...", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        closelittleRunner_Click(sender, e);
                        break;
                    }
                }
            }
        }



        private void startgame_Click(object sender, EventArgs e)
        {
            game = new Game();
            if (!game.IsDisposed)
            {
                game.FormClosing += new FormClosingEventHandler(childrenFormClosing);
                game.Show();
                Hide();
            }
        }
        private void starteditor_Click(object sender, EventArgs e)
        {
            editor = new Editor();
            if (!editor.IsDisposed)
            {
                editor.FormClosing += new FormClosingEventHandler(childrenFormClosing);
                editor.Show();
                Hide();
            }
        }

        void childrenFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!e.Cancel)
                Show();
        }

        private void closelittleRunner_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void ProgramSwitcher_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                Hide();
        }

        private void homepage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(homepage.Text);
        }
    }
}