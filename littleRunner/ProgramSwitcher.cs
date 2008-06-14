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

            worker.RunWorkerAsync();
        }
        private void worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Script s = new Script(new World());
        }



        private void startgame_Click(object sender, EventArgs e)
        {
            game = new Game(this);
            if (!game.IsDisposed)
            {
                game.Show();
                Hide();
            }
        }

        private void starteditor_Click(object sender, EventArgs e)
        {
            editor = new Editor(this);
            if (!editor.IsDisposed)
            {
                editor.Show();
                Hide();
            }
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