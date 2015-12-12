namespace MaxLifx.UIs
{
    partial class SoundGeneratorUI
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
            this.pLoops = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbOffTimes = new System.Windows.Forms.TextBox();
            this.tbOnTimes = new System.Windows.Forms.TextBox();
            this.cbConfigs = new System.Windows.Forms.ComboBox();
            this.bSave = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pRandoms = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pLoops
            // 
            this.pLoops.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pLoops.AutoScroll = true;
            this.pLoops.Location = new System.Drawing.Point(0, 0);
            this.pLoops.Name = "pLoops";
            this.pLoops.Size = new System.Drawing.Size(794, 404);
            this.pLoops.TabIndex = 99;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 111;
            this.label2.Text = "Stop Times";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 110;
            this.label1.Text = "Start Times";
            // 
            // tbOffTimes
            // 
            this.tbOffTimes.Location = new System.Drawing.Point(85, 78);
            this.tbOffTimes.Name = "tbOffTimes";
            this.tbOffTimes.Size = new System.Drawing.Size(367, 20);
            this.tbOffTimes.TabIndex = 109;
            this.tbOffTimes.TextChanged += new System.EventHandler(this.tbOffTimes_TextChanged);
            // 
            // tbOnTimes
            // 
            this.tbOnTimes.Location = new System.Drawing.Point(85, 52);
            this.tbOnTimes.Name = "tbOnTimes";
            this.tbOnTimes.Size = new System.Drawing.Size(367, 20);
            this.tbOnTimes.TabIndex = 108;
            this.tbOnTimes.TextChanged += new System.EventHandler(this.tbOnTimes_TextChanged);
            // 
            // cbConfigs
            // 
            this.cbConfigs.FormattingEnabled = true;
            this.cbConfigs.Location = new System.Drawing.Point(12, 12);
            this.cbConfigs.Name = "cbConfigs";
            this.cbConfigs.Size = new System.Drawing.Size(224, 21);
            this.cbConfigs.TabIndex = 100;
            this.cbConfigs.SelectedIndexChanged += new System.EventHandler(this.cbConfigs_SelectedIndexChanged);
            // 
            // bSave
            // 
            this.bSave.Location = new System.Drawing.Point(242, 10);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(75, 23);
            this.bSave.TabIndex = 99;
            this.bSave.Text = "Save As";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 104);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(802, 430);
            this.tabControl1.TabIndex = 112;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.pLoops);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(794, 404);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Looping";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.pRandoms);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(794, 404);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Random";
            // 
            // pRandoms
            // 
            this.pRandoms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pRandoms.AutoScroll = true;
            this.pRandoms.Location = new System.Drawing.Point(0, 2);
            this.pRandoms.Name = "pRandoms";
            this.pRandoms.Size = new System.Drawing.Size(794, 404);
            this.pRandoms.TabIndex = 100;
            // 
            // SoundGeneratorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 546);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbOffTimes);
            this.Controls.Add(this.tbOnTimes);
            this.Controls.Add(this.cbConfigs);
            this.Controls.Add(this.bSave);
            this.Name = "SoundGeneratorUI";
            this.Text = "SoundGeneratorUI";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.ComboBox cbConfigs;
        private System.Windows.Forms.Panel pLoops;
        private System.Windows.Forms.TextBox tbOnTimes;
        private System.Windows.Forms.TextBox tbOffTimes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel pRandoms;
    }
}