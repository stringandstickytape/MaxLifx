namespace MaxLifx
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.lbBulbs = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbManualBulbMac = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.lvThreads = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button8 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.bStopAll = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.panelBulbColours = new System.Windows.Forms.Panel();
            this.button10 = new System.Windows.Forms.Button();
            this.bSaveSched = new System.Windows.Forms.Button();
            this.tbSchedTime = new System.Windows.Forms.TextBox();
            this.button9 = new System.Windows.Forms.Button();
            this.bTimelineAdd = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.bDelete = new System.Windows.Forms.Button();
            this.bContinue = new System.Windows.Forms.Button();
            this.gbMonitors = new System.Windows.Forms.GroupBox();
            this.gbSequencer = new System.Windows.Forms.GroupBox();
            this.timeline1 = new MaxLifx.Controls.Timeline();
            this.bCollapseMonitors = new System.Windows.Forms.Button();
            this.bCollapseSequencer = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.bulbsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.turnOnAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turnOffAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.advancedDiscoverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox3.SuspendLayout();
            this.gbMonitors.SuspendLayout();
            this.gbSequencer.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbBulbs
            // 
            this.lbBulbs.FormattingEnabled = true;
            this.lbBulbs.Location = new System.Drawing.Point(15, 19);
            this.lbBulbs.Name = "lbBulbs";
            this.lbBulbs.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbBulbs.Size = new System.Drawing.Size(188, 147);
            this.lbBulbs.TabIndex = 2;
            this.lbBulbs.SelectedIndexChanged += new System.EventHandler(this.lbBulbs_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.tbManualBulbMac);
            this.groupBox3.Controls.Add(this.button5);
            this.groupBox3.Controls.Add(this.lbBulbs);
            this.groupBox3.Location = new System.Drawing.Point(757, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(219, 227);
            this.groupBox3.TabIndex = 31;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Bulbs";
            this.groupBox3.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 176);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Add Manual / Virtual Bulb";
            // 
            // tbManualBulbMac
            // 
            this.tbManualBulbMac.Location = new System.Drawing.Point(15, 192);
            this.tbManualBulbMac.Name = "tbManualBulbMac";
            this.tbManualBulbMac.Size = new System.Drawing.Size(125, 20);
            this.tbManualBulbMac.TabIndex = 4;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(146, 191);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(55, 23);
            this.button5.TabIndex = 3;
            this.button5.Text = "Add";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(156, 31);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(129, 56);
            this.button6.TabIndex = 0;
            this.button6.Text = "Start Sound Response Thread";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(275, 184);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(136, 23);
            this.button7.TabIndex = 35;
            this.button7.Text = "End Thread";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // lvThreads
            // 
            this.lvThreads.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvThreads.Location = new System.Drawing.Point(6, 19);
            this.lvThreads.MultiSelect = false;
            this.lvThreads.Name = "lvThreads";
            this.lvThreads.Size = new System.Drawing.Size(236, 146);
            this.lvThreads.TabIndex = 36;
            this.lvThreads.UseCompatibleStateImageBehavior = false;
            this.lvThreads.View = System.Windows.Forms.View.Details;
            this.lvThreads.SelectedIndexChanged += new System.EventHandler(this.lvThreads_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 160;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "UUID";
            this.columnHeader2.Width = 184;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(275, 112);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(136, 66);
            this.button8.TabIndex = 37;
            this.button8.Text = "Show Thread Settings";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 57);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start Screen Colour Thread";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(349, 213);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(62, 23);
            this.button2.TabIndex = 38;
            this.button2.Text = "Save Threadset";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(275, 213);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(68, 23);
            this.button3.TabIndex = 39;
            this.button3.Text = "Load Threadset";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // bStopAll
            // 
            this.bStopAll.Location = new System.Drawing.Point(275, 242);
            this.bStopAll.Name = "bStopAll";
            this.bStopAll.Size = new System.Drawing.Size(136, 23);
            this.bStopAll.TabIndex = 40;
            this.bStopAll.Text = "End All";
            this.bStopAll.UseVisualStyleBackColor = true;
            this.bStopAll.Click += new System.EventHandler(this.bStopAll_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(291, 30);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(120, 59);
            this.button4.TabIndex = 41;
            this.button4.Text = "Start Sound Generator";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // panelBulbColours
            // 
            this.panelBulbColours.AutoScroll = true;
            this.panelBulbColours.Location = new System.Drawing.Point(6, 19);
            this.panelBulbColours.Name = "panelBulbColours";
            this.panelBulbColours.Size = new System.Drawing.Size(380, 141);
            this.panelBulbColours.TabIndex = 46;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(639, 246);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(102, 23);
            this.button10.TabIndex = 5;
            this.button10.Text = "Load Sequence";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // bSaveSched
            // 
            this.bSaveSched.Location = new System.Drawing.Point(639, 275);
            this.bSaveSched.Name = "bSaveSched";
            this.bSaveSched.Size = new System.Drawing.Size(102, 23);
            this.bSaveSched.TabIndex = 4;
            this.bSaveSched.Text = "Save Sequence";
            this.bSaveSched.UseVisualStyleBackColor = true;
            this.bSaveSched.Click += new System.EventHandler(this.bSaveSched_Click);
            // 
            // tbSchedTime
            // 
            this.tbSchedTime.Enabled = false;
            this.tbSchedTime.Location = new System.Drawing.Point(639, 143);
            this.tbSchedTime.Name = "tbSchedTime";
            this.tbSchedTime.Size = new System.Drawing.Size(101, 20);
            this.tbSchedTime.TabIndex = 2;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(639, 198);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(101, 23);
            this.button9.TabIndex = 1;
            this.button9.Text = "Restart";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // bTimelineAdd
            // 
            this.bTimelineAdd.Location = new System.Drawing.Point(639, 22);
            this.bTimelineAdd.Name = "bTimelineAdd";
            this.bTimelineAdd.Size = new System.Drawing.Size(101, 23);
            this.bTimelineAdd.TabIndex = 49;
            this.bTimelineAdd.Text = "Add";
            this.bTimelineAdd.UseVisualStyleBackColor = true;
            this.bTimelineAdd.Click += new System.EventHandler(this.bTimelineAdd_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(639, 51);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(101, 23);
            this.button11.TabIndex = 50;
            this.button11.Text = "Edit";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // bDelete
            // 
            this.bDelete.Location = new System.Drawing.Point(639, 80);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(101, 23);
            this.bDelete.TabIndex = 51;
            this.bDelete.Text = "Delete";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bContinue
            // 
            this.bContinue.Location = new System.Drawing.Point(639, 169);
            this.bContinue.Name = "bContinue";
            this.bContinue.Size = new System.Drawing.Size(101, 23);
            this.bContinue.TabIndex = 52;
            this.bContinue.Text = "Play/Pause";
            this.bContinue.UseVisualStyleBackColor = true;
            this.bContinue.Click += new System.EventHandler(this.bContinue_Click);
            // 
            // gbMonitors
            // 
            this.gbMonitors.Controls.Add(this.panelBulbColours);
            this.gbMonitors.Location = new System.Drawing.Point(10, 270);
            this.gbMonitors.Name = "gbMonitors";
            this.gbMonitors.Size = new System.Drawing.Size(401, 166);
            this.gbMonitors.TabIndex = 53;
            this.gbMonitors.TabStop = false;
            this.gbMonitors.Text = "Bulb Monitors";
            // 
            // gbSequencer
            // 
            this.gbSequencer.Controls.Add(this.button10);
            this.gbSequencer.Controls.Add(this.bContinue);
            this.gbSequencer.Controls.Add(this.timeline1);
            this.gbSequencer.Controls.Add(this.bSaveSched);
            this.gbSequencer.Controls.Add(this.bDelete);
            this.gbSequencer.Controls.Add(this.button11);
            this.gbSequencer.Controls.Add(this.bTimelineAdd);
            this.gbSequencer.Controls.Add(this.tbSchedTime);
            this.gbSequencer.Controls.Add(this.button9);
            this.gbSequencer.Location = new System.Drawing.Point(9, 442);
            this.gbSequencer.Name = "gbSequencer";
            this.gbSequencer.Size = new System.Drawing.Size(402, 18);
            this.gbSequencer.TabIndex = 54;
            this.gbSequencer.TabStop = false;
            this.gbSequencer.Text = "Threadset / MP3 Sequencer";
            // 
            // timeline1
            // 
            this.timeline1.Location = new System.Drawing.Point(7, 19);
            this.timeline1.Name = "timeline1";
            this.timeline1.PlaybackTime = 0F;
            this.timeline1.Size = new System.Drawing.Size(615, 274);
            this.timeline1.TabIndex = 47;
            this.timeline1.Text = "timeline1";
            // 
            // bCollapseMonitors
            // 
            this.bCollapseMonitors.Location = new System.Drawing.Point(-2, 264);
            this.bCollapseMonitors.Name = "bCollapseMonitors";
            this.bCollapseMonitors.Size = new System.Drawing.Size(18, 23);
            this.bCollapseMonitors.TabIndex = 55;
            this.bCollapseMonitors.Text = "+/-";
            this.bCollapseMonitors.UseVisualStyleBackColor = true;
            this.bCollapseMonitors.Click += new System.EventHandler(this.bCollapseMonitors_Click);
            // 
            // bCollapseSequencer
            // 
            this.bCollapseSequencer.Location = new System.Drawing.Point(-2, 436);
            this.bCollapseSequencer.Name = "bCollapseSequencer";
            this.bCollapseSequencer.Size = new System.Drawing.Size(18, 23);
            this.bCollapseSequencer.TabIndex = 56;
            this.bCollapseSequencer.Text = "+/-";
            this.bCollapseSequencer.UseVisualStyleBackColor = true;
            this.bCollapseSequencer.Click += new System.EventHandler(this.bCollapseSequencer_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bulbsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(434, 24);
            this.menuStrip1.TabIndex = 58;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // bulbsToolStripMenuItem
            // 
            this.bulbsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripSeparator1,
            this.turnOnAllToolStripMenuItem,
            this.turnOffAllToolStripMenuItem,
            this.panicToolStripMenuItem,
            this.toolStripSeparator2,
            this.advancedDiscoverToolStripMenuItem});
            this.bulbsToolStripMenuItem.Name = "bulbsToolStripMenuItem";
            this.bulbsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.bulbsToolStripMenuItem.Text = "Bulbs";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItem1.Text = "Discover";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(172, 6);
            // 
            // turnOnAllToolStripMenuItem
            // 
            this.turnOnAllToolStripMenuItem.Name = "turnOnAllToolStripMenuItem";
            this.turnOnAllToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.turnOnAllToolStripMenuItem.Text = "Turn On All";
            this.turnOnAllToolStripMenuItem.Click += new System.EventHandler(this.turnOnAllToolStripMenuItem_Click);
            // 
            // turnOffAllToolStripMenuItem
            // 
            this.turnOffAllToolStripMenuItem.Name = "turnOffAllToolStripMenuItem";
            this.turnOffAllToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.turnOffAllToolStripMenuItem.Text = "Turn Off All";
            this.turnOffAllToolStripMenuItem.Click += new System.EventHandler(this.turnOffAllToolStripMenuItem_Click);
            // 
            // panicToolStripMenuItem
            // 
            this.panicToolStripMenuItem.Name = "panicToolStripMenuItem";
            this.panicToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.panicToolStripMenuItem.Text = "Panic";
            this.panicToolStripMenuItem.Click += new System.EventHandler(this.panicToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(172, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "MaxLifx-Z minimised to System Tray";
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvThreads);
            this.groupBox1.Location = new System.Drawing.Point(19, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 171);
            this.groupBox1.TabIndex = 59;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Running Threads";
            // 
            // advancedDiscoverToolStripMenuItem
            // 
            this.advancedDiscoverToolStripMenuItem.Name = "advancedDiscoverToolStripMenuItem";
            this.advancedDiscoverToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.advancedDiscoverToolStripMenuItem.Text = "Advanced Discover";
            this.advancedDiscoverToolStripMenuItem.Click += new System.EventHandler(this.advancedDiscoverToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 466);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bStopAll);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.bCollapseSequencer);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bCollapseMonitors);
            this.Controls.Add(this.gbSequencer);
            this.Controls.Add(this.gbMonitors);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(450, 505);
            this.MinimumSize = new System.Drawing.Size(450, 505);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "MaxLifx-Z";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.gbMonitors.ResumeLayout(false);
            this.gbSequencer.ResumeLayout(false);
            this.gbSequencer.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lbBulbs;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.ListView lvThreads;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button bStopAll;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel panelBulbColours;
        private System.Windows.Forms.TextBox tbSchedTime;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button bSaveSched;
        private System.Windows.Forms.Button button10;
        private MaxLifx.Controls.Timeline timeline1;
        private System.Windows.Forms.Button bTimelineAdd;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.Button bContinue;
        private System.Windows.Forms.GroupBox gbMonitors;
        private System.Windows.Forms.GroupBox gbSequencer;
        private System.Windows.Forms.Button bCollapseMonitors;
        private System.Windows.Forms.Button bCollapseSequencer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem bulbsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem turnOnAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem turnOffAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem panicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbManualBulbMac;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem advancedDiscoverToolStripMenuItem;
    }
}

