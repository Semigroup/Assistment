namespace Assistment.form.Internet
{
    partial class InternetImageResult
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.Ansehen = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(239, 200);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(0, 206);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "Speichern";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(81, 206);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(75, 23);
            this.RemoveButton.TabIndex = 2;
            this.RemoveButton.Text = "Entfernen";
            this.RemoveButton.UseVisualStyleBackColor = true;
            // 
            // Ansehen
            // 
            this.Ansehen.Location = new System.Drawing.Point(161, 206);
            this.Ansehen.Name = "Ansehen";
            this.Ansehen.Size = new System.Drawing.Size(75, 23);
            this.Ansehen.TabIndex = 3;
            this.Ansehen.Text = "Ansehen";
            this.Ansehen.UseVisualStyleBackColor = true;
            // 
            // InternetImageResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Ansehen);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.pictureBox1);
            this.Name = "InternetImageResult";
            this.Size = new System.Drawing.Size(239, 236);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button Ansehen;
    }
}
