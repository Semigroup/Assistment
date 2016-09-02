namespace Assistment.form
{
    partial class ChainedSizeFBox
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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.XBox = new Assistment.form.FloatBox();
            this.YBox = new Assistment.form.FloatBox();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(0, 24);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(82, 21);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Chained";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // XBox
            // 
            this.XBox.Location = new System.Drawing.Point(0, 0);
            this.XBox.Name = "XBox";
            this.XBox.Size = new System.Drawing.Size(51, 22);
            this.XBox.TabIndex = 2;
            this.XBox.UserValue = 0F;
            this.XBox.UserValueMaximum = 3.402823E+38F;
            this.XBox.UserValueMinimum = -3.402823E+38F;
            // 
            // YBox
            // 
            this.YBox.Location = new System.Drawing.Point(57, 0);
            this.YBox.Name = "YBox";
            this.YBox.Size = new System.Drawing.Size(51, 22);
            this.YBox.TabIndex = 3;
            this.YBox.UserValue = 0F;
            this.YBox.UserValueMaximum = 3.402823E+38F;
            this.YBox.UserValueMinimum = -3.402823E+38F;
            // 
            // ChainedSizeFBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.YBox);
            this.Controls.Add(this.XBox);
            this.Controls.Add(this.checkBox1);
            this.Name = "ChainedSizeFBox";
            this.Size = new System.Drawing.Size(114, 45);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1;
        private FloatBox XBox;
        private FloatBox YBox;
    }
}
