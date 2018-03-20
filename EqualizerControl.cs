using System;
using System.Windows.Forms;
using Equalization;

namespace NAudioPlayer
{
    public partial class EqualizerControl : UserControl
    {
        public Equalizer equalizer;
        public float[] Freqs = new float[] { 31.25f, 62.5f, 125f, 250f, 500f, 1000f, 2000f, 4000f, 8000f, 16000f };

        public EqualizerControl()
        {
            InitializeComponent();

            button1.Text = "Сброс";
            button2.Text = "Речь";

            labelFreq1.Text = convertor(Freqs[0]);
            labelFreq2.Text = convertor(Freqs[1]);
            labelFreq3.Text = convertor(Freqs[2]);
            labelFreq4.Text = convertor(Freqs[3]);
            labelFreq5.Text = convertor(Freqs[4]);
            labelFreq6.Text = convertor(Freqs[5]);
            labelFreq7.Text = convertor(Freqs[6]);
            labelFreq8.Text = convertor(Freqs[7]);
            labelFreq9.Text = convertor(Freqs[8]);
            labelFreq10.Text = convertor(Freqs[9]);

            UpdateLabels();
        }

        private string convertor(float n)
        {
            if (n < 1000)
                return n.ToString() + "Гц";
            else
                return (n / 1000).ToString() + "МГц";
        }


        public void UpdateLabels()
        {
            labelDbUp1.Text = trackBar0.Value.ToString() + " дБ";
            labelDbUp2.Text = trackBar1.Value.ToString() + " дБ";
            labelDbUp3.Text = trackBar2.Value.ToString() + " дБ";
            labelDbUp4.Text = trackBar3.Value.ToString() + " дБ";
            labelDbUp5.Text = trackBar4.Value.ToString() + " дБ";
            labelDbUp6.Text = trackBar5.Value.ToString() + " дБ";
            labelDbUp7.Text = trackBar6.Value.ToString() + " дБ";
            labelDbUp8.Text = trackBar7.Value.ToString() + " дБ";
            labelDbUp9.Text = trackBar8.Value.ToString() + " дБ";
            labelDbUp10.Text = trackBar9.Value.ToString() + " дБ";

            labelDbDown1.Text = trackBar10.Value.ToString() + " дБ";
            labelDbDown2.Text = trackBar11.Value.ToString() + " дБ";
            labelDbDown3.Text = trackBar12.Value.ToString() + " дБ";
            labelDbDown4.Text = trackBar13.Value.ToString() + " дБ";
            labelDbDown5.Text = trackBar14.Value.ToString() + " дБ";
            labelDbDown6.Text = trackBar15.Value.ToString() + " дБ";
            labelDbDown7.Text = trackBar16.Value.ToString() + " дБ";
            labelDbDown8.Text = trackBar17.Value.ToString() + " дБ";
            labelDbDown9.Text = trackBar18.Value.ToString() + " дБ";
            labelDbDown10.Text = trackBar19.Value.ToString() + " дБ";
        }

        public void UpdateTracks()
        {
            trackBar0.Value = (int)equalizer.Band0L;
            trackBar1.Value = (int)equalizer.Band1L;
            trackBar2.Value = (int)equalizer.Band2L;
            trackBar3.Value = (int)equalizer.Band3L;
            trackBar4.Value = (int)equalizer.Band4L;
            trackBar5.Value = (int)equalizer.Band5L;
            trackBar6.Value = (int)equalizer.Band6L;
            trackBar7.Value = (int)equalizer.Band7L;
            trackBar8.Value = (int)equalizer.Band8L;
            trackBar9.Value = (int)equalizer.Band9L;

            trackBar10.Value = (int)equalizer.Band0R;
            trackBar11.Value = (int)equalizer.Band1R;
            trackBar12.Value = (int)equalizer.Band2R;
            trackBar13.Value = (int)equalizer.Band3R;
            trackBar14.Value = (int)equalizer.Band4R;
            trackBar15.Value = (int)equalizer.Band5R;
            trackBar16.Value = (int)equalizer.Band6R;
            trackBar17.Value = (int)equalizer.Band7R;
            trackBar18.Value = (int)equalizer.Band8R;
            trackBar19.Value = (int)equalizer.Band9R;
        }

