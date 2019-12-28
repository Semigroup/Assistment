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
            this.label2 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.floatBox2 = new Assistment.form.FloatBox();
            this.floatBox1 = new Assistment.form.FloatBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.targetDinAFormatBox = new Assistment.form.IntBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(220, 160);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 33);
            this.button1.TabIndex = 0;
            this.button1.Text = "Speichern";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(220, 102);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(98, 17);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "In PDF packen";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.Change);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(218, 61);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Pixel pro Millimeter";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(220, 80);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(81, 17);
            this.checkBox2.TabIndex = 7;
            this.checkBox2.Text = "Hochformat";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.Change);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(218, 39);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "DPI";
            // 
            // floatBox2
            // 
            this.floatBox2.Location = new System.Drawing.Point(316, 35);
            this.floatBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.floatBox2.Name = "floatBox2";
            this.floatBox2.Size = new System.Drawing.Size(38, 18);
            this.floatBox2.TabIndex = 8;
            this.floatBox2.UserValue = 600F;
            this.floatBox2.UserValueMaximum = 100000F;
            this.floatBox2.UserValueMinimum = 1E-08F;
            this.floatBox2.UserValueChanged += new System.EventHandler(this.floatBox2_UserValueChanged);
            // 
            // floatBox1
            // 
            this.floatBox1.Location = new System.Drawing.Point(316, 57);
            this.floatBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.floatBox1.Name = "floatBox1";
            this.floatBox1.Size = new System.Drawing.Size(38, 18);
            this.floatBox1.TabIndex = 4;
            this.floatBox1.UserValue = 600F / 25.4F;
            this.floatBox1.UserValueMaximum = 100000F;
            this.floatBox1.UserValueMinimum = 1E-08F;
            this.floatBox1.UserValueChanged += new System.EventHandler(this.floatBox1_UserValueChanged);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(9, 344);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(352, 19);
            this.progressBar1.TabIndex = 10;
            // 
            // targetDinAFormatBox
            // 
            this.targetDinAFormatBox.Location = new System.Drawing.Point(316, 123);
            this.targetDinAFormatBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.targetDinAFormatBox.Name = "targetDinAFormatBox";
            this.targetDinAFormatBox.Size = new System.Drawing.Size(37, 16);
            this.targetDinAFormatBox.TabIndex = 11;
            this.targetDinAFormatBox.UserValue = 4;
            this.targetDinAFormatBox.UserValueMaximum = 10;
            this.targetDinAFormatBox.UserValueMinimum = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(217, 126);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Ziel DinA Format";
            // 
            // PDFDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 372);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.targetDinAFormatBox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.floatBox2);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.floatBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private IntBox targetDinAFormatBox;
        private System.Windows.Forms.Label label3;
    }
}