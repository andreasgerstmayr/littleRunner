using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace littleRunner
{
    public partial class ProgramSwitcher : Form
    {
        Game game;
        Editor editor;
        bool canClose;

        public ProgramSwitcher()
        {
            InitializeComponent();

            canClose = false;
        }

        private void ProgramSwitcher_Shown(object sender, EventArgs e)
        {
             // fill the Files.f-Dictionary
            Files.fill();


            // check all files
            foreach (KeyValuePair<gFile, string> pair in Files.f)
            {
                string filename = pair.Value;
                List<string> files = AnimateImage.getFiles(filename);

                foreach (string file in files)
                {
                    if (!File.Exists(file))
                    {
                        MessageBox.Show("File " + file + " not found!\n\nClosing ...", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        closeToolStripMenuItem_Click(sender, e);
                        break;
                    }
                }
            }
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
            canClose = true;
            Close();
        }

        private void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void startGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startgame_Click(sender, e);
        }

        private void startEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            starteditor_Click(sender, e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canClose = true;
            Close();
        }

        private void ProgramSwitcher_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!canClose)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void ProgramSwitcher_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                Hide();
        }
    }
}