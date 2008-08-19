using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace littleRunner.Highscoredata
{
    public partial class HighscoreForm : Form
    {
        bool opened;

        public HighscoreForm(int score)
        {
            InitializeComponent();
            opened = false;

            this.score.Text = score.ToString();
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

            Highscore.Write(name.Text, Convert.ToInt32(score.Text));
            name.Enabled = false;
            bButton.Enabled = false;

            // load highscores in panel
            highscores.RowStyles.Clear();
            int row = 0;
            foreach (Highscore.Data data in Highscore.ReadTop10())
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

                row++;
            }

            stretchForm.Enabled = true;
        }

        private void stretchForm_Tick(object sender, EventArgs e)
        {
            Height += 20;
            if (Height > 495)
            {
                stretchForm.Enabled = false;

                opened = true;
                bButton.Text = "Close";
                bButton.Enabled = true;
            }
        }
    }
}
