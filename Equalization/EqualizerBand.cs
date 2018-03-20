namespace Equalization
{
    public class EqualizerBand
    {
        public float Frequency { get; set; }
        public float GainL { get; set; }
        public float GainR { get; set; }
        public float Bandwidth { get; set; }
        public float Gain
        {
            get { return GainL + GainR; }
            set { GainL = GainR = value; }
        }
    }
}