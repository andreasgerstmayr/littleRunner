using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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

            // check all files in resources
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

        private void openhelp_Click(object sender, EventArgs e)
        {
            Help h = new Help();
            h.ShowDialog(); 
        }

        private void openabout_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }


        private void closelittleRunner_Click(object sender, EventArgs e)
        {
            canClose = true;
            Close();
        }

        private void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
        }

        private void startGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startgame_Click(sender, e);
        }

        private void startEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            starteditor_Click(sender, e);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openhelp_Click(sender, e);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openabout_Click(sender, e);
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

    }
}