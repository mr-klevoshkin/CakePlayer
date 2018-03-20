using System;
using System.Windows.Forms;
using NAudio.Mixer;

namespace NAudioPlayer
{
    partial class Form1
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
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.browseButton = new System.Windows.Forms.Button();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.volume = new System.Windows.Forms.TrackBar();
            this.loopCheckBox = new System.Windows.Forms.CheckBox();
            this.AudioPosition = new System.Windows.Forms.TrackBar();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.TimePosition = new System.Windows.Forms.Label();
            this.A = new System.Windows.Forms.Label();
            this.B = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.front = new System.Windows.Forms.Button();
            this.back = new System.Windows.Forms.Button();
            this.speedBar = new System.Windows.Forms.TrackBar();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.labelVolumeText = new System.Windows.Forms.Label();
            this.speedLabelText = new System.Windows.Forms.Label();
            this.volumeLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.channelLeftGainTrack = new System.Windows.Forms.TrackBar();
            this.channelRightGainTrack = new System.Windows.Forms.TrackBar();
            this.boxStereo = new System.Windows.Forms.CheckBox();
            this.stereoPanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.equalizerControl1 = new NAudioPlayer.EqualizerControl();
            this.flowPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.volume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AudioPosition)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelLeftGainTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelRightGainTrack)).BeginInit();
            this.stereoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowPanel
            // 
            this.flowPanel.Controls.Add(this.browseButton);
            this.flowPanel.Controls.Add(this.buttonPlay);
            this.flowPanel.Controls.Add(this.buttonStop);
            this.flowPanel.Controls.Add(this.resetButton);
            this.flowPanel.Location = new System.Drawing.Point(0, 0);
            this.flowPanel.Margin = new System.Windows.Forms.Padding(10);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Size = new System.Drawing.Size(292, 30);
            this.flowPanel.TabIndex = 0;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(3, 3);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(65, 25);
            this.browseButton.TabIndex = 2;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(74, 3);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(65, 25);
            this.buttonPlay.TabIndex = 0;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(145, 3);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(65, 25);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "Stop";
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(216, 3);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(65, 25);
            this.resetButton.TabIndex = 14;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // volume
            // 
            this.volume.LargeChange = 10;
            this.volume.Location = new System.Drawing.Point(3, 71);
            this.volume.Maximum = 100;
            this.volume.Name = "volume";
            this.volume.Size = new System.Drawing.Size(259, 45);
            this.volume.SmallChange = 20;
            this.volume.TabIndex = 20;
            this.volume.TickFrequency = 10;
            this.volume.Scroll += new System.EventHandler(this.volume_Scroll);
            // 
            // loop
            // 
            this.loopCheckBox.AutoSize = true;
            this.loopCheckBox.Location = new System.Drawing.Point(312, 187);
            this.loopCheckBox.Name = "loop";
            this.loopCheckBox.Size = new System.Drawing.Size(49, 17);
            this.loopCheckBox.TabIndex = 3;
            this.loopCheckBox.Text = "A - B";
            this.loopCheckBox.UseVisualStyleBackColor = true;
            this.loopCheckBox.CheckedChanged += new System.EventHandler(this.loop_CheckedChanged);
            // 
            // AudioPosition
            // 
            this.AudioPosition.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AudioPosition.Location = new System.Drawing.Point(312, 80);
            this.AudioPosition.Margin = new System.Windows.Forms.Padding(0);
            this.AudioPosition.Name = "AudioPosition";
            this.AudioPosition.Size = new System.Drawing.Size(474, 45);
            this.AudioPosition.TabIndex = 0;
            this.AudioPosition.TickFrequency = 10;
            this.AudioPosition.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.AudioPosition.Scroll += new System.EventHandler(this.AudioPosition_Scroll);
            this.AudioPosition.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AudioPosition_MouseUp);
            // 
            // timer
            // 
            this.timer.Interval = 500;
            this.timer.Tick += new System.EventHandler(this.timer_tick);
            // 
            // TimePosition
            // 
            this.TimePosition.AutoSize = true;
            this.TimePosition.Location = new System.Drawing.Point(309, 171);
            this.TimePosition.Name = "TimePosition";
            this.TimePosition.Size = new System.Drawing.Size(66, 13);
            this.TimePosition.TabIndex = 2;
            this.TimePosition.Text = "00:00/00:00";
            // 
            // A
            // 
            this.A.AutoSize = true;
            this.A.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.A.Cursor = System.Windows.Forms.Cursors.NoMoveHoriz;
            this.A.Location = new System.Drawing.Point(5, 3);
            this.A.Name = "A";
            this.A.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.A.Size = new System.Drawing.Size(16, 15);
            this.A.TabIndex = 3;
            this.A.Text = "A";
            this.A.MouseDown += new System.Windows.Forms.MouseEventHandler(this.A_MouseDown);
            this.A.MouseMove += new System.Windows.Forms.MouseEventHandler(this.A_MouseMove);
            this.A.MouseUp += new System.Windows.Forms.MouseEventHandler(this.A_MouseUp);
            // 
            // B
            // 
            this.B.AutoSize = true;
            this.B.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.B.Cursor = System.Windows.Forms.Cursors.NoMoveHoriz;
            this.B.Location = new System.Drawing.Point(453, 3);
            this.B.Name = "B";
            this.B.Size = new System.Drawing.Size(16, 15);
            this.B.TabIndex = 4;
            this.B.Text = "B";
            this.B.MouseDown += new System.Windows.Forms.MouseEventHandler(this.B_MouseDown);
            this.B.MouseMove += new System.Windows.Forms.MouseEventHandler(this.B_MouseMove);
            this.B.MouseUp += new System.Windows.Forms.MouseEventHandler(this.B_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.A);
            this.panel1.Controls.Add(this.B);
            this.panel1.Location = new System.Drawing.Point(312, 125);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(474, 26);
            this.panel1.TabIndex = 5;
            // 
            // front
            // 
            this.front.Location = new System.Drawing.Point(442, 171);
            this.front.Name = "front";
            this.front.Size = new System.Drawing.Size(24, 23);
            this.front.TabIndex = 5;
            this.front.Text = "►";
            this.front.UseVisualStyleBackColor = true;
            this.front.Click += new System.EventHandler(this.front_Click);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(403, 171);
            this.back.Name = "back";
            this.back.Size = new System.Drawing.Size(24, 23);
            this.back.TabIndex = 5;
            this.back.Text = "◄";
            this.back.UseVisualStyleBackColor = true;
            this.back.Click += new System.EventHandler(this.back_Click);
            // 
            // speedBar
            // 
            this.speedBar.Location = new System.Drawing.Point(500, 175);
            this.speedBar.Maximum = 20;
            this.speedBar.Name = "speedBar";
            this.speedBar.Size = new System.Drawing.Size(259, 45);
            this.speedBar.TabIndex = 6;
            this.speedBar.Value = 5;
            this.speedBar.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // labelSpeed
            // 
            this.labelSpeed.AutoSize = true;
            this.labelSpeed.Location = new System.Drawing.Point(765, 175);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(27, 13);
            this.labelSpeed.TabIndex = 7;
            this.labelSpeed.Text = "x1.0";
            // 
            // labelVolumeText
            // 
            this.labelVolumeText.AutoSize = true;
            this.labelVolumeText.Location = new System.Drawing.Point(12, 51);
            this.labelVolumeText.Name = "labelVolumeText";
            this.labelVolumeText.Size = new System.Drawing.Size(68, 13);
            this.labelVolumeText.TabIndex = 3;
            this.labelVolumeText.Text = "Main Volume";
            // 
            // speedLabelText
            // 
            this.speedLabelText.AutoSize = true;
            this.speedLabelText.Location = new System.Drawing.Point(509, 159);
            this.speedLabelText.Name = "speedLabelText";
            this.speedLabelText.Size = new System.Drawing.Size(38, 13);
            this.speedLabelText.TabIndex = 3;
            this.speedLabelText.Text = "Speed";
            // 
            // volumeLabel
            // 
            this.volumeLabel.AutoSize = true;
            this.volumeLabel.Location = new System.Drawing.Point(268, 71);
            this.volumeLabel.Name = "volumeLabel";
            this.volumeLabel.Size = new System.Drawing.Size(24, 13);
            this.volumeLabel.TabIndex = 8;
            this.volumeLabel.Text = "0 %";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(312, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(474, 74);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            // 
            // channelLeftGainTrack
            // 
            this.channelLeftGainTrack.Location = new System.Drawing.Point(104, 0);
            this.channelLeftGainTrack.Name = "channelLeftGainTrack";
            this.channelLeftGainTrack.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.channelLeftGainTrack.Size = new System.Drawing.Size(45, 107);
            this.channelLeftGainTrack.TabIndex = 18;
            this.channelLeftGainTrack.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.channelLeftGainTrack.Value = 10;
            this.channelLeftGainTrack.Scroll += new System.EventHandler(this.channelLeftGainTrack_Scroll);
            // 
            // channelRightGainTrack
            // 
            this.channelRightGainTrack.Location = new System.Drawing.Point(155, 0);
            this.channelRightGainTrack.Name = "channelRightGainTrack";
            this.channelRightGainTrack.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.channelRightGainTrack.Size = new System.Drawing.Size(45, 107);
            this.channelRightGainTrack.TabIndex = 19;
            this.channelRightGainTrack.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.channelRightGainTrack.Value = 10;
            this.channelRightGainTrack.Scroll += new System.EventHandler(this.channelRightGainTrack_Scroll);
            // 
            // boxStereo
            // 
            this.boxStereo.AutoSize = true;
            this.boxStereo.BackColor = System.Drawing.SystemColors.Control;
            this.boxStereo.Location = new System.Drawing.Point(3, 41);
            this.boxStereo.Name = "boxStereo";
            this.boxStereo.Size = new System.Drawing.Size(99, 17);
            this.boxStereo.TabIndex = 13;
            this.boxStereo.Text = "Stereo to Mono";
            this.boxStereo.UseVisualStyleBackColor = false;
            this.boxStereo.CheckedChanged += new System.EventHandler(this.boxStereoMono_CheckedChanged);
            // 
            // stereoPanel
            // 
            this.stereoPanel.Controls.Add(this.label4);
            this.stereoPanel.Controls.Add(this.channelLeftGainTrack);
            this.stereoPanel.Controls.Add(this.channelRightGainTrack);
            this.stereoPanel.Controls.Add(this.boxStereo);
            this.stereoPanel.Location = new System.Drawing.Point(15, 122);
            this.stereoPanel.Name = "stereoPanel";
            this.stereoPanel.Size = new System.Drawing.Size(200, 107);
            this.stereoPanel.TabIndex = 20;
            this.stereoPanel.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Status: STEREO";
            // 
            // equalizerControl1
            // 
            this.equalizerControl1.Location = new System.Drawing.Point(9, 235);
            this.equalizerControl1.Name = "equalizerControl1";
            this.equalizerControl1.Size = new System.Drawing.Size(783, 343);
            this.equalizerControl1.TabIndex = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 598);
            this.Controls.Add(this.stereoPanel);
            this.Controls.Add(this.equalizerControl1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.volumeLabel);
            this.Controls.Add(this.labelSpeed);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.flowPanel);
            this.Controls.Add(this.front);
            this.Controls.Add(this.volume);
            this.Controls.Add(this.speedBar);
            this.Controls.Add(this.loopCheckBox);
            this.Controls.Add(this.speedLabelText);
            this.Controls.Add(this.labelVolumeText);
            this.Controls.Add(this.back);
            this.Controls.Add(this.AudioPosition);
            this.Controls.Add(this.TimePosition);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.flowPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.volume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AudioPosition)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelLeftGainTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelRightGainTrack)).EndInit();
            this.stereoPanel.ResumeLayout(false);
            this.stereoPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private FlowLayoutPanel flowPanel;
        private Button buttonPlay;
        private Button buttonStop;
        private TrackBar volume;
        private Timer timer;
        private TrackBar AudioPosition;
        private Label TimePosition;
        private Label A;
        private Label B;
        private Panel panel1;
        private CheckBox loopCheckBox;
        private Button front;
        private Button back;
        private TrackBar speedBar;
        private Label labelSpeed;
        private Label labelVolumeText;
        private Label speedLabelText;
        private Label volumeLabel;
        private Button browseButton;

        #endregion
        private Button resetButton;
        private PictureBox pictureBox1;
        private EqualizerControl equalizerControl1;
        private TrackBar channelLeftGainTrack;
        private TrackBar channelRightGainTrack;
        private CheckBox boxStereo;
        private Panel stereoPanel;
        private Label label4;
    }
}

