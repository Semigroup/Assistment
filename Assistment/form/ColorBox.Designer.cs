namespace Assistment.form
{
    partial class ColorBox
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
            this.BlueBox = new Assistment.form.IntBox();
            this.GreenBox = new Assistment.form.IntBox();
            this.RedBox = new Assistment.form.IntBox();
            this.AlphaBox = new Assistment.form.IntBox();
            this.SuspendLayout();
            // 
            // BlueBox
            // 
            this.BlueBox.Location = new System.Drawing.Point(165, 0);
            this.BlueBox.Name = "BlueBox";
            this.BlueBox.Size = new System.Drawing.Size(49, 20);
            this.BlueBox.TabIndex = 3;
            this.BlueBox.UserValueMaximum = 255;
            this.BlueBox.UserValueMinimum = 0;
            // 
            // GreenBox
            // 
            this.GreenBox.Location = new System.Drawing.Point(110, 0);
            this.GreenBox.Name = "GreenBox";
            this.GreenBox.Size = new System.Drawing.Size(49, 20);
            this.GreenBox.TabIndex = 2;
            this.GreenBox.UserValueMaximum = 255;
            this.GreenBox.UserValueMinimum = 0;
            // 
            // RedBox
            // 
            this.RedBox.Location = new System.Drawing.Point(55, 0);
            this.RedBox.Name = "RedBox";
            this.RedBox.Size = new System.Drawing.Size(49, 20);
            this.RedBox.TabIndex = 1;
            this.RedBox.UserValueMaximum = 255;
            this.RedBox.UserValueMinimum = 0;
            // 
            // AlphaBox
            // 
            this.AlphaBox.Location = new System.Drawing.Point(0, 0);
            this.AlphaBox.Name = "AlphaBox";
            this.AlphaBox.Size = new System.Drawing.Size(49, 20);
            this.AlphaBox.TabIndex = 0;
            this.AlphaBox.UserValueMaximum = 255;
            this.AlphaBox.UserValueMinimum = 0;
            // 
            // ColorBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BlueBox);
            this.Controls.Add(this.GreenBox);
            this.Controls.Add(this.RedBox);
            this.Controls.Add(this.AlphaBox);
            this.Name = "ColorBox";
            this.Size = new System.Drawing.Size(217, 23);
            this.ResumeLayout(false);

        }

        #endregion

        private IntBox AlphaBox;
        private IntBox RedBox;
        private IntBox GreenBox;
        private IntBox BlueBox;

    }
}
