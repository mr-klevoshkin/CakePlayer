using System;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;

namespace Visualization
{
    public class Vistalizator : IDisposable
    {
        public ImageSettings imageSettings;
        private ISampleProvider provider;
        private WaveStream stream;
        private Bitmap backgroundImage;
        private Image image;


        private int loopBegin;
        private int loopLength;
        private bool DontMoveRightBorder = false;
        private TimeSpan loopBeginTime;
        private bool IsLooped;

        private static WaveFormRenderer waveFormRenderer = new WaveFormRenderer();
        private static WaveFormRendererSettings standardSettings = new StandardWaveFormRendererSettings() { Name = "Standard" };
        private static SoundCloudOriginalSettings soundcloudOriginalSettings = new SoundCloudOriginalSettings() { Name = "SoundCloud Original" };
        private static SoundCloudBlockWaveFormSettings soundCloudLightBlocks = new SoundCloudBlockWaveFormSettings(Color.FromArgb(102, 102, 102), Color.FromArgb(103, 103, 103), Color.FromArgb(179, 179, 179),
                Color.FromArgb(218, 218, 218))
        { Name = "SoundCloud Light Blocks" };
        private static SoundCloudBlockWaveFormSettings soundCloudDarkBlocks = new SoundCloudBlockWaveFormSettings(Color.FromArgb(52, 52, 52), Color.FromArgb(55, 55, 55), Color.FromArgb(154, 154, 154),
                Color.FromArgb(204, 204, 204))
        { Name = "SoundCloud Darker Blocks" };
        private static SoundCloudBlockWaveFormSettings soundCloudOrangeBlocks = new SoundCloudBlockWaveFormSettings(Color.FromArgb(255, 76, 0), Color.FromArgb(255, 52, 2), Color.FromArgb(255, 171, 141),
                Color.FromArgb(255, 213, 199))
        { Name = "SoundCloud Orange Blocks" };
        private static SoundCloudBlockWaveFormSettings soundCloudOrangeTransparentBlocks = new SoundCloudBlockWaveFormSettings(Color.FromArgb(196, 197, 53, 0), Color.FromArgb(64, 83, 22, 3), Color.FromArgb(196, 79, 26, 0),
                Color.FromArgb(64, 79, 79, 79))
        {
            Name = "SoundCloud Orange Transparent Blocks",
            PixelsPerPeak = 2,
            SpacerPixels = 1,
            TopSpacerGradientStartColor = Color.FromArgb(64, 83, 22, 3),
             BackgroundColor = Color.Transparent
        };
        private static SoundCloudBlockWaveFormSettings soundCloudGrayTransparentBlocks = new SoundCloudBlockWaveFormSettings(Color.FromArgb(196, 224, 225, 224), Color.FromArgb(64, 224, 224, 224), Color.FromArgb(196, 128, 128, 128),
                Color.FromArgb(64, 128, 128, 128))
        {
            Name = "SoundCloud Gray Transparent Blocks",
            PixelsPerPeak = 2,
            SpacerPixels = 1,
            TopSpacerGradientStartColor = Color.FromArgb(64, 224, 224, 224),
            BackgroundColor = Color.Transparent
        };



        public Vistalizator(WaveStream waveStream, ImageSettings imageSettings)
        {
            this.imageSettings = imageSettings;

            stream = waveStream;
            //stream.Seek(2000, System.IO.SeekOrigin.Begin);
            provider = WaveExtensionMethods.ToSampleProvider(stream);
            backgroundImage = new Bitmap(imageSettings.Size.Width, imageSettings.Size.Height);
            using (var graphics = Graphics.FromImage(backgroundImage))
                graphics.Clear(Color.LightGray);
            loopBegin = 0;
            loopLength = (int)waveStream.Length;
            loopBeginTime = TimeSpan.Zero;
            IsLooped = false;
            image = null;


            //stream = new WaveStreamReader(waveStream, waveStream.WaveFormat, 0);
            //provider = WaveExtensionMethods.ToSampleProvider(stream);

            //comboBoxPeakCalculationStrategy.Items.Add("Max Absolute Value");
            //comboBoxPeakCalculationStrategy.Items.Add("Max Rms Value");
            //comboBoxPeakCalculationStrategy.Items.Add("Sampled Peaks");
            //comboBoxPeakCalculationStrategy.Items.Add("Scaled Average");
            //comboBoxPeakCalculationStrategy.SelectedIndex = 0;
            //comboBoxPeakCalculationStrategy.SelectedIndexChanged += (sender, args) => RenderWaveform();

        }

        public object[] GetRenderVariants()
        {
             object[] variants = 
             {
                standardSettings,
                soundcloudOriginalSettings,
                soundCloudLightBlocks,
                soundCloudDarkBlocks,
                soundCloudOrangeBlocks,
                soundCloudOrangeTransparentBlocks,
                soundCloudGrayTransparentBlocks
            };
            return variants;
        }

