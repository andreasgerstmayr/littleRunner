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

        public static string GetStartLevelName(string levelpack, bool slashed)
        {
            string[] infofile = File.ReadAllLines("Data/Levels/" + levelpack + (slashed ? "" : "/") + "info.txt");
            return infofile[0].Substring(7);
        }


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
                levelpacks_Click(this, new EventArgs());
            }
        }

        private void levelpacks_Click(object sender, EventArgs e)
        {
            this.FormClosing -= progSwitchHandler;
            Close();

            string selectedLevelPack = levelpacks.SelectedItem.ToString();
            Game game = new Game(selectedLevelPack, GetStartLevelName(selectedLevelPack, false));
            game.FormClosing += new FormClosingEventHandler(progSwitchHandler);
            game.Show();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
