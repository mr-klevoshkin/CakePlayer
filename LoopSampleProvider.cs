using System;
using NAudio.Wave;

namespace NAudioPlayer
{
    public class LoopSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider sampleProvider;
        private readonly WaveStream waveStream;
        private int phase; // 0 = not started yet, 1 = less_then_loop, 2 = take_data, 3 = more_then_loop
        private int phasePos;

        /// <summary>
        /// Number of samples in source to discard
        /// </summary>
        public TimeSpan loopBegin { get; private set; }
        /// <summary>
        /// Number of samples to read from source (if 0, then read it all)
        /// </summary>
        public TimeSpan loopLength { get; private set; }

        public int begin;
        public int length;

        /// <summary>
        /// Creates a new instance of LoopSampleProvider
        /// </summary>
        /// <param name="sampleProvider">The Source Sample Provider to read from</param>
        public LoopSampleProvider(WaveStream waveStream, ISampleProvider sampleProvider)
        {
            this.sampleProvider = sampleProvider;
            this.waveStream = waveStream;
            loopBegin = TimeSpan.Zero;
            loopLength = TimeSpan.Zero;

            begin = 0;
            length =(int)waveStream.Length;

            IsLooped = false;
        }

        public bool IsLooped { get; set; }

        /// <summary>
        /// Sets loop params into provider.
        /// </summary>
        /// <param name="begin">Start position of loop, TimeSpan.</param>        
        /// <param name="length">Length of loop, TimeSpan.</param>
        public void SetLoopTime (TimeSpan loopBegin, TimeSpan loopLength)
        {
            this.loopBegin = loopBegin;
            this.loopLength = loopLength;

            SetLoopTime(TimeSpanToBytes(loopBegin), TimeSpanToBytes(loopLength));
        }
        /// <summary>
        /// Sets loop params into provider.
        /// </summary>
        /// <param name="begin">Start position of loop, int.</param>        
        /// <param name="length">Length of loop, int.</param>
        public void SetLoopTime(int loopBegin, int loopLength)
        {
            begin = loopBegin - loopBegin % waveStream.BlockAlign;
            length = loopLength - loopLength % waveStream.BlockAlign;
            if (length <= 0)
                throw new ArgumentException("Length have to be > 0 !!!");
            if (begin < 0 || begin > waveStream.Length)
                throw new ArgumentException("Begin have to be >= 0 and < waveStream.Length!");
            if (waveStream.Position == 0)
                phase = 0;
            else if (waveStream.Position < begin)
                phase = 1;
            else if (waveStream.Position < begin + length)
            {
                phase = 2;
                phasePos = (int)waveStream.Position - begin;
            }
            else
                phase = 3;

            IsLooped = true;
        }

        public void UnsetLoop()
        {
            IsLooped = false;
        }

        private int TimeSpanToBytes(TimeSpan time)
        {
            return (int)(time.TotalSeconds * waveStream.WaveFormat.AverageBytesPerSecond);
        }

        /// <summary>
        /// The WaveFormat of this SampleProvider
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return sampleProvider.WaveFormat; }
        }

        /// <summary>
        /// Reads from this sample provider
        /// </summary>
        /// <param name="buffer">Sample buffer</param>
        /// <param name="offset">Offset within sample buffer to read to</param>
        /// <param name="count">Number of samples required</param>
        /// <returns>Number of samples read</returns>
        public int Read(float[] buffer, int offset, int count)
        {
            if (IsLooped)
            {
                int samplesRead = 0;
                if (phase == 0) // not started yet
                {
                    phase++;
                }

                if (phase == 1) // skip
                {
                    if (begin > 0)
                        waveStream.Position = begin;
                    phase++;
                    phasePos = 0;
                }
                if (phase == 2) // take data
                {
                    int samplesRequired = count;
                    if (length != 0)
                        samplesRequired = Math.Min(samplesRequired, length - phasePos);
                    samplesRead = sampleProvider.Read(buffer, offset, samplesRequired);
                    phasePos += samplesRead;
                    if (samplesRead == 0 || waveStream.Position >= (begin + length))
                    {
                        phase++;
                        phasePos = 0;
                    }
                }
                if (phase == 3) // out of loop - repeat
                {
                    waveStream.Position = begin;
                    phase = 2;
                    phasePos = 0;
                }
                return samplesRead;
            }
            else
                return sampleProvider.Read(buffer, offset, count);
        }
    }
}

