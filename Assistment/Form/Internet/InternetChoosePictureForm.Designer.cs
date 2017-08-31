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
            this.SuchButton = new System.Windows.Forms.Button();
            this.SuchTextBox = new System.Windows.Forms.TextBox();
            this.scrollList1 = new Assistment.form.ScrollList();
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
            this.SuchTextBox.Size = new System.Drawing.Size(677, 20);
            this.SuchTextBox.TabIndex = 1;
            this.SuchTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SuchTextBox_KeyPress);
            // 
            // scrollList1
            // 
            this.scrollList1.Location = new System.Drawing.Point(12, 40);
            this.scrollList1.Name = "scrollList1";
            this.scrollList1.Size = new System.Drawing.Size(758, 518);
            this.scrollList1.TabIndex = 2;
            // 
            // InternetChoosePictureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 559);
            this.Controls.Add(this.scrollList1);
            this.Controls.Add(this.SuchTextBox);
            this.Controls.Add(this.SuchButton);
            this.Name = "InternetChoosePictureForm";
            this.Text = "InternetChoosePictureForm";
            this.SizeChanged += new System.EventHandler(this.InternetChoosePictureForm_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SuchButton;
        private System.Windows.Forms.TextBox SuchTextBox;
        private ScrollList scrollList1;
    }
}