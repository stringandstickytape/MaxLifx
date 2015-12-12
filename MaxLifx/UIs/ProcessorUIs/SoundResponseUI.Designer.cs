namespace MaxLifx.UIs
{
    partial class SoundResponseUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoundResponseUI));
            MaxLifx.Controls.ColourStrategy.AnalogousColourStrategy analogousColourStrategy1 = new MaxLifx.Controls.ColourStrategy.AnalogousColourStrategy();
            MaxLifx.Controls.ColourStrategy.AnalogousColourStrategy analogousColourStrategy2 = new MaxLifx.Controls.ColourStrategy.AnalogousColourStrategy();
            MaxLifx.Controls.ColourStrategy.AnalogousColourStrategy analogousColourStrategy3 = new MaxLifx.Controls.ColourStrategy.AnalogousColourStrategy();
            MaxLifx.Controls.ColourStrategy.AnalogousColourStrategy analogousColourStrategy4 = new MaxLifx.Controls.ColourStrategy.AnalogousColourStrategy();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbMaxBrightness = new System.Windows.Forms.TrackBar();
            this.tbMinBrightness = new System.Windows.Forms.TrackBar();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.cbBrightnessInvert = new System.Windows.Forms.CheckBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.tbOnTimes = new System.Windows.Forms.TextBox();
            this.tbOffTimes = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.nDelay = new System.Windows.Forms.NumericUpDown();
            this.cbWaveType = new System.Windows.Forms.ComboBox();
            this.nTransition = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.nWaveDuration = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.cbSaturationInvert = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbFree = new System.Windows.Forms.CheckBox();
            this.cbPerBulb = new System.Windows.Forms.CheckBox();
            this.cbHueInvert = new System.Windows.Forms.CheckBox();
            this.cbLinkRanges = new System.Windows.Forms.CheckBox();
            this.hueSelector1 = new MaxLifx.Controls.HueSelector();
            this.button2 = new System.Windows.Forms.Button();
            this.cbConfigs = new System.Windows.Forms.ComboBox();
            this.bSave = new System.Windows.Forms.Button();
            this.lbLabels = new System.Windows.Forms.ListBox();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.spectrumAnalyser1 = new MaxLifx.Controls.SpectrumAnalyser();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.bUp = new System.Windows.Forms.Button();
            this.bDown = new System.Windows.Forms.Button();
            this.cbReorder = new System.Windows.Forms.CheckBox();
            this.cbUpdateAudioResponse = new System.Windows.Forms.CheckBox();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbMaxBrightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMinBrightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nTransition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWaveDuration)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.groupBox2);
            this.groupBox4.Controls.Add(this.tbOnTimes);
            this.groupBox4.Controls.Add(this.tbOffTimes);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.nDelay);
            this.groupBox4.Controls.Add(this.cbWaveType);
            this.groupBox4.Controls.Add(this.nTransition);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.nWaveDuration);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Location = new System.Drawing.Point(1175, 41);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(248, 418);
            this.groupBox4.TabIndex = 106;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Parameters";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 102;
            this.label2.Text = "Turn Off at:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 169);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 101;
            this.label1.Text = "Turn On at:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tbMaxBrightness);
            this.groupBox2.Controls.Add(this.tbMinBrightness);
            this.groupBox2.Controls.Add(this.pictureBox5);
            this.groupBox2.Controls.Add(this.cbBrightnessInvert);
            this.groupBox2.Controls.Add(this.pictureBox7);
            this.groupBox2.Controls.Add(this.pictureBox6);
            this.groupBox2.Location = new System.Drawing.Point(16, 232);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(216, 166);
            this.groupBox2.TabIndex = 104;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Brightness Range";
            // 
            // tbMaxBrightness
            // 
            this.tbMaxBrightness.LargeChange = 4096;
            this.tbMaxBrightness.Location = new System.Drawing.Point(-5, 82);
            this.tbMaxBrightness.Maximum = 65535;
            this.tbMaxBrightness.Name = "tbMaxBrightness";
            this.tbMaxBrightness.Size = new System.Drawing.Size(222, 45);
            this.tbMaxBrightness.SmallChange = 1024;
            this.tbMaxBrightness.TabIndex = 74;
            this.tbMaxBrightness.TickFrequency = 16384;
            this.tbMaxBrightness.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tbMaxBrightness.ValueChanged += new System.EventHandler(this.tbMaxBrightness_ValueChanged);
            // 
            // tbMinBrightness
            // 
            this.tbMinBrightness.LargeChange = 4096;
            this.tbMinBrightness.Location = new System.Drawing.Point(-5, 28);
            this.tbMinBrightness.Maximum = 65535;
            this.tbMinBrightness.Name = "tbMinBrightness";
            this.tbMinBrightness.Size = new System.Drawing.Size(222, 45);
            this.tbMinBrightness.SmallChange = 1024;
            this.tbMinBrightness.TabIndex = 69;
            this.tbMinBrightness.TickFrequency = 16384;
            this.tbMinBrightness.ValueChanged += new System.EventHandler(this.tbMinBrightness_ValueChanged);
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(6, 19);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(198, 23);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox5.TabIndex = 68;
            this.pictureBox5.TabStop = false;
            // 
            // cbBrightnessInvert
            // 
            this.cbBrightnessInvert.AutoSize = true;
            this.cbBrightnessInvert.Location = new System.Drawing.Point(6, 142);
            this.cbBrightnessInvert.Name = "cbBrightnessInvert";
            this.cbBrightnessInvert.Size = new System.Drawing.Size(53, 17);
            this.cbBrightnessInvert.TabIndex = 101;
            this.cbBrightnessInvert.Text = "Invert";
            this.cbBrightnessInvert.UseVisualStyleBackColor = true;
            this.cbBrightnessInvert.CheckedChanged += new System.EventHandler(this.cbBrightnessInvert_CheckedChanged);
            // 
            // pictureBox7
            // 
            this.pictureBox7.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox7.Image")));
            this.pictureBox7.Location = new System.Drawing.Point(6, 62);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(198, 31);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox7.TabIndex = 72;
            this.pictureBox7.TabStop = false;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox6.Image")));
            this.pictureBox6.Location = new System.Drawing.Point(6, 113);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(198, 23);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox6.TabIndex = 73;
            this.pictureBox6.TabStop = false;
            // 
            // tbOnTimes
            // 
            this.tbOnTimes.Location = new System.Drawing.Point(95, 166);
            this.tbOnTimes.Name = "tbOnTimes";
            this.tbOnTimes.Size = new System.Drawing.Size(100, 20);
            this.tbOnTimes.TabIndex = 100;
            this.tbOnTimes.TextChanged += new System.EventHandler(this.tbOnTimes_TextChanged);
            // 
            // tbOffTimes
            // 
            this.tbOffTimes.Location = new System.Drawing.Point(94, 192);
            this.tbOffTimes.Name = "tbOffTimes";
            this.tbOffTimes.Size = new System.Drawing.Size(100, 20);
            this.tbOffTimes.TabIndex = 99;
            this.tbOffTimes.TextChanged += new System.EventHandler(this.tbOffTimes_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 137);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(235, 23);
            this.button1.TabIndex = 95;
            this.button1.Text = "Reset Wave Timer";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // nDelay
            // 
            this.nDelay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nDelay.Location = new System.Drawing.Point(94, 19);
            this.nDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nDelay.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nDelay.Name = "nDelay";
            this.nDelay.Size = new System.Drawing.Size(120, 20);
            this.nDelay.TabIndex = 88;
            this.nDelay.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nDelay.ValueChanged += new System.EventHandler(this.nDelay_ValueChanged);
            // 
            // cbWaveType
            // 
            this.cbWaveType.FormattingEnabled = true;
            this.cbWaveType.Location = new System.Drawing.Point(95, 84);
            this.cbWaveType.Name = "cbWaveType";
            this.cbWaveType.Size = new System.Drawing.Size(121, 21);
            this.cbWaveType.TabIndex = 87;
            this.cbWaveType.SelectedIndexChanged += new System.EventHandler(this.cbWaveType_SelectedIndexChanged);
            // 
            // nTransition
            // 
            this.nTransition.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nTransition.Location = new System.Drawing.Point(95, 45);
            this.nTransition.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nTransition.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nTransition.Name = "nTransition";
            this.nTransition.Size = new System.Drawing.Size(120, 20);
            this.nTransition.TabIndex = 89;
            this.nTransition.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nTransition.ValueChanged += new System.EventHandler(this.nTransition_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(51, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 90;
            this.label7.Text = "Delay";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(33, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 91;
            this.label8.Text = "Transition";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(222, 113);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(20, 13);
            this.label12.TabIndex = 98;
            this.label12.Text = "ms";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(221, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 13);
            this.label10.TabIndex = 92;
            this.label10.Text = "ms";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(4, 113);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 13);
            this.label13.TabIndex = 97;
            this.label13.Text = "Wave Duration";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(221, 47);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 13);
            this.label9.TabIndex = 93;
            this.label9.Text = "ms";
            // 
            // nWaveDuration
            // 
            this.nWaveDuration.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nWaveDuration.Location = new System.Drawing.Point(95, 111);
            this.nWaveDuration.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nWaveDuration.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nWaveDuration.Name = "nWaveDuration";
            this.nWaveDuration.Size = new System.Drawing.Size(120, 20);
            this.nWaveDuration.TabIndex = 96;
            this.nWaveDuration.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nWaveDuration.ValueChanged += new System.EventHandler(this.nWaveDuration_Changed);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(22, 87);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 13);
            this.label11.TabIndex = 94;
            this.label11.Text = "Wave Type";
            // 
            // cbSaturationInvert
            // 
            this.cbSaturationInvert.AutoSize = true;
            this.cbSaturationInvert.Location = new System.Drawing.Point(6, 503);
            this.cbSaturationInvert.Name = "cbSaturationInvert";
            this.cbSaturationInvert.Size = new System.Drawing.Size(133, 17);
            this.cbSaturationInvert.TabIndex = 102;
            this.cbSaturationInvert.Text = "Invert Saturation Order";
            this.cbSaturationInvert.UseVisualStyleBackColor = true;
            this.cbSaturationInvert.CheckedChanged += new System.EventHandler(this.cbSaturationInvert_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbFree);
            this.groupBox1.Controls.Add(this.cbPerBulb);
            this.groupBox1.Controls.Add(this.cbHueInvert);
            this.groupBox1.Controls.Add(this.cbLinkRanges);
            this.groupBox1.Controls.Add(this.cbSaturationInvert);
            this.groupBox1.Controls.Add(this.hueSelector1);
            this.groupBox1.Location = new System.Drawing.Point(120, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(523, 536);
            this.groupBox1.TabIndex = 103;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hue Range";
            // 
            // cbFree
            // 
            this.cbFree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFree.AutoSize = true;
            this.cbFree.Location = new System.Drawing.Point(6, 89);
            this.cbFree.Name = "cbFree";
            this.cbFree.Size = new System.Drawing.Size(47, 17);
            this.cbFree.TabIndex = 111;
            this.cbFree.Text = "Free";
            this.cbFree.UseVisualStyleBackColor = true;
            this.cbFree.CheckedChanged += new System.EventHandler(this.cbFree_CheckedChanged);
            // 
            // cbPerBulb
            // 
            this.cbPerBulb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPerBulb.AutoSize = true;
            this.cbPerBulb.Location = new System.Drawing.Point(6, 43);
            this.cbPerBulb.Name = "cbPerBulb";
            this.cbPerBulb.Size = new System.Drawing.Size(99, 17);
            this.cbPerBulb.TabIndex = 108;
            this.cbPerBulb.Text = "Colour Per Bulb";
            this.cbPerBulb.UseVisualStyleBackColor = true;
            this.cbPerBulb.CheckedChanged += new System.EventHandler(this.cbPerBulb_CheckedChanged);
            // 
            // cbHueInvert
            // 
            this.cbHueInvert.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbHueInvert.AutoSize = true;
            this.cbHueInvert.Location = new System.Drawing.Point(6, 19);
            this.cbHueInvert.Margin = new System.Windows.Forms.Padding(460, 460, 3, 3);
            this.cbHueInvert.Name = "cbHueInvert";
            this.cbHueInvert.Size = new System.Drawing.Size(111, 17);
            this.cbHueInvert.TabIndex = 103;
            this.cbHueInvert.Text = "Invert Hue Range";
            this.cbHueInvert.UseVisualStyleBackColor = true;
            this.cbHueInvert.CheckedChanged += new System.EventHandler(this.cbHueInvert_CheckedChanged);
            // 
            // cbLinkRanges
            // 
            this.cbLinkRanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLinkRanges.AutoSize = true;
            this.cbLinkRanges.Location = new System.Drawing.Point(6, 66);
            this.cbLinkRanges.Name = "cbLinkRanges";
            this.cbLinkRanges.Size = new System.Drawing.Size(86, 17);
            this.cbLinkRanges.TabIndex = 110;
            this.cbLinkRanges.Text = "Link Ranges";
            this.cbLinkRanges.UseVisualStyleBackColor = true;
            this.cbLinkRanges.CheckedChanged += new System.EventHandler(this.cbLinkRanges_CheckedChanged);
            // 
            // hueSelector1
            // 
            this.hueSelector1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hueSelector1.BackColor = System.Drawing.SystemColors.Control;
            this.hueSelector1.Brightness = 0D;
            this.hueSelector1.Free = false;
            this.hueSelector1.GenericColourStrategy = analogousColourStrategy1;
            this.hueSelector1.HandleCount = 1;
            this.hueSelector1.Invert = false;
            this.hueSelector1.LinkRanges = false;
            this.hueSelector1.Location = new System.Drawing.Point(4, 16);
            this.hueSelector1.Margin = new System.Windows.Forms.Padding(0);
            this.hueSelector1.Name = "hueSelector1";
            this.hueSelector1.PerBulb = false;
            this.hueSelector1.SelectedOneColourStrategy = analogousColourStrategy2;
            this.hueSelector1.SelectedThreeColourStrategy = analogousColourStrategy3;
            this.hueSelector1.SelectedTwoColourStrategy = analogousColourStrategy4;
            this.hueSelector1.Size = new System.Drawing.Size(514, 514);
            this.hueSelector1.TabIndex = 107;
            this.hueSelector1.Text = "z";
            this.hueSelector1.HuesChanged += new System.EventHandler(this.colourControl1_HuesChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(120, 583);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(99, 23);
            this.button2.TabIndex = 109;
            this.button2.Text = "Reset Ranges";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
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
            // lbLabels
            // 
            this.lbLabels.FormattingEnabled = true;
            this.lbLabels.Location = new System.Drawing.Point(13, 93);
            this.lbLabels.Name = "lbLabels";
            this.lbLabels.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbLabels.Size = new System.Drawing.Size(101, 511);
            this.lbLabels.TabIndex = 86;
            this.lbLabels.SelectedIndexChanged += new System.EventHandler(this.lbLabels_SelectedIndexChanged);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // spectrumAnalyser1
            // 
            this.spectrumAnalyser1.Location = new System.Drawing.Point(4, 19);
            this.spectrumAnalyser1.Name = "spectrumAnalyser1";
            this.spectrumAnalyser1.Size = new System.Drawing.Size(512, 512);
            this.spectrumAnalyser1.TabIndex = 107;
            this.spectrumAnalyser1.Text = "spectrumAnalyser1";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(649, 583);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(135, 23);
            this.button3.TabIndex = 108;
            this.button3.Text = "Reset Ranges";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.cbUpdateAudioResponse);
            this.groupBox3.Controls.Add(this.spectrumAnalyser1);
            this.groupBox3.Location = new System.Drawing.Point(649, 41);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(520, 536);
            this.groupBox3.TabIndex = 110;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Audio Frequency Response";
            // 
            // bUp
            // 
            this.bUp.Enabled = false;
            this.bUp.Location = new System.Drawing.Point(16, 40);
            this.bUp.Name = "bUp";
            this.bUp.Size = new System.Drawing.Size(44, 23);
            this.bUp.TabIndex = 111;
            this.bUp.Text = "Up";
            this.bUp.UseVisualStyleBackColor = true;
            this.bUp.Click += new System.EventHandler(this.bUp_Click);
            // 
            // bDown
            // 
            this.bDown.Enabled = false;
            this.bDown.Location = new System.Drawing.Point(66, 40);
            this.bDown.Name = "bDown";
            this.bDown.Size = new System.Drawing.Size(44, 23);
            this.bDown.TabIndex = 112;
            this.bDown.Text = "Down";
            this.bDown.UseVisualStyleBackColor = true;
            this.bDown.Click += new System.EventHandler(this.bDown_Click);
            // 
            // cbReorder
            // 
            this.cbReorder.AutoSize = true;
            this.cbReorder.Location = new System.Drawing.Point(19, 72);
            this.cbReorder.Name = "cbReorder";
            this.cbReorder.Size = new System.Drawing.Size(64, 17);
            this.cbReorder.TabIndex = 113;
            this.cbReorder.Text = "Reorder";
            this.cbReorder.UseVisualStyleBackColor = true;
            this.cbReorder.CheckedChanged += new System.EventHandler(this.cbReorder_CheckedChanged);
            // 
            // cbUpdateAudioResponse
            // 
            this.cbUpdateAudioResponse.AutoSize = true;
            this.cbUpdateAudioResponse.Checked = true;
            this.cbUpdateAudioResponse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUpdateAudioResponse.Location = new System.Drawing.Point(145, -1);
            this.cbUpdateAudioResponse.Name = "cbUpdateAudioResponse";
            this.cbUpdateAudioResponse.Size = new System.Drawing.Size(188, 17);
            this.cbUpdateAudioResponse.TabIndex = 114;
            this.cbUpdateAudioResponse.Text = "update in real time (CPU intensive)";
            this.cbUpdateAudioResponse.UseVisualStyleBackColor = true;
            this.cbUpdateAudioResponse.CheckedChanged += new System.EventHandler(this.cbUpdateAudioResponse_CheckedChanged);
            // 
            // SoundResponseUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1435, 617);
            this.Controls.Add(this.cbReorder);
            this.Controls.Add(this.bDown);
            this.Controls.Add(this.bUp);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbConfigs);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.lbLabels);
            this.Controls.Add(this.groupBox4);
            this.MaximumSize = new System.Drawing.Size(1451, 656);
            this.MinimumSize = new System.Drawing.Size(1451, 656);
            this.Name = "SoundResponseUI";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SoundResponseUI";
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbMaxBrightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMinBrightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nTransition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWaveDuration)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.TrackBar tbMinBrightness;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.TrackBar tbMaxBrightness;
        private System.Windows.Forms.ListBox lbLabels;
        private System.Windows.Forms.ComboBox cbWaveType;
        private System.Windows.Forms.NumericUpDown nDelay;
        private System.Windows.Forms.NumericUpDown nTransition;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown nWaveDuration;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.ComboBox cbConfigs;
        private System.Windows.Forms.CheckBox cbBrightnessInvert;
        private System.Windows.Forms.CheckBox cbSaturationInvert;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox cbHueInvert;
        private System.Windows.Forms.TextBox tbOffTimes;
        private System.Windows.Forms.TextBox tbOnTimes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private MaxLifx.Controls.HueSelector hueSelector1;
        private System.Windows.Forms.CheckBox cbPerBulb;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox cbLinkRanges;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.CheckBox cbFree;
        private Controls.SpectrumAnalyser spectrumAnalyser1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button bUp;
        private System.Windows.Forms.Button bDown;
        private System.Windows.Forms.CheckBox cbReorder;
        private System.Windows.Forms.CheckBox cbUpdateAudioResponse;
    }
}