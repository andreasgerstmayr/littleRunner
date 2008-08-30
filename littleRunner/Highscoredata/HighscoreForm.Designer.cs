namespace littleRunner.Highscoredata
{
    partial class HighscoreForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HighscoreForm));
            this.lName = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.lScore = new System.Windows.Forms.Label();
            this.score = new System.Windows.Forms.Label();
            this.bButton = new System.Windows.Forms.Button();
            this.lHighscores = new System.Windows.Forms.Label();
            this.stretchForm = new System.Windows.Forms.Timer(this.components);
            this.highscores = new System.Windows.Forms.TableLayoutPanel();
            this.errProv = new System.Windows.Forms.ErrorProvider(this.components);
            this.lTime = new System.Windows.Forms.Label();
            this.time = new System.Windows.Forms.Label();
            this.durationToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.highscorePanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.errProv)).BeginInit();
            this.highscorePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lName
            // 
            this.lName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lName.AutoSize = true;
            this.lName.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lName.Location = new System.Drawing.Point(62, 85);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(84, 16);
            this.lName.TabIndex = 0;
            this.lName.Text = "Your name:";
            // 
            // name
            // 
            this.name.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.name.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name.Location = new System.Drawing.Point(166, 85);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(115, 23);
            this.name.TabIndex = 1;
            // 
            // lScore
            // 
            this.lScore.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lScore.AutoSize = true;
            this.lScore.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lScore.Location = new System.Drawing.Point(61, 116);
            this.lScore.Name = "lScore";
            this.lScore.Size = new System.Drawing.Size(85, 16);
            this.lScore.TabIndex = 2;
            this.lScore.Text = "Your score:";
            // 
            // score
            // 
            this.score.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.score.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.score.Location = new System.Drawing.Point(166, 116);
            this.score.Name = "score";
            this.score.Size = new System.Drawing.Size(115, 20);
            this.score.TabIndex = 3;
            this.score.Text = "0";
            this.score.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bButton
            // 
            this.bButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.bButton.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bButton.Location = new System.Drawing.Point(104, 198);
            this.bButton.Name = "bButton";
            this.bButton.Size = new System.Drawing.Size(135, 29);
            this.bButton.TabIndex = 4;
            this.bButton.Text = "Save";
            this.bButton.UseVisualStyleBackColor = true;
            this.bButton.Click += new System.EventHandler(this.bSave_Click);
            // 
            // lHighscores
            // 
            this.lHighscores.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lHighscores.AutoSize = true;
            this.lHighscores.Font = new System.Drawing.Font("Bitstream Vera Sans", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lHighscores.Location = new System.Drawing.Point(89, 20);
            this.lHighscores.Name = "lHighscores";
            this.lHighscores.Size = new System.Drawing.Size(164, 31);
            this.lHighscores.TabIndex = 5;
            this.lHighscores.Text = "Highscores";
            // 
            // stretchForm
            // 
            this.stretchForm.Interval = 50;
            this.stretchForm.Tick += new System.EventHandler(this.stretchForm_Tick);
            // 
            // highscores
            // 
            this.highscores.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.highscores.AutoSize = true;
            this.highscores.BackColor = System.Drawing.SystemColors.Control;
            this.highscores.ColumnCount = 3;
            this.highscores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.highscores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.highscores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.highscores.Location = new System.Drawing.Point(7, 3);
            this.highscores.Name = "highscores";
            this.highscores.RowCount = 1;
            this.highscores.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.highscores.Size = new System.Drawing.Size(291, 100);
            this.highscores.TabIndex = 6;
            // 
            // errProv
            // 
            this.errProv.ContainerControl = this;
            // 
            // lTime
            // 
            this.lTime.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lTime.AutoSize = true;
            this.lTime.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTime.Location = new System.Drawing.Point(61, 147);
            this.lTime.Name = "lTime";
            this.lTime.Size = new System.Drawing.Size(77, 16);
            this.lTime.TabIndex = 7;
            this.lTime.Text = "Your time:";
            // 
            // time
            // 
            this.time.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.time.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.time.Location = new System.Drawing.Point(166, 143);
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(115, 20);
            this.time.TabIndex = 8;
            this.time.Text = "00m 00s";
            this.time.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // highscorePanel
            // 
            this.highscorePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.highscorePanel.AutoScroll = true;
            this.highscorePanel.Controls.Add(this.highscores);
            this.highscorePanel.Location = new System.Drawing.Point(12, 248);
            this.highscorePanel.Name = "highscorePanel";
            this.highscorePanel.Size = new System.Drawing.Size(320, 5);
            this.highscorePanel.TabIndex = 9;
            this.highscorePanel.Visible = false;
            // 
            // HighscoreForm
            // 
            this.AcceptButton = this.bButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 268);
            this.Controls.Add(this.highscorePanel);
            this.Controls.Add(this.time);
            this.Controls.Add(this.lTime);
            this.Controls.Add(this.lHighscores);
            this.Controls.Add(this.bButton);
            this.Controls.Add(this.score);
            this.Controls.Add(this.lScore);
            this.Controls.Add(this.name);
            this.Controls.Add(this.lName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HighscoreForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Highscore";
            ((System.ComponentModel.ISupportInitialize)(this.errProv)).EndInit();
            this.highscorePanel.ResumeLayout(false);
            this.highscorePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label lScore;
        private System.Windows.Forms.Label score;
        private System.Windows.Forms.Button bButton;
        private System.Windows.Forms.Label lHighscores;
        private System.Windows.Forms.Timer stretchForm;
        private System.Windows.Forms.TableLayoutPanel highscores;
        private System.Windows.Forms.ErrorProvider errProv;
        private System.Windows.Forms.Label time;
        private System.Windows.Forms.Label lTime;
        private System.Windows.Forms.ToolTip durationToolTip;
        private System.Windows.Forms.Panel highscorePanel;
    }
}