namespace Assistment.form
{
    partial class RectangleFBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RectangleFBox));
            this.locationBox = new Assistment.form.PointFBox();
            this.sizeBox = new Assistment.form.PointFBox();
            this.LabelLocation = new System.Windows.Forms.Label();
            this.LabelSize = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // locationBox
            // 
            this.locationBox.Location = new System.Drawing.Point(52, 0);
            this.locationBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.locationBox.Name = "locationBox";
            this.locationBox.Size = new System.Drawing.Size(81, 18);
            this.locationBox.TabIndex = 0;
            this.locationBox.UserPoint = ((System.Drawing.PointF)(resources.GetObject("locationBox.UserPoint")));
            this.locationBox.UserSize = new System.Drawing.SizeF(0F, 0F);
            this.locationBox.UserX = 0F;
            this.locationBox.UserY = 0F;
            // 
            // sizeBox
            // 
            this.sizeBox.Location = new System.Drawing.Point(52, 22);
            this.sizeBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.sizeBox.Name = "sizeBox";
            this.sizeBox.Size = new System.Drawing.Size(81, 18);
            this.sizeBox.TabIndex = 1;
            this.sizeBox.UserPoint = ((System.Drawing.PointF)(resources.GetObject("sizeBox.UserPoint")));
            this.sizeBox.UserSize = new System.Drawing.SizeF(0F, 0F);
            this.sizeBox.UserX = 0F;
            this.sizeBox.UserY = 0F;
            // 
            // LabelLocation
            // 
            this.LabelLocation.AutoSize = true;
            this.LabelLocation.Location = new System.Drawing.Point(3, 5);
            this.LabelLocation.Name = "LabelLocation";
            this.LabelLocation.Size = new System.Drawing.Size(44, 13);
            this.LabelLocation.TabIndex = 2;
            this.LabelLocation.Text = "Position";
            // 
            // LabelSize
            // 
            this.LabelSize.AutoSize = true;
            this.LabelSize.Location = new System.Drawing.Point(3, 22);
            this.LabelSize.Name = "LabelSize";
            this.LabelSize.Size = new System.Drawing.Size(36, 13);
            this.LabelSize.TabIndex = 3;
            this.LabelSize.Text = "Größe";
            // 
            // RectangleFBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LabelSize);
            this.Controls.Add(this.LabelLocation);
            this.Controls.Add(this.sizeBox);
            this.Controls.Add(this.locationBox);
            this.Name = "RectangleFBox";
            this.Size = new System.Drawing.Size(134, 42);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PointFBox locationBox;
        private PointFBox sizeBox;
        private System.Windows.Forms.Label LabelLocation;
        private System.Windows.Forms.Label LabelSize;
    }
}