        public void SetEqualizerFromControl()
        {
            equalizer.Band0L = trackBar0.Value;
            equalizer.Band1L = trackBar1.Value;
            equalizer.Band2L = trackBar2.Value;
            equalizer.Band3L = trackBar3.Value;
            equalizer.Band4L = trackBar4.Value;
            equalizer.Band5L = trackBar5.Value;
            equalizer.Band6L = trackBar6.Value;
            equalizer.Band7L = trackBar7.Value;
            equalizer.Band8L = trackBar8.Value;
            equalizer.Band9L = trackBar9.Value;

            equalizer.Band0R = trackBar10.Value;
            equalizer.Band1R = trackBar11.Value;
            equalizer.Band2R = trackBar12.Value;
            equalizer.Band3R = trackBar13.Value;
            equalizer.Band4R = trackBar14.Value;
            equalizer.Band5R = trackBar15.Value;
            equalizer.Band6R = trackBar16.Value;
            equalizer.Band7R = trackBar17.Value;
            equalizer.Band8R = trackBar18.Value;
            equalizer.Band9R = trackBar19.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            equalizer.SetZeroAmps();
            UpdateTracks();
            UpdateLabels();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            equalizer.SetVoiceAmps();
            UpdateTracks();
            UpdateLabels();
        }

        private void trackBar0_Scroll(object sender, EventArgs e)
        {
            equalizer.Band0L = trackBar0.Value;
            labelDbUp1.Text = trackBar0.Value.ToString() + " дБ";
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            equalizer.Band1L = trackBar1.Value;
            labelDbUp2.Text = trackBar1.Value.ToString() + " дБ";
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            equalizer.Band2L = trackBar2.Value;
            
            labelDbUp3.Text = trackBar2.Value.ToString() + " дБ";
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            equalizer.Band3L = trackBar3.Value;
            
            labelDbUp4.Text = trackBar3.Value.ToString() + " дБ";
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            equalizer.Band4L = trackBar4.Value;
            
            labelDbUp5.Text = trackBar4.Value.ToString() + " дБ";
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            equalizer.Band5L = trackBar5.Value;
            
            labelDbUp6.Text = trackBar5.Value.ToString() + " дБ";
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            equalizer.Band6L = trackBar6.Value;
            
            labelDbUp7.Text = trackBar6.Value.ToString() + " дБ";
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            equalizer.Band7L = trackBar7.Value;
            
            labelDbUp8.Text = trackBar7.Value.ToString() + " дБ";
        }

        private void trackBar8_Scroll(object sender, EventArgs e)
        {
            equalizer.Band8L = trackBar8.Value;
            
            labelDbUp9.Text = trackBar8.Value.ToString() + " дБ";
        }

        private void trackBar9_Scroll(object sender, EventArgs e)
        {
            equalizer.Band9L = trackBar9.Value;
            
            labelDbUp10.Text = trackBar9.Value.ToString() + " дБ";
        }

        private void trackBar10_Scroll(object sender, EventArgs e)
        {
            equalizer.Band0R = trackBar10.Value;
            labelDbDown1.Text = trackBar10.Value.ToString() + " дБ";
        }

        private void trackBar11_Scroll(object sender, EventArgs e)
        {
            equalizer.Band1R = trackBar11.Value;
            labelDbDown2.Text = trackBar11.Value.ToString() + " дБ";
        }

        private void trackBar12_Scroll(object sender, EventArgs e)
        {
            equalizer.Band2R = trackBar12.Value;

            labelDbDown3.Text = trackBar12.Value.ToString() + " дБ";
        }

        private void trackBar13_Scroll(object sender, EventArgs e)
        {
            equalizer.Band3R = trackBar13.Value;

            labelDbDown4.Text = trackBar13.Value.ToString() + " дБ";
        }

        private void trackBar14_Scroll(object sender, EventArgs e)
        {
            equalizer.Band4R = trackBar14.Value;

            labelDbDown5.Text = trackBar14.Value.ToString() + " дБ";
        }

        private void trackBar15_Scroll(object sender, EventArgs e)
        {
            equalizer.Band5R = trackBar15.Value;

            labelDbDown6.Text = trackBar15.Value.ToString() + " дБ";
        }

        private void trackBar16_Scroll(object sender, EventArgs e)
        {
            equalizer.Band6R = trackBar16.Value;

            labelDbDown7.Text = trackBar16.Value.ToString() + " дБ";
        }

        private void trackBar17_Scroll(object sender, EventArgs e)
        {
            equalizer.Band7R = trackBar17.Value;

            labelDbDown8.Text = trackBar17.Value.ToString() + " дБ";
        }

        private void trackBar18_Scroll(object sender, EventArgs e)
        {
            equalizer.Band8R = trackBar18.Value;

            labelDbDown9.Text = trackBar18.Value.ToString() + " дБ";
        }

        private void trackBar19_Scroll(object sender, EventArgs e)
        {
            equalizer.Band9R = trackBar19.Value;

            labelDbDown10.Text = trackBar19.Value.ToString() + " дБ";
        }
    }
}
