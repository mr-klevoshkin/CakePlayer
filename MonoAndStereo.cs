using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;

namespace NAudioPlayer
{
    public enum MonoAndStereoMode
    {
        None,
        MonoToStereo,
        StereoToMono,
    }

    public class MonoAndStereo : ISampleProvider
    {
        private MonoToStereoSampleProvider monoToStereoProvider;
        private StereoToMonoSampleProvider stereoToMonoProvider;
        private ISampleProvider sourceProvider;

        private MonoAndStereoMode monoAndStereoMode;

        public MonoAndStereo(ISampleProvider sourceProvider)
        {
            this.sourceProvider = sourceProvider;
            this.MonoAndStereoMode = MonoAndStereoMode.None;
            if (sourceProvider.WaveFormat.Channels == 1)
            {
                this.monoToStereoProvider = new MonoToStereoSampleProvider(sourceProvider);
            }
            else
            {
                this.stereoToMonoProvider = new StereoToMonoSampleProvider(sourceProvider);
            }           
        }

        public WaveFormat WaveFormat
        {
            get
            {
                return CurrentSampleProvider.WaveFormat;
            }
        }

        public MonoAndStereoMode MonoAndStereoMode
        {
            get
            {
                return monoAndStereoMode;
            }
            set
            {
                switch (value)
                {
                    case MonoAndStereoMode.MonoToStereo:
                        if (monoToStereoProvider == null)
                            throw new ArgumentException();
                        break;
                    case MonoAndStereoMode.StereoToMono:
                        if (stereoToMonoProvider == null)
                            throw new ArgumentException();
                        break;
                    case MonoAndStereoMode.None:
                        break;
                }

                monoAndStereoMode = value;
            }
        }

        /// <summary>
        /// Multiplier for left channel (default is 1.0)
        /// </summary>
        public float LeftVolume {
            get
            {
                if (MonoAndStereoMode == MonoAndStereoMode.MonoToStereo)
                    return monoToStereoProvider.LeftVolume;
                else if (monoAndStereoMode == MonoAndStereoMode.StereoToMono)
                    return stereoToMonoProvider.LeftVolume;
                else
                    return 1f;
            }
            set
            {
                if (MonoAndStereoMode == MonoAndStereoMode.MonoToStereo)
                    monoToStereoProvider.LeftVolume = value;
                else if (monoAndStereoMode == MonoAndStereoMode.StereoToMono)
                    stereoToMonoProvider.LeftVolume = value;
            }
        }

        /// <summary>
        /// Multiplier for right channel (default is 1.0)
        /// </summary>
        public float RightVolume
        {
            get
            {
                if (MonoAndStereoMode == MonoAndStereoMode.MonoToStereo)
                    return monoToStereoProvider.RightVolume;
                else if (monoAndStereoMode == MonoAndStereoMode.StereoToMono)
                    return stereoToMonoProvider.RightVolume;
                else
                    return 1f;
            }
            set
            {
                if (MonoAndStereoMode == MonoAndStereoMode.MonoToStereo)
                    monoToStereoProvider.RightVolume = value;
                else if (monoAndStereoMode == MonoAndStereoMode.StereoToMono)
                    stereoToMonoProvider.RightVolume = value;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            return CurrentSampleProvider.Read(buffer, offset, count);
        }

        private ISampleProvider CurrentSampleProvider
        {
            get
            {
                switch (monoAndStereoMode)
                {
                    case MonoAndStereoMode.None:
                        return sourceProvider;
                    case MonoAndStereoMode.MonoToStereo:
                        return monoToStereoProvider;
                    case MonoAndStereoMode.StereoToMono:
                        return stereoToMonoProvider;
                    default:
                        throw new Exception("problems...");
                }
            }
        }
    }
}
