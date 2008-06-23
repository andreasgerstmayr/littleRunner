namespace littleRunner
{
    partial class ProgramSwitcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgramSwitcher));
            this.startgame = new System.Windows.Forms.Button();
            this.starteditor = new System.Windows.Forms.Button();
            this.closelittleRunner = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.homepage = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // startgame
            // 
            this.startgame.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.startgame.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.startgame.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.startgame.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.startgame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startgame.Font = new System.Drawing.Font("Bitstream Vera Sans", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startgame.Location = new System.Drawing.Point(176, 353);
            this.startgame.Name = "startgame";
            this.startgame.Size = new System.Drawing.Size(212, 62);
            this.startgame.TabIndex = 0;
            this.startgame.Text = "Game";
            this.startgame.UseVisualStyleBackColor = true;
            this.startgame.Click += new System.EventHandler(this.startgame_Click);
            // 
            // starteditor
            // 
            this.starteditor.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.starteditor.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.starteditor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.starteditor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.starteditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.starteditor.Font = new System.Drawing.Font("Bitstream Vera Sans", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starteditor.Location = new System.Drawing.Point(406, 353);
            this.starteditor.Name = "starteditor";
            this.starteditor.Size = new System.Drawing.Size(212, 62);
            this.starteditor.TabIndex = 1;
            this.starteditor.Text = "Editor";
            this.starteditor.UseVisualStyleBackColor = true;
            this.starteditor.Click += new System.EventHandler(this.starteditor_Click);
            // 
            // closelittleRunner
            // 
            this.closelittleRunner.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.closelittleRunner.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closelittleRunner.Font = new System.Drawing.Font("Bitstream Vera Sans", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closelittleRunner.Location = new System.Drawing.Point(204, 478);
            this.closelittleRunner.Name = "closelittleRunner";
            this.closelittleRunner.Size = new System.Drawing.Size(387, 41);
            this.closelittleRunner.TabIndex = 2;
            this.closelittleRunner.Text = "Close";
            this.closelittleRunner.UseVisualStyleBackColor = true;
            this.closelittleRunner.Click += new System.EventHandler(this.closelittleRunner_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.Image = global::littleRunner.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(147, 59);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(500, 150);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // homepage
            // 
            this.homepage.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.homepage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.homepage.AutoSize = true;
            this.homepage.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.homepage.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.homepage.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.homepage.Location = new System.Drawing.Point(581, 565);
            this.homepage.Name = "homepage";
            this.homepage.Size = new System.Drawing.Size(201, 16);
            this.homepage.TabIndex = 4;
            this.homepage.TabStop = true;
            this.homepage.Text = "http://littlerunner.andihit.net";
            this.homepage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.homepage_LinkClicked);
            // 
            // ProgramSwitcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.ClientSize = new System.Drawing.Size(794, 590);
            this.Controls.Add(this.homepage);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.starteditor);
            this.Controls.Add(this.closelittleRunner);
            this.Controls.Add(this.startgame);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProgramSwitcher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "littleRunner";
            this.SizeChanged += new System.EventHandler(this.ProgramSwitcher_SizeChanged);
            this.Shown += new System.EventHandler(this.ProgramSwitcher_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startgame;
        private System.Windows.Forms.Button starteditor;
        private System.Windows.Forms.Button closelittleRunner;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel homepage;
    }
}