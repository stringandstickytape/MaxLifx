namespace MaxLifx.UIs
{
    partial class ScreenColourUI
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
            this.button5 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tlx = new System.Windows.Forms.TextBox();
            this.tly = new System.Windows.Forms.TextBox();
            this.brx = new System.Windows.Forms.TextBox();
            this.bry = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.kelvin = new System.Windows.Forms.Label();
            this.tbKelvin = new System.Windows.Forms.TextBox();
            this.tbBrightnessMin = new System.Windows.Forms.TextBox();
            this.tbSaturationMin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.delay = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.fade = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.brightness = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.saturation = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.lbLabels = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnMonitor1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tbMonitor = new System.Windows.Forms.TextBox();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(10, 118);
            this.button5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(322, 35);
            this.button5.TabIndex = 23;
            this.button5.Text = "Set Screen Area";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 171);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 20);
            this.label1.TabIndex = 21;
            this.label1.Text = "Top-Left";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 203);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 20);
            this.label2.TabIndex = 22;
            this.label2.Text = "Bottom-Right";
            // 
            // tlx
            // 
            this.tlx.Location = new System.Drawing.Point(158, 166);
            this.tlx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tlx.Name = "tlx";
            this.tlx.Size = new System.Drawing.Size(54, 26);
            this.tlx.TabIndex = 17;
            this.tlx.Text = "0";
            this.tlx.TextChanged += new System.EventHandler(this.pos_TextChanged);
            // 
            // tly
            // 
            this.tly.Location = new System.Drawing.Point(216, 166);
            this.tly.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tly.Name = "tly";
            this.tly.Size = new System.Drawing.Size(54, 26);
            this.tly.TabIndex = 18;
            this.tly.Text = "0";
            this.tly.TextChanged += new System.EventHandler(this.pos_TextChanged);
            // 
            // brx
            // 
            this.brx.Location = new System.Drawing.Point(158, 198);
            this.brx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.brx.Name = "brx";
            this.brx.Size = new System.Drawing.Size(54, 26);
            this.brx.TabIndex = 19;
            this.brx.Text = "0";
            this.brx.TextChanged += new System.EventHandler(this.pos_TextChanged);
            // 
            // bry
            // 
            this.bry.Location = new System.Drawing.Point(216, 198);
            this.bry.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bry.Name = "bry";
            this.bry.Size = new System.Drawing.Size(54, 26);
            this.bry.TabIndex = 20;
            this.bry.Text = "0";
            this.bry.TextChanged += new System.EventHandler(this.pos_TextChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.kelvin);
            this.groupBox4.Controls.Add(this.tbKelvin);
            this.groupBox4.Controls.Add(this.tbBrightnessMin);
            this.groupBox4.Controls.Add(this.tbSaturationMin);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.delay);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.fade);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.brightness);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.saturation);
            this.groupBox4.Location = new System.Drawing.Point(26, 700);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Size = new System.Drawing.Size(342, 198);
            this.groupBox4.TabIndex = 33;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Parameters";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(174, 163);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 20);
            this.label7.TabIndex = 105;
            this.label7.Text = "[2500-9000]";
            // 
            // kelvin
            // 
            this.kelvin.AutoSize = true;
            this.kelvin.Location = new System.Drawing.Point(6, 163);
            this.kelvin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.kelvin.Name = "kelvin";
            this.kelvin.Size = new System.Drawing.Size(50, 20);
            this.kelvin.TabIndex = 104;
            this.kelvin.Text = "Kelvin";
            // 
            // tbKelvin
            // 
            this.tbKelvin.Location = new System.Drawing.Point(118, 158);
            this.tbKelvin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbKelvin.Name = "tbKelvin";
            this.tbKelvin.Size = new System.Drawing.Size(54, 26);
            this.tbKelvin.TabIndex = 32;
            this.tbKelvin.Text = "3500";
            this.tbKelvin.TextChanged += new System.EventHandler(this.pos_TextChanged);
            // 
            // tbBrightnessMin
            // 
            this.tbBrightnessMin.Location = new System.Drawing.Point(118, 91);
            this.tbBrightnessMin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbBrightnessMin.Name = "tbBrightnessMin";
            this.tbBrightnessMin.Size = new System.Drawing.Size(54, 26);
            this.tbBrightnessMin.TabIndex = 30;
            this.tbBrightnessMin.Text = "0";
            this.tbBrightnessMin.TextChanged += new System.EventHandler(this.pos_TextChanged);
            // 
            // tbSaturationMin
            // 
            this.tbSaturationMin.Location = new System.Drawing.Point(118, 125);
            this.tbSaturationMin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbSaturationMin.Name = "tbSaturationMin";
            this.tbSaturationMin.Size = new System.Drawing.Size(54, 26);
            this.tbSaturationMin.TabIndex = 31;
            this.tbSaturationMin.Text = "0";
            this.tbSaturationMin.TextChanged += new System.EventHandler(this.pos_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(174, 29);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 20);
            this.label3.TabIndex = 29;
            this.label3.Text = "ms";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 63);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 20);
            this.label6.TabIndex = 19;
            this.label6.Text = "Delay";
            // 
            // delay
            // 
            this.delay.Location = new System.Drawing.Point(118, 58);
            this.delay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.delay.Name = "delay";
            this.delay.Size = new System.Drawing.Size(54, 26);
            this.delay.TabIndex = 20;
            this.delay.Text = "10";
            this.delay.TextChanged += new System.EventHandler(this.pos_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(174, 63);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(30, 20);
            this.label13.TabIndex = 28;
            this.label13.Text = "ms";
            // 
            // fade
            // 
            this.fade.Location = new System.Drawing.Point(118, 25);
            this.fade.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.fade.Name = "fade";
            this.fade.Size = new System.Drawing.Size(54, 26);
            this.fade.TabIndex = 17;
            this.fade.Text = "20";
            this.fade.TextChanged += new System.EventHandler(this.pos_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 29);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 20);
            this.label5.TabIndex = 16;
            this.label5.Text = "Fade";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(232, 129);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 20);
            this.label12.TabIndex = 27;
            this.label12.Text = "(0-65535)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 95);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 20);
            this.label10.TabIndex = 22;
            this.label10.Text = "Brightness";
            // 
            // brightness
            // 
            this.brightness.Location = new System.Drawing.Point(176, 91);
            this.brightness.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.brightness.Name = "brightness";
            this.brightness.Size = new System.Drawing.Size(54, 26);
            this.brightness.TabIndex = 23;
            this.brightness.Text = "65535";
            this.brightness.TextChanged += new System.EventHandler(this.pos_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(232, 97);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(78, 20);
            this.label11.TabIndex = 26;
            this.label11.Text = "(0-65535)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 129);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 20);
            this.label9.TabIndex = 24;
            this.label9.Text = "Saturation";
            // 
            // saturation
            // 
            this.saturation.Location = new System.Drawing.Point(176, 125);
            this.saturation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.saturation.Name = "saturation";
            this.saturation.Size = new System.Drawing.Size(54, 26);
            this.saturation.TabIndex = 25;
            this.saturation.Text = "65535";
            this.saturation.TextChanged += new System.EventHandler(this.pos_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(9, 243);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(324, 51);
            this.button2.TabIndex = 35;
            this.button2.Text = "Assign Parts of Capture Area To Bulbs";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbLabels
            // 
            this.lbLabels.FormattingEnabled = true;
            this.lbLabels.ItemHeight = 20;
            this.lbLabels.Location = new System.Drawing.Point(10, 29);
            this.lbLabels.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lbLabels.Name = "lbLabels";
            this.lbLabels.Size = new System.Drawing.Size(320, 244);
            this.lbLabels.TabIndex = 87;
            this.lbLabels.SelectedIndexChanged += new System.EventHandler(this.lbLabels_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.btnMonitor1);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.bry);
            this.groupBox1.Controls.Add(this.brx);
            this.groupBox1.Controls.Add(this.tly);
            this.groupBox1.Controls.Add(this.tlx);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(26, 379);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(342, 312);
            this.groupBox1.TabIndex = 102;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Screen Area To Capture";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(10, 74);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(322, 35);
            this.button1.TabIndex = 37;
            this.button1.Text = "Secondary Monitor";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnMonitor1
            // 
            this.btnMonitor1.Location = new System.Drawing.Point(9, 29);
            this.btnMonitor1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnMonitor1.Name = "btnMonitor1";
            this.btnMonitor1.Size = new System.Drawing.Size(324, 35);
            this.btnMonitor1.TabIndex = 36;
            this.btnMonitor1.Text = "Primary Monitor";
            this.btnMonitor1.UseVisualStyleBackColor = true;
            this.btnMonitor1.Click += new System.EventHandler(this.btnMonitor1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbLabels);
            this.groupBox2.Location = new System.Drawing.Point(26, 142);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(342, 166);
            this.groupBox2.TabIndex = 103;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select Lights to Configure:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(39, 329);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(121, 24);
            this.checkBox1.TabIndex = 104;
            this.checkBox1.Text = "Enable Bulb";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // tbMonitor
            // 
            this.tbMonitor.Location = new System.Drawing.Point(176, 27);
            this.tbMonitor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbMonitor.Name = "tbMonitor";
            this.tbMonitor.Size = new System.Drawing.Size(54, 26);
            this.tbMonitor.TabIndex = 106;
            this.tbMonitor.Text = "0";
            this.tbMonitor.TextChanged += new System.EventHandler(this.pos_TextChanged);
            // 
            // ScreenColourUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 954);
            this.Controls.Add(this.tbMonitor);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ScreenColourUI";
            this.Text = "ScreenColourUI";
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tlx;
        private System.Windows.Forms.TextBox tly;
        private System.Windows.Forms.TextBox brx;
        private System.Windows.Forms.TextBox bry;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox delay;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox fade;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox brightness;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox saturation;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lbLabels;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbBrightnessMin;
        private System.Windows.Forms.TextBox tbSaturationMin;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnMonitor1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbKelvin;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label kelvin;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox tbMonitor;
    }
}