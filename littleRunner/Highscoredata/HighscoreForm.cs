using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using littleRunner.Gamedata;


namespace littleRunner.Highscoredata
{
    public partial class HighscoreForm : Form
    {
        bool opened;

        public HighscoreForm(int score, DateTime started)
        {
            InitializeComponent();
            opened = false;

            this.score.Text = score.ToString();

            TimeSpan duration = DateTime.Now - started;
            this.time.Text = String.Format("{0:00}m {1:00}s", duration.TotalMinutes, duration.Seconds);
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (opened)
            {
                Close();
                return;
            }


            if (name.Text.Length == 0)
            {
                errProv.SetError(name, "Please enter a name.");
                return;
            }
            else
                errProv.SetError(name, null);
            

            if (Cheat.Activated)
                MessageBox.Show("Cheaters can't save highscores!", "Cheater alert [littleRunner]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                Highscore.Write(name.Text, Convert.ToInt32(score.Text), time.Text);


            name.Enabled = false;
            bButton.Enabled = false;

            // load highscores in panel
            highscores.RowStyles.Clear();
            int row = 0;
            foreach (Highscore.Data data in Highscore.Read())
            {
                highscores.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                Label rankLabel = new Label();
                rankLabel.Text = (row + 1).ToString() + ".";
                rankLabel.Font = new Font("Verdana", 12F, FontStyle.Bold);
                rankLabel.AutoSize = true;
                highscores.Controls.Add(rankLabel, 0, row);

                Label nameLabel = new Label();
                nameLabel.Text = data.Name;
                nameLabel.Font = new Font("Verdana", 12F, FontStyle.Bold);
                nameLabel.AutoSize = true;
                highscores.Controls.Add(nameLabel, 1, row);

                Label scoreLabel = new Label();
                scoreLabel.Text = data.Points.ToString();
                scoreLabel.Font = new Font("Verdana", 12F, FontStyle.Regular);
                scoreLabel.AutoSize = true;
                scoreLabel.TextAlign = ContentAlignment.MiddleRight;
                highscores.Controls.Add(scoreLabel, 2, row);

                durationToolTip.SetToolTip(nameLabel, data.Time);
                durationToolTip.SetToolTip(scoreLabel, data.Time);


                row++;
            }
            Application.DoEvents();

            stretchForm.Enabled = true;
            highscorePanel.Visible = true;
        }

        private void stretchForm_Tick(object sender, EventArgs e)
        {
            Height += 20;
            if (Height >= 485)
            {
                stretchForm.Enabled = false;

                opened = true;
                bButton.Text = "Close";
                bButton.Enabled = true;
            }
        }
    }
}
