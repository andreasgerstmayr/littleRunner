namespace littleRunner
{
    partial class About
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
            this.button1 = new System.Windows.Forms.Button();
            this.credits = new System.Windows.Forms.TextBox();
            this.lAbout = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(48, 245);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(268, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // credits
            // 
            this.credits.Font = new System.Drawing.Font("Bitstream Vera Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.credits.Location = new System.Drawing.Point(12, 80);
            this.credits.Multiline = true;
            this.credits.Name = "credits";
            this.credits.ReadOnly = true;
            this.credits.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.credits.Size = new System.Drawing.Size(345, 136);
            this.credits.TabIndex = 2;
            this.credits.Text = "- Game -\r\nProgrammer: Andreas G. (andihit)\r\n\r\n- Graphics -\r\nSecret Maryo Chronicl" +
                "es (GPL; http://www.secretmaryo.org)\r\nSupertux (GPL; http://supertux.lethargik.o" +
                "rg)";
            // 
            // lAbout
            // 
            this.lAbout.AutoSize = true;
            this.lAbout.Font = new System.Drawing.Font("Bitstream Vera Sans", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAbout.Location = new System.Drawing.Point(121, 19);
            this.lAbout.Name = "lAbout";
            this.lAbout.Size = new System.Drawing.Size(112, 38);
            this.lAbout.TabIndex = 3;
            this.lAbout.Text = "About";
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 280);
            this.Controls.Add(this.lAbout);
            this.Controls.Add(this.credits);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "About";
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox credits;
        private System.Windows.Forms.Label lAbout;
    }
}