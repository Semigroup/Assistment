namespace Assistment.form
{
    partial class PointBox
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
            this.intBox1 = new Assistment.form.IntBox();
            this.intBox2 = new Assistment.form.IntBox();
            this.SuspendLayout();
            // 
            // intBox1
            // 
            this.intBox1.Location = new System.Drawing.Point(0, 0);
            this.intBox1.Name = "intBox1";
            this.intBox1.Size = new System.Drawing.Size(49, 20);
            this.intBox1.TabIndex = 0;
            this.intBox1.UserValueMaximum = 2147483647;
            this.intBox1.UserValueMinimum = -2147483648;
            // 
            // intBox2
            // 
            this.intBox2.Location = new System.Drawing.Point(51, 0);
            this.intBox2.Name = "intBox2";
            this.intBox2.Size = new System.Drawing.Size(49, 20);
            this.intBox2.TabIndex = 1;
            this.intBox2.UserValueMaximum = 2147483647;
            this.intBox2.UserValueMinimum = -2147483648;
            // 
            // PointBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.intBox2);
            this.Controls.Add(this.intBox1);
            this.Name = "PointBox";
            this.Size = new System.Drawing.Size(100, 23);
            this.ResumeLayout(false);

        }

        #endregion

        private IntBox intBox1;
        private IntBox intBox2;


    }
}
