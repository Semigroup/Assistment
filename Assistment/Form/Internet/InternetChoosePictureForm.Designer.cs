namespace Assistment.form.Internet
{
    partial class InternetChoosePictureForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InternetChoosePictureForm));
            this.SuchButton = new System.Windows.Forms.Button();
            this.SuchTextBox = new System.Windows.Forms.TextBox();
            this.scrollList1 = new Assistment.form.ScrollList();
            this.ppmBox1 = new Assistment.form.PpmBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pointFBox1 = new Assistment.form.PointFBox();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // SuchButton
            // 
            this.SuchButton.Location = new System.Drawing.Point(12, 12);
            this.SuchButton.Name = "SuchButton";
            this.SuchButton.Size = new System.Drawing.Size(75, 23);
            this.SuchButton.TabIndex = 0;
            this.SuchButton.Text = "Suchen";
            this.SuchButton.UseVisualStyleBackColor = true;
            this.SuchButton.Click += new System.EventHandler(this.SuchButton_Click);
            // 
            // SuchTextBox
            // 
            this.SuchTextBox.Location = new System.Drawing.Point(93, 14);
            this.SuchTextBox.Name = "SuchTextBox";
            this.SuchTextBox.Size = new System.Drawing.Size(321, 20);
            this.SuchTextBox.TabIndex = 1;
            this.SuchTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SuchTextBox_KeyPress);
            // 
            // scrollList1
            // 
            this.scrollList1.Location = new System.Drawing.Point(461, 77);
            this.scrollList1.Name = "scrollList1";
            this.scrollList1.Size = new System.Drawing.Size(320, 481);
            this.scrollList1.TabIndex = 2;
            // 
            // ppmBox1
            // 
            this.ppmBox1.Location = new System.Drawing.Point(638, 14);
            this.ppmBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ppmBox1.Name = "ppmBox1";
            this.ppmBox1.Ppm = 23.62205F;
            this.ppmBox1.PpmMaximum = 1000F;
            this.ppmBox1.PpmMinimum = 0.001F;
            this.ppmBox1.Size = new System.Drawing.Size(133, 36);
            this.ppmBox1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(519, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Nach Auflösung filtern:";
            // 
            // pointFBox1
            // 
            this.pointFBox1.Location = new System.Drawing.Point(638, 54);
            this.pointFBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pointFBox1.Name = "pointFBox1";
            this.pointFBox1.Size = new System.Drawing.Size(81, 18);
            this.pointFBox1.TabIndex = 5;
            this.pointFBox1.UserPoint = ((System.Drawing.PointF)(resources.GetObject("pointFBox1.UserPoint")));
            this.pointFBox1.UserSize = new System.Drawing.SizeF(10F, 10F);
            this.pointFBox1.UserX = 10F;
            this.pointFBox1.UserY = 10F;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(519, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Größe in mm:";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(50, 40);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(89, 17);
            this.radioButton1.TabIndex = 7;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Große Bilder?";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(145, 40);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(112, 17);
            this.radioButton2.TabIndex = 8;
            this.radioButton2.Text = "Mittelgroße Bilder?";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(263, 40);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(77, 17);
            this.radioButton3.TabIndex = 9;
            this.radioButton3.Text = "Alle Bilder?";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // InternetChoosePictureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 559);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pointFBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ppmBox1);
            this.Controls.Add(this.scrollList1);
            this.Controls.Add(this.SuchTextBox);
            this.Controls.Add(this.SuchButton);
            this.Name = "InternetChoosePictureForm";
            this.Text = "Mittelgroße Bilder?";
            this.SizeChanged += new System.EventHandler(this.InternetChoosePictureForm_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SuchButton;
        private System.Windows.Forms.TextBox SuchTextBox;
        private ScrollList scrollList1;
        private PpmBox ppmBox1;
        private System.Windows.Forms.Label label1;
        private PointFBox pointFBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
    }
}