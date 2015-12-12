using ColourControl;

namespace ColourControl
{
    partial class ColourControl
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
            this.hueSelector1 = new HueSelector();
            this.SuspendLayout();
            // 
            // hueSelector1
            // 
            this.hueSelector1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hueSelector1.Location = new System.Drawing.Point(0, 0);
            this.hueSelector1.Margin = new System.Windows.Forms.Padding(0);
            this.hueSelector1.Name = "hueSelector1";
            this.hueSelector1.Size = new System.Drawing.Size(150, 150);
            this.hueSelector1.TabIndex = 0;
            this.hueSelector1.Text = "hueSelector1";
            // 
            // ColourControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.hueSelector1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ColourControl";
            this.ResumeLayout(false);

        }

        #endregion

        private HueSelector hueSelector1;
    }
}
