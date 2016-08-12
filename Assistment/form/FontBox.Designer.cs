namespace Assistment.form
{
    partial class FontBox
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.Schriftart = new System.Windows.Forms.Button();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.SuspendLayout();
            // 
            // Schriftart
            // 
            this.Schriftart.Location = new System.Drawing.Point(0, 0);
            this.Schriftart.Name = "Schriftart";
            this.Schriftart.Size = new System.Drawing.Size(178, 28);
            this.Schriftart.TabIndex = 0;
            this.Schriftart.Text = "Schriftart Auswählen";
            this.Schriftart.UseVisualStyleBackColor = true;
            this.Schriftart.Click += new System.EventHandler(this.Schriftart_Click);
            // 
            // FontBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Schriftart);
            this.Name = "FontBox";
            this.Size = new System.Drawing.Size(179, 29);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Schriftart;
        private System.Windows.Forms.FontDialog fontDialog1;
    }
}
