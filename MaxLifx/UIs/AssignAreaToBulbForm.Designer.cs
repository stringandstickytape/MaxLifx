namespace MaxLifx
{
    partial class AssignAreaToBulbForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.lbBulbs = new System.Windows.Forms.ListBox();
            this.cbArea = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 139);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(260, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbBulbs
            // 
            this.lbBulbs.FormattingEnabled = true;
            this.lbBulbs.Location = new System.Drawing.Point(12, 11);
            this.lbBulbs.Name = "lbBulbs";
            this.lbBulbs.Size = new System.Drawing.Size(260, 95);
            this.lbBulbs.TabIndex = 1;
            this.lbBulbs.SelectedIndexChanged += new System.EventHandler(this.lbBulbs_SelectedIndexChanged);
            // 
            // cbArea
            // 
            this.cbArea.Enabled = false;
            this.cbArea.FormattingEnabled = true;
            this.cbArea.Location = new System.Drawing.Point(130, 112);
            this.cbArea.Name = "cbArea";
            this.cbArea.Size = new System.Drawing.Size(142, 21);
            this.cbArea.TabIndex = 2;
            this.cbArea.SelectedIndexChanged += new System.EventHandler(this.cbArea_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Area assigned to bulb:";
            // 
            // AssignAreaToBulbForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 173);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbArea);
            this.Controls.Add(this.lbBulbs);
            this.Controls.Add(this.button1);
            this.Name = "AssignAreaToBulbForm";
            this.Text = "AssignAreaToBulbForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lbBulbs;
        private System.Windows.Forms.ComboBox cbArea;
        private System.Windows.Forms.Label label1;
    }
}