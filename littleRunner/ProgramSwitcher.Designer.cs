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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgramSwitcher));
            this.startgame = new System.Windows.Forms.Button();
            this.starteditor = new System.Windows.Forms.Button();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayIconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.startGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closelittleRunner = new System.Windows.Forms.Button();
            this.trayIconMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // startgame
            // 
            this.startgame.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startgame.Location = new System.Drawing.Point(63, 32);
            this.startgame.Name = "startgame";
            this.startgame.Size = new System.Drawing.Size(132, 37);
            this.startgame.TabIndex = 0;
            this.startgame.Text = "Game";
            this.startgame.UseVisualStyleBackColor = true;
            this.startgame.Click += new System.EventHandler(this.startgame_Click);
            // 
            // starteditor
            // 
            this.starteditor.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starteditor.Location = new System.Drawing.Point(218, 32);
            this.starteditor.Name = "starteditor";
            this.starteditor.Size = new System.Drawing.Size(134, 37);
            this.starteditor.TabIndex = 1;
            this.starteditor.Text = "Editor";
            this.starteditor.UseVisualStyleBackColor = true;
            this.starteditor.Click += new System.EventHandler(this.starteditor_Click);
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayIconMenu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "littleRunner";
            this.trayIcon.Visible = true;
            this.trayIcon.DoubleClick += new System.EventHandler(this.trayIcon_DoubleClick);
            // 
            // trayIconMenu
            // 
            this.trayIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startGameToolStripMenuItem,
            this.startEditorToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.trayIconMenu.Name = "trayIconMenu";
            this.trayIconMenu.Size = new System.Drawing.Size(141, 114);
            // 
            // startGameToolStripMenuItem
            // 
            this.startGameToolStripMenuItem.Name = "startGameToolStripMenuItem";
            this.startGameToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.startGameToolStripMenuItem.Text = "Start game";
            this.startGameToolStripMenuItem.Click += new System.EventHandler(this.startGameToolStripMenuItem_Click);
            // 
            // startEditorToolStripMenuItem
            // 
            this.startEditorToolStripMenuItem.Name = "startEditorToolStripMenuItem";
            this.startEditorToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.startEditorToolStripMenuItem.Text = "Start editor";
            this.startEditorToolStripMenuItem.Click += new System.EventHandler(this.startEditorToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // closelittleRunner
            // 
            this.closelittleRunner.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closelittleRunner.Location = new System.Drawing.Point(141, 103);
            this.closelittleRunner.Name = "closelittleRunner";
            this.closelittleRunner.Size = new System.Drawing.Size(132, 36);
            this.closelittleRunner.TabIndex = 2;
            this.closelittleRunner.Text = "Close";
            this.closelittleRunner.UseVisualStyleBackColor = true;
            this.closelittleRunner.Click += new System.EventHandler(this.closelittleRunner_Click);
            // 
            // ProgramSwitcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 151);
            this.Controls.Add(this.starteditor);
            this.Controls.Add(this.startgame);
            this.Controls.Add(this.closelittleRunner);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProgramSwitcher";
            this.Text = "littleRunner - Start program";
            this.SizeChanged += new System.EventHandler(this.ProgramSwitcher_SizeChanged);
            this.Shown += new System.EventHandler(this.ProgramSwitcher_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProgramSwitcher_FormClosing);
            this.trayIconMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button startgame;
        private System.Windows.Forms.Button starteditor;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip trayIconMenu;
        private System.Windows.Forms.ToolStripMenuItem startGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Button closelittleRunner;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}