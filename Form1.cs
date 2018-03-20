using System;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;
using Visualization;

namespace NAudioPlayer
{
    public partial class Form1 : Form, IDisposable
    {
        private AudioFileReader audioFile;
        private AudioFileReader imageStream;
        private CakePlayer player;

        private bool WaitingNewPosition = false;
        private int AB;
        private TimeSpan timeA;
        private TimeSpan timeB;

        public Form1()
        {
            InitializeComponent();

            player = new CakePlayer();
            AB = B.Location.X - A.Location.X;
        }

        private void InitFile()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "WAV Files|*.wav|MP3 Files|*.mp3";
            string fileName = ofd.ShowDialog() == DialogResult.OK ? ofd.FileName : null;
            if (fileName == null)
                audioFile = null;
            else
            {
                audioFile = new AudioFileReader(fileName);
                imageStream = new AudioFileReader(fileName);
            }

            if (audioFile != null)
            {
                if (player.IsPlaying)
                    player.Close();
                player.Open(audioFile);
                player.DrawSignal(imageStream, pictureBox1);

                volume.Value = player.Volume;
                AudioPosition.Minimum = 0;
                AudioPosition.Maximum = (int)player.Length;
                AudioPosition.TickFrequency = AudioPosition.Maximum;
                volumeLabel.Text = volume.Value.ToString() + " %";
                TimePosition.Text = "00:00/" + player.TotalTime.Minutes.ToString() + ':' + player.TotalTime.Seconds.ToString();

                timeA = TimeSpan.FromMilliseconds(0);
                timeB = TimeSpan.FromMilliseconds(player.TotalTime.TotalMilliseconds);

                equalizerControl1.equalizer = player.initializeEqulizerControl();

                if (player.Channel == 2)
                    stereoPanel.Show();
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (!player.IsDataAvailable)
                InitFile();
            if (!player.IsPlaying)
            {
                player.Play();
                timer.Enabled = true;
                buttonPlay.Text = "Pause";
            }
            else
            {
                player.Pause();
                timer.Enabled = false;
                buttonPlay.Text = "Play";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitFile();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            if (player.Position != -1)
                AudioPosition.Value = (int)(player.Position * player.Length);
            else
                AudioPosition.Value = 0;
            TimePosition.Text = player.Time.ToString("mm\\:ss") + "/" + player.TotalTime.ToString("mm\\:ss");
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            buttonPlay.Text = "Play";
            player?.Stop();
            timer.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (player != null)
            {
                player.Dispose();
                audioFile?.Dispose();
                audioFile = null;
                imageStream.Dispose();
            }
        }

        private void volume_Scroll(object sender, EventArgs e)
        {
            if (player != null)
            {
                player.Volume = volume.Value;
                volumeLabel.Text = (volume.Value).ToString() + " %";
            }
        }

        private void AudioPosition_Scroll(object sender, EventArgs e)
        {
            var value = AudioPosition.Value;
            WaitingNewPosition = true;
            timer.Enabled = false;
        }

        private void AudioPosition_MouseUp(object sender, MouseEventArgs e)
        {
            if (WaitingNewPosition)
            {
                WaitingNewPosition = false;
                player.Position = (float)AudioPosition.Value / AudioPosition.Maximum;
                timer.Enabled = true;
            }
        }

        bool isDown;
        private void A_MouseDown(object sender, MouseEventArgs e)
        {
            isDown = true;
        }
        private void A_MouseMove(object sender, MouseEventArgs e)
        {

            Control c = sender as Control;
            if (isDown)
            {
                if (PointToClient(Control.MousePosition).X - panel1.Location.X > 5 + A.Size.Width / 2)
                    c.Location = new Point(PointToClient(Control.MousePosition).X - panel1.Location.X - A.Size.Width / 2, A.Location.Y);
                timeA = TimeSpan.FromMilliseconds((A.Location.X * player.TotalTime.TotalMilliseconds / AB));
                loopCheckBox.Text = timeA.ToString("mm\\:ss") + " - " + timeB.ToString("mm\\:ss");
                if (loopCheckBox.Checked)
                    player.SetPlaybackLoop(timeA, timeB);
            }
        }
        private void A_MouseUp(object sender, MouseEventArgs e)
        {
            isDown = false;
        }
        private void B_MouseDown(object sender, MouseEventArgs e)
        {
            isDown = true;
        }
        private void B_MouseMove(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;
            if (isDown)
            {
                if (PointToClient(Control.MousePosition).X - panel1.Location.X < 5 + AB + B.Size.Width / 2)
                    c.Location = new Point(PointToClient(Control.MousePosition).X - panel1.Location.X - B.Size.Width / 2, A.Location.Y);
{
                    timeB = TimeSpan.FromMilliseconds((B.Location.X * player.TotalTime.TotalMilliseconds / AB) - 1);
                    loopCheckBox.Text = timeA.ToString("mm\\:ss") + " - " + timeB.ToString("mm\\:ss");
                    if (loopCheckBox.Checked)
                        player.SetPlaybackLoop(timeA, timeB);
                }
            }
        }
        private void B_MouseUp(object sender, MouseEventArgs e)
        {
            isDown = false;
        }

        private void loop_CheckedChanged(object sender, EventArgs e)
        {

            if (loopCheckBox.Checked)
                player.SetPlaybackLoop(timeA, timeB);
            else
                player.UnsetPlaybackLoop();
        }

        private void back_Click(object sender, EventArgs e)
        {
            player.Backward();
        }
        private void front_Click(object sender, EventArgs e)
        {
            player.Forward();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            player.Rate = 0.5f + speedBar.Value * 0.1f;
            labelSpeed.Text = $"x{player.Rate:F2}"; 
        }


        private void boxStereoMono_CheckedChanged(object sender, EventArgs e)
        {
            if (boxStereo.Checked)
            {
                player.Channel = 1;
                player.LeftVolume = channelLeftGainTrack.Value / 10f;
                player.RightVolume = channelRightGainTrack.Value / 10f;
                boxStereo.Text = "Status: MONO";
            }
            else
            {
                player.Channel = 2;
                player.LeftVolume = 1f;
                player.RightVolume = 1f;
                label4.Text = "Status: STEREO";
            }
        }

        private void Reset()
        {
            labelSpeed.Text = $"x{player.Rate:F2}";
            equalizerControl1.UpdateTracks();
            equalizerControl1.UpdateLabels();
            boxStereo.Checked = false;

            loopCheckBox.Text = "A - B";
            A.Location = new Point(5, 3);
            B.Location = new Point(5 + AB, 3);
            loopCheckBox.Checked = false;
            speedBar.Value = 5;

            channelLeftGainTrack.Value = 10;
            channelRightGainTrack.Value = 10;
        }
        private void resetButton_Click(object sender, EventArgs e)
        {
            player.Reset();
            Reset();
        }

        private void channelLeftGainTrack_Scroll(object sender, EventArgs e)
        {
            player.LeftVolume = channelLeftGainTrack.Value / 10f;
        }

        private void channelRightGainTrack_Scroll(object sender, EventArgs e)
        {
            player.RightVolume = channelRightGainTrack.Value / 10f;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {

        }
    }
}