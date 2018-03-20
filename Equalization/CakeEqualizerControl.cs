using System;
using System.Windows.Forms;
using NAudioPlayer;

namespace Equalization
{
    public partial class EqualizerControl : UserControl
    {
        public CakeProvider cakeFile;
        public float[] Freqs = new float[] { 31.25f, 62.5f, 125f, 250f, 500f, 1000f, 2000f, 4000f, 8000f, 16000f };

        public EqualizerControl()
        {
            InitializeComponent();

            button1.Text = "Сброс";
            button2.Text = "Речь";

            label2.Text = Freqs[0].ToString() + " Гц";
            label3.Text = Freqs[1].ToString() + " Гц";
            label4.Text = Freqs[2].ToString() + " Гц";
            label5.Text = Freqs[3].ToString() + " Гц";
            label6.Text = Freqs[4].ToString() + " Гц";
            label7.Text = Freqs[5].ToString() + " Гц";
            label8.Text = Freqs[6].ToString() + " Гц";
            label9.Text = Freqs[7].ToString() + " Гц";
            label10.Text = Freqs[8].ToString() + " Гц";
            label11.Text = Freqs[9].ToString() + " Гц";

            UpdateLabels();
        }

        private void UpdateLabels()
        {
            label13.Text = trackBar0.Value.ToString() + " дБ";
            label14.Text = trackBar1.Value.ToString() + " дБ";
            label15.Text = trackBar2.Value.ToString() + " дБ";
            label16.Text = trackBar3.Value.ToString() + " дБ";
            label17.Text = trackBar4.Value.ToString() + " дБ";
            label18.Text = trackBar5.Value.ToString() + " дБ";
            label19.Text = trackBar6.Value.ToString() + " дБ";
            label20.Text = trackBar7.Value.ToString() + " дБ";
            label21.Text = trackBar8.Value.ToString() + " дБ";
            label22.Text = trackBar9.Value.ToString() + " дБ";
        }

        private void UpdateTracks()
        {
            trackBar0.Value = (int)cakeFile.Band1;
            trackBar1.Value = (int)cakeFile.Band2;
            trackBar2.Value = (int)cakeFile.Band3;
            trackBar3.Value = (int)cakeFile.Band4;
            trackBar4.Value = (int)cakeFile.Band5;
            trackBar5.Value = (int)cakeFile.Band6;
            trackBar6.Value = (int)cakeFile.Band7;
            trackBar7.Value = (int)cakeFile.Band8;
            trackBar8.Value = (int)cakeFile.Band9;
            trackBar9.Value = (int)cakeFile.Band10;
        }

        public void SetEqualizerFromControl()
        {
            cakeFile.Band1 = trackBar0.Value;
            cakeFile.Band3 = trackBar1.Value;
            cakeFile.Band3 = trackBar2.Value;
            cakeFile.Band4 = trackBar3.Value;
            cakeFile.Band5 = trackBar4.Value;
            cakeFile.Band6 = trackBar5.Value;
            cakeFile.Band7 = trackBar6.Value;
            cakeFile.Band8 = trackBar7.Value;
            cakeFile.Band9 = trackBar8.Value;
            cakeFile.Band10 = trackBar9.Value;
        }

 

        private void trackBar0_Scroll(object sender, EventArgs e)
        {
            cakeFile.Band1 = trackBar0.Value;
            label13.Text = trackBar0.Value.ToString() + " дБ";
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            cakeFile.Band2 = trackBar1.Value;
            label14.Text = trackBar1.Value.ToString() + " дБ";
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            cakeFile.Band3 = trackBar2.Value;
            
            label15.Text = trackBar2.Value.ToString() + " дБ";
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            cakeFile.Band4 = trackBar3.Value;
            
            label16.Text = trackBar3.Value.ToString() + " дБ";
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            cakeFile.Band5 = trackBar4.Value;
            
            label17.Text = trackBar4.Value.ToString() + " дБ";
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            cakeFile.Band6 = trackBar5.Value;
            
            label18.Text = trackBar5.Value.ToString() + " дБ";
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            cakeFile.Band7 = trackBar6.Value;
            
            label19.Text = trackBar6.Value.ToString() + " дБ";
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            cakeFile.Band8 = trackBar7.Value;
            
            label20.Text = trackBar7.Value.ToString() + " дБ";
        }

        private void trackBar8_Scroll(object sender, EventArgs e)
        {
            cakeFile.Band9 = trackBar8.Value;
            
            label21.Text = trackBar8.Value.ToString() + " дБ";
        }

        private void trackBar9_Scroll(object sender, EventArgs e)
        {
            cakeFile.Band10 = trackBar9.Value;
            
            label22.Text = trackBar9.Value.ToString() + " дБ";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cakeFile.ResetEqualizer();
            UpdateTracks();
            UpdateLabels();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cakeFile.SetVoiceAmps();
            UpdateTracks();
            UpdateLabels();
        }
    }
}
