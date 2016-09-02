namespace Assistment.form
{
    partial class BallPointFBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BallPointFBox));
            this.label1 = new System.Windows.Forms.Label();
            this.pointFBox1 = new Assistment.form.PointFBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 200);
            this.label1.TabIndex = 2;
            this.label1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.label1_MouseClick);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label1_MouseClick);
            // 
            // pointFBox1
            // 
            this.pointFBox1.Location = new System.Drawing.Point(41, 200);
            this.pointFBox1.Name = "pointFBox1";
            this.pointFBox1.Size = new System.Drawing.Size(108, 22);
            this.pointFBox1.TabIndex = 1;
            this.pointFBox1.UserPoint = ((System.Drawing.PointF)(resources.GetObject("pointFBox1.UserPoint")));
            this.pointFBox1.UserSize = new System.Drawing.SizeF(0F, 0F);
            this.pointFBox1.UserX = 0F;
            this.pointFBox1.UserY = 0F;
            // 
            // BallPointFBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pointFBox1);
            this.Name = "BallPointFBox";
            this.Size = new System.Drawing.Size(201, 225);
            this.ResumeLayout(false);

        }

        #endregion

        private PointFBox pointFBox1;
        private System.Windows.Forms.Label label1;
    }
}
