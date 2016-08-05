namespace Assistment.form
{
    partial class PointFBox
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
            this.floatBox1 = new Assistment.form.FloatBox();
            this.floatBox2 = new Assistment.form.FloatBox();
            this.SuspendLayout();
            // 
            // floatBox1
            // 
            this.floatBox1.Location = new System.Drawing.Point(0, 0);
            this.floatBox1.Name = "floatBox1";
            this.floatBox1.Size = new System.Drawing.Size(51, 22);
            this.floatBox1.TabIndex = 0;
            this.floatBox1.UserValue = 0F;
            // 
            // floatBox2
            // 
            this.floatBox2.Location = new System.Drawing.Point(57, 0);
            this.floatBox2.Name = "floatBox2";
            this.floatBox2.Size = new System.Drawing.Size(51, 22);
            this.floatBox2.TabIndex = 1;
            this.floatBox2.UserValue = 0F;
            // 
            // PointFBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.floatBox2);
            this.Controls.Add(this.floatBox1);
            this.Name = "PointFBox";
            this.Size = new System.Drawing.Size(108, 22);
            this.ResumeLayout(false);

        }

        #endregion

        private FloatBox floatBox1;
        private FloatBox floatBox2;
    }
}
