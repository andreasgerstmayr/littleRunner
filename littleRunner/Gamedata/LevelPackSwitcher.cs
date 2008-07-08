using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;


namespace littleRunner.Gamedata
{
    public partial class LevelPackSwitcher : Form
    {
        FormClosingEventHandler progSwitchHandler;

        public LevelPackSwitcher(FormClosingEventHandler progSwitchHandler)
        {
            InitializeComponent();
            this.progSwitchHandler = progSwitchHandler;

            foreach (string levelpackdir in Directory.GetDirectories("Data/Levels"))
            {
                string[] levelpackPieces = levelpackdir.Split('/', '\\');
                string levelpack = levelpackPieces[levelpackPieces.Length - 1];

                levelpacks.Items.Add(levelpack);
            }


            if (levelpacks.Items.Count == 1)
            {
                levelpacks.SelectedIndex = 0;
                levelpacks_DoubleClick(this, new EventArgs());
            }
        }

        private void levelpacks_DoubleClick(object sender, EventArgs e)
        {
            this.FormClosing -= progSwitchHandler;
            Close();

            string selectedLevelPack = levelpacks.SelectedItem.ToString();
            Game game = new Game(selectedLevelPack, "start.lrl");
            game.FormClosing += new FormClosingEventHandler(progSwitchHandler);
            game.Show();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
