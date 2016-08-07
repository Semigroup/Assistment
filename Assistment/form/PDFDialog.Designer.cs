namespace Assistment.form
{
    partial class PDFDialog
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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.floatBox1 = new Assistment.form.FloatBox();
            this.label2 = new System.Windows.Forms.Label();
            this.floatBox2 = new Assistment.form.FloatBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(290, 153);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(183, 55);
            this.button1.TabIndex = 0;
            this.button1.Text = "Datei Erschaffen";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(294, 126);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(122, 21);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "In PDF packen";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.Change);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(291, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Pixel pro Millimeter";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(294, 99);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(103, 21);
            this.checkBox2.TabIndex = 7;
            this.checkBox2.Text = "Hochformat";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.Change);
            // 
            // floatBox1
            // 
            this.floatBox1.Location = new System.Drawing.Point(422, 70);
            this.floatBox1.Name = "floatBox1";
            this.floatBox1.Size = new System.Drawing.Size(51, 22);
            this.floatBox1.TabIndex = 4;
            this.floatBox1.UserValue = 10F;
            this.floatBox1.UserValueMaximum = 100000F;
            this.floatBox1.UserValueMinimum = 1E-08F;
            this.floatBox1.UserValueChanged += new System.EventHandler(this.floatBox1_UserValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(291, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "DPI";
            // 
            // floatBox2
            // 
            this.floatBox2.Location = new System.Drawing.Point(422, 43);
            this.floatBox2.Name = "floatBox2";
            this.floatBox2.Size = new System.Drawing.Size(51, 22);
            this.floatBox2.TabIndex = 8;
            this.floatBox2.UserValue = 254F;
            this.floatBox2.UserValueMaximum = 100000F;
            this.floatBox2.UserValueMinimum = 1E-08F;
            this.floatBox2.UserValueChanged += new System.EventHandler(this.floatBox2_UserValueChanged);
            // 
            // PDFDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 458);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.floatBox2);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.floatBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Name = "PDFDialog";
            this.Text = "PDFDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private FloatBox floatBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label2;
        private FloatBox floatBox2;
    }
}