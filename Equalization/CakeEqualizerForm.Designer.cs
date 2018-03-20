namespace Equalization
{
    partial class CakeEqualizerForm
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
            this.equalizerControl1 = new EqualizerControl();
            this.SuspendLayout();
            // 
            // equalizerControl1
            // 
            this.equalizerControl1.Location = new System.Drawing.Point(12, 12);
            this.equalizerControl1.Name = "equalizerControl1";
            this.equalizerControl1.Size = new System.Drawing.Size(789, 235);
            this.equalizerControl1.TabIndex = 0;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 250);
            this.Controls.Add(this.equalizerControl1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
        }

        #endregion

        public EqualizerControl equalizerControl1;
    }
}