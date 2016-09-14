namespace Assistment.form
{
    partial class PpmBox
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
            this.floatBoxppm = new Assistment.form.FloatBox();
            this.floatBoxDpI = new Assistment.form.FloatBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // floatBoxppm
            // 
            this.floatBoxppm.Location = new System.Drawing.Point(0, 0);
            this.floatBoxppm.Name = "floatBoxppm";
            this.floatBoxppm.Size = new System.Drawing.Size(51, 22);
            this.floatBoxppm.TabIndex = 0;
            this.floatBoxppm.UserValue = 11.81102F;
            this.floatBoxppm.UserValueMaximum = 1000F;
            this.floatBoxppm.UserValueMinimum = 0.001F;
            // 
            // floatBoxDpI
            // 
            this.floatBoxDpI.Location = new System.Drawing.Point(0, 22);
            this.floatBoxDpI.Name = "floatBoxDpI";
            this.floatBoxDpI.Size = new System.Drawing.Size(51, 22);
            this.floatBoxDpI.TabIndex = 1;
            this.floatBoxDpI.UserValue = 300F;
            this.floatBoxDpI.UserValueMaximum = 10000F;
            this.floatBoxDpI.UserValueMinimum = 0.001F;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Pixel pro Millimeter";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Dots per Inch";
            // 
            // PpmBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.floatBoxDpI);
            this.Controls.Add(this.floatBoxppm);
            this.Name = "PpmBox";
            this.Size = new System.Drawing.Size(177, 44);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FloatBox floatBoxppm;
        private FloatBox floatBoxDpI;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
