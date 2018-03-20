using NAudio.Dsp;
using NAudio.Wave;
using System;

namespace Equalization
{
    /// <summary>
    /// Basic example of a multi-band eq
    /// uses the same settings for both channels in stereo audio
    /// Call Update after you've updated the bands
    /// Potentially to be added to NAudio in a future version
    /// </summary>
    public class Equalizer : ISampleProvider
    {
        public float[] Freqs = new float[] { 31.25f, 62.5f, 125f, 250f, 500f, 1000f, 2000f, 4000f, 8000f, 16000f };
        private static EqualizerBand[] zeroBands = new EqualizerBand[]
        {
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 31.25f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 62.5f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 125f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 250f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 500f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 1000f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 2000f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 4000f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 8000f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 16000f, GainL = 0, GainR = 0 },
        };

        private static EqualizerBand[] voiceBands = new EqualizerBand[]
        {
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 31.25f, GainL = -10f, GainR = -10f },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 62.5f, GainL = -10f, GainR = -10f },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 125f, GainL = -10f, GainR = -10f },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 250f, GainL = -5f, GainR = -5f },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 500f, GainL = 0f, GainR = 0f },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 1000f, GainL = 7f, GainR = 7F },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 2000f, GainL = 10f, GainR = 10f },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 4000f, GainL = 10f, GainR = 10f },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 8000f, GainL = 7f, GainR = 7f },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 16000f, GainL = 0f, GainR = 0f },
        };

        private EqualizerBand[] bands = new EqualizerBand[]
        {
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 31.25f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 62.5f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 125f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 250f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 500f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 1000f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 2000f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 4000f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 8000f, GainL = 0, GainR = 0 },
            new EqualizerBand {Bandwidth = 0.8f, Frequency = 16000f, GainL = 0, GainR = 0 },
        };

        private float[,] amps;
        private readonly ISampleProvider sourceProvider;
        private readonly BiQuadFilter[,] filters;
        private readonly int channels;

        private readonly int bandCount = 10; // bands for 1 channel
        private bool updated;
        private bool doubleMode;

        public Equalizer(ISampleProvider sourceProvider)
        {
            channels = sourceProvider.WaveFormat.Channels;

            this.sourceProvider = sourceProvider;
            filters = new BiQuadFilter[channels, bands.Length];
            CreateFilters();

            doubleMode = true;

            amps = new float[2, bands.Length];
            amps[0, 0] = Band0L;
            amps[0, 1] = Band1L;
            amps[0, 2] = Band2L;
            amps[0, 3] = Band3L;
            amps[0, 4] = Band4L;
            amps[0, 5] = Band5L;
            amps[0, 6] = Band6L;
            amps[0, 7] = Band7L;
            amps[0, 8] = Band8L;
            amps[0, 9] = Band9L;
            amps[1, 0] = Band0R;
            amps[1, 1] = Band1R;
            amps[1, 2] = Band2R;
            amps[1, 3] = Band3R;
            amps[1, 4] = Band4R;
            amps[1, 5] = Band5R;
            amps[1, 6] = Band6R;
            amps[1, 7] = Band7R;
            amps[1, 8] = Band8R;
            amps[1, 9] = Band9R;
        }

        public bool DoubleMode
        {
            get { return doubleMode; }
            set { doubleMode = value; }
        }

        private void CreateFilters(int b = -1, int ch = -1)
        {
            if (b == -1 && ch == -1)
            {
                for (int n = 0; n < channels; n++)
                {
                    bool L = (n % 2 == 0);
                    float Gain;
                    for (int bandIndex = 0; bandIndex < bands.Length; bandIndex++)
                    {
                        var band = bands[bandIndex];
                        //попробуй using
                        if (L)
                            Gain = band.GainL;
                        else
                            Gain = band.GainR;

                        if (filters[n, bandIndex] == null)
                            filters[n, bandIndex] = BiQuadFilter.PeakingEQ(sourceProvider.WaveFormat.SampleRate, band.Frequency, band.Bandwidth, Gain);
                        else
                            filters[n, bandIndex].SetPeakingEq(sourceProvider.WaveFormat.SampleRate, band.Frequency, band.Bandwidth, Gain);
                    }
                }
            }
            else
            {
                float Gain;
                var band = bands[b];
                if (ch % 2 == 0)
                    Gain = band.GainL;
                else
                    Gain = band.GainR;
                filters[ch, b].SetPeakingEq(sourceProvider.WaveFormat.SampleRate, band.Frequency, band.Bandwidth, Gain);
            }

        }

        public bool IsEqualized()
        {
            bool res = false;
            foreach (EqualizerBand band in bands)
                if (band.Gain != 0f)
                    res = true;
            return res;          
        }

        public EqualizerBand[] GetAmps()
        {
            return bands;
        }

        public void SetAmps(EqualizerBand[] newBands)
        {
            if (newBands.Length != bandCount)
                throw new ArgumentException("Lenght of bands should be " + bandCount.ToString());
            for (int i = 0; i < bandCount; i++)
                bands[i].Gain = newBands[i].Gain;
        }

        public float GetBandAmp(int num, string flag = "both")
        {
            switch (flag)
            {
                case "both":
                    return bands[num].Gain;
                case "left":
                    return amps[0, num];
                case "right":
                    return amps[1, num];
                default:
                    throw new Exception("flag has to be 'left', 'rigth' or 'both' !!!");
            }
        }

        public void SetBandAmp(int num, float amp, string flag = "both")
        {
            switch (flag)
            {
                case "both":
                    amps[0, num] = amp;
                    amps[1, num] = amp;
                    break;
                case "left":
                    amps[0, num] = amp;
                    break;
                case "right":
                    amps[1, num] = amp;
                    break;
                default:
                    throw new Exception("flag has to be 'left', 'rigth' or 'both' !!!");
            }
        }

        public void SetVoiceAmps()
        {
            for (int i = 0; i < bands.Length; i++)
                bands[i].Gain = voiceBands[i].Gain;
            Update();
        }

        public void SetZeroAmps()
        {
            for (int i = 0; i < bands.Length; i++)
                bands[i].Gain = zeroBands[i].Gain;
            Update();
        }

        public void Update(int b = -1, int ch = -1)
        {
            updated = true;
            CreateFilters();
        }

        public WaveFormat WaveFormat { get { return sourceProvider.WaveFormat; } }

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = sourceProvider.Read(buffer, offset, count);

            if (updated)
            {
                CreateFilters();
                updated = false;
            }

            for (int n = 0; n < samplesRead; n++)
            {
                int ch = n % channels;
                for (int band = 0; band < bandCount; band++)
                    buffer[offset + n] = filters[ch, band].Transform(buffer[offset + n]);
            }
            return samplesRead;
        }

        public float MinimumGain //in decibels

        {
            get { return -20; }
        }

        public float MaximumGain //in decibels
        {
            get { return 20; }
        }

        public float Band0L
        {
            get { return bands[0].GainL; }
            set
            {
                if (bands[0].GainL != value)
                {
                    bands[0].GainL = value;
                    Update(0,0);
                }
            }
        }

        public float Band0R
        {
            get { return bands[0].GainR; }
            set
            {
                if (bands[0].GainR != value)
                {
                    bands[0].GainR = value;
                    Update(0,1);
                }
            }
        }
        public float Band1L
        {
            get { return bands[1].GainL; }
            set
            {
                if (bands[1].GainL != value)
                {
                    bands[1].GainL = value;
                    Update(1,0);
                }
            }
        }
        public float Band1R
        {
            get { return bands[1].GainR; }
            set
            {
                if (bands[1].GainR != value)
                {
                    bands[1].GainR = value;
                    Update(1,1);
                }
            }
        }
        public float Band2L
        {
            get { return bands[2].GainL; }
            set
            {
                if (bands[2].GainL != value)
                {
                    bands[2].GainL = value;
                    Update(2,0);
                }
            }
        }
        public float Band2R
        {
            get { return bands[2].GainR; }
            set
            {
                if (bands[2].GainR != value)
                {
                    bands[2].GainR = value;
                    Update(2,1);
                }
            }
        }
        public float Band3L
        {
            get { return bands[3].GainL; }
            set
            {
                if (bands[3].GainL != value)
                {
                    bands[3].GainL = value;
                    Update(3,0);
                }
            }
        }
        public float Band3R
        {
            get { return bands[3].GainR; }
            set
            {
                if (bands[3].GainR != value)
                {
                    bands[3].GainR = value;
                    Update(3,1);
                }
            }
        }
        public float Band4L
        {
            get { return bands[4].GainL; }
            set
            {
                if (bands[4].GainL != value)
                {
                    bands[4].GainL = value;
                    Update(4,0);
                }
            }
        }
        public float Band4R
        {
            get { return bands[4].GainR; }
            set
            {
                if (bands[4].GainR != value)
                {
                    bands[4].GainR = value;
                    Update(4,1);
                }
            }
        }
        public float Band5L
        {
            get { return bands[5].GainL; }
            set
            {
                if (bands[5].GainL != value)
                {
                    bands[5].GainL = value;
                    Update(5,1);
                }
            }
        }
        public float Band5R
        {
            get { return bands[5].GainR; }
            set
            {
                if (bands[5].GainR != value)
                {
                    bands[5].GainR = value;
                    Update(5,1);
                }
            }
        }
        public float Band6L
        {
            get { return bands[6].GainL; }
            set
            {
                if (bands[6].GainL != value)
                {
                    bands[6].GainL = value;
                    Update(6,0);
                }
            }
        }
        public float Band6R
        {
            get { return bands[6].GainR; }
            set
            {
                if (bands[6].GainR != value)
                {
                    bands[6].GainR = value;
                    Update(6,1);
                }
            }
        }
        public float Band7L
        {
            get { return bands[7].GainL; }
            set
            {
                if (bands[7].GainL != value)
                {
                    bands[7].GainL = value;
                    Update(7,0);
                }
            }
        }
        public float Band7R
        {
            get { return bands[7].GainR; }
            set
            {
                if (bands[7].GainR != value)
                {
                    bands[7].GainR = value;
                    Update(7,1);
                }
            }
        }
        public float Band8L
        {
            get { return bands[8].GainL; }
            set
            {
                if (bands[8].GainL != value)
                {
                    bands[8].GainL = value;
                    Update(8,0);
                }
            }
        }
        public float Band8R
        {
            get { return bands[8].GainR; }
            set
            {
                if (bands[8].GainR != value)
                {
                    bands[8].GainR = value;
                    Update(8,1);
                }
            }
        }
        public float Band9L
        {
            get { return bands[9].GainL; }
            set
            {
                if (bands[9].GainL != value)
                {
                    bands[9].GainL = value;
                    Update(9,0);
                }
            }
        }
        public float Band9R
        {
            get { return bands[9].GainR; }
            set
            {
                if (bands[9].GainR != value)
                {
                    bands[9].GainR = value;
                    Update(9,1);
                }
            }
        }
    }
}