        private IPeakProvider GetPeakProvider(int flag)
        {
             switch (flag)
            {
                case 0:
                    return new MaxPeakProvider();
                case 1:
                    return new RmsPeakProvider(imageSettings.Size.Height);
                case 2:
                    return new SamplingPeakProvider(imageSettings.Size.Height);
                case 3:
                    return new AveragePeakProvider(4);
                default:
                    throw new InvalidOperationException("Unknown calculation strategy");
            }
        }

        public Image GetImage()
        {
            var settings = standardSettings;
            settings.BackgroundImage = backgroundImage;
            settings.TopHeight = imageSettings.TopHeight;
            settings.BottomHeight = imageSettings.BottomHeight;
            settings.Width = imageSettings.Size.Width;
            settings.DecibelScale = imageSettings.DecibelScale;
            var peakProvider = GetPeakProvider(1);
            try
            {
                image = waveFormRenderer.Render(stream, provider, peakProvider, settings);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                stream.Position = 0;
            }
            return image;
        }

        private int TimeSpanToPixels(TimeSpan time)
        {
            int posSamples = (int)time.TotalSeconds * stream.WaveFormat.SampleRate; // Position in Samples

            int bytesPerSample = (stream.WaveFormat.BitsPerSample / 8);
            var samples = stream.Length / (bytesPerSample);
            var samplesPerPixel = (int)(samples / imageSettings.Size.Width);

            return posSamples / samplesPerPixel;   
        }

        private void SetLoop(int Begin, int Length)
        {
            loopBegin = Begin;
            loopLength = Length;
            Graphics.FromImage(backgroundImage).FillRectangle(Brushes.SkyBlue, Begin, 0, Length, imageSettings.Size.Height);
            IsLooped = true;
        }

        private void SetLoopTime(TimeSpan Begin, TimeSpan Length)
        {
            loopBeginTime = Begin;
            Graphics.FromImage(backgroundImage).FillRectangle(Brushes.SkyBlue, TimeSpanToPixels(Begin), 0, TimeSpanToPixels(Length), imageSettings.Size.Height);
            IsLooped = true;
        }

        public Image UnsetLoopBG()
        {
            using (var graphics = Graphics.FromImage(backgroundImage))
                graphics.FillRectangle(Brushes.LightGray, 0, 0, imageSettings.Size.Width, imageSettings.Size.Height);
            IsLooped = false;
            return GetImage();
        }

        public Image SetLoopBG(int newLoopBegin, int newLength) // In Pixels
        {
            if (!IsLooped)
            {
                SetLoop(newLoopBegin, newLength);
                loopBegin = newLoopBegin;
                loopLength = newLength;
                return GetImage();
            }
            else
            {
                if (newLength != loopLength)
                {
                    if(DontMoveRightBorder)
                    {
                        if (newLoopBegin > loopBegin)
                            LeftBorderMovesRight(newLoopBegin - loopBegin);
                        else if (newLoopBegin < loopBegin)
                            LeftBorderMovesLeft(loopBegin - newLoopBegin);
                    }
                    else
                    {
                        if (newLength > loopLength)
                            RightBorderMovesRight(newLength - loopLength);
                        else if (newLength < loopLength)
                            RightBorderMovesLeft(loopLength - newLength);
                    }
                    loopBegin = newLoopBegin;
                    loopLength = newLength;
                    return GetImage();
                }
                else
                    return image;
            }
            
        }
        public Image SetLoopBG(TimeSpan newLoopBegin, TimeSpan newLength) // In Time
        {
            if (newLoopBegin != loopBeginTime)
                DontMoveRightBorder = true;
            else
                DontMoveRightBorder = false;
            loopBeginTime = newLoopBegin;
            return SetLoopBG(TimeSpanToPixels(newLoopBegin), TimeSpanToPixels(newLength));
        }

        private void LeftBorderMovesLeft(int delta)
        {
            if (delta > 1)
                Graphics.FromImage(backgroundImage).FillRectangle(Brushes.SkyBlue, loopBegin - delta, 0, delta, imageSettings.Size.Height);
        }
        private void LeftBorderMovesRight(int delta)
        {
            if (delta > 1)
                Graphics.FromImage(backgroundImage).FillRectangle(Brushes.LightGray, loopBegin, 0, delta, imageSettings.Size.Height);
        }
        private void RightBorderMovesRight(int delta)
        {
            if (delta > 1)
                Graphics.FromImage(backgroundImage).FillRectangle(Brushes.SkyBlue, loopBegin + loopLength, 0, delta, imageSettings.Size.Height);
        }
        private void RightBorderMovesLeft(int delta)
        {
            if (delta > 1)
                Graphics.FromImage(backgroundImage).FillRectangle(Brushes.LightGray, loopBegin + loopLength - delta, 0, delta, imageSettings.Size.Height);
        }
        public void Dispose()
        {
            stream.Dispose();
        }
    }

    public struct ImageSettings
    {
        public Size Size;
        public int TopHeight;
        public int BottomHeight;
        public bool DecibelScale;
    }
}
