using System;
using System.IO;
using NAudio.Wave;
using System.Windows.Forms;
using Visualization;
using Equalization;

namespace NAudioPlayer
{
    public class CakePlayer : IDisposable// ,IPlayer
    {
        private WaveOutEvent outputDevice;
        private CakeProvider audioProvider;
        private PictureBox image;

        public static readonly TimeSpan BackwardForwardIntervalDefault = TimeSpan.FromMilliseconds(1000 * 5); // 1000 * 1 = 1 секунда
        private float playbackRate = 1f;
        public PlaybackState playbackState { get; private set; }
        public bool IsDataAvailable { get; private set; }

        public CakePlayer()
        {
            outputDevice = new WaveOutEvent();
            outputDevice.PlaybackStopped += PlaybackStopped;
            audioProvider = new CakeProvider();
            SkipStep = BackwardForwardIntervalDefault;
            AutoVolumeCorrection = true;
            image = new PictureBox();
            playbackState = PlaybackState.Stopped;
            IsDataAvailable = false;

        }


        private void PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (outputDevice.PlaybackState == PlaybackState.Stopped)
            {
                audioProvider.Position = 0;
                playbackState = PlaybackState.Stopped;
                if (Stopped != null)
                {
                    Stopped(this, EventArgs.Empty);
                }
            }
        }

        public bool AutoVolumeCorrection
        {
            get;
            set;
        }

        public Equalizer initializeEqulizerControl()
        {
            return audioProvider.GetEqualizer();
        }

        //public void Open(SimplePrihdWaveStreamRequest stream)
        //{
        //    audioProvider.Init(stream);
        //    if (playbackRate != 1)
        //        audioProvider.Speed = playbackRate;
        //    outputDevice?.Init(audioProvider);
        //}

        public void Open(WaveStream stream)
        {
            audioProvider.Init(stream);
            if (playbackRate != 1)
                audioProvider.Speed = playbackRate;
            outputDevice?.Init(audioProvider);
            IsDataAvailable = true;
        }

        public void DrawSignal(WaveStream stream, PictureBox pictureBox)
        {
            image = pictureBox;
            ImageSettings imageInfo = new ImageSettings
            {
                Size = pictureBox.Size,
                TopHeight = pictureBox.Size.Height / 2,
                BottomHeight = pictureBox.Size.Height / 2,
                DecibelScale = false
            };

            audioProvider.InitDrawing(stream, imageInfo);
            image.Image = audioProvider.GetImage();
        }

        public void Close()
        {
            Stop();
            playbackState = PlaybackState.Stopped;
            if (audioProvider.IsInited)
            {
                playbackRate = audioProvider.Speed;
                audioProvider.Dispose();
            }
            IsDataAvailable = false;
        }

        public void Play()
        {
            if (!audioProvider.IsInited)
                throw new Exception("Open file befor playing! Call 'Open'");
            outputDevice?.Play();
            playbackState = PlaybackState.Playing;
        }

        public void Pause()
        {
            outputDevice?.Pause();
            playbackState = PlaybackState.Paused;
        }

        public void Stop()
        {
            outputDevice?.Stop();
            playbackState = PlaybackState.Stopped;
        }

        public TimeSpan SkipStep
        {
            get;
            set;
        }

        public void Forward()
        {
            if (audioProvider.IsInited)
                audioProvider.Skip((int)SkipStep.TotalSeconds);
        }

        public void Backward()
        {
            if (audioProvider.IsInited)
                audioProvider.Skip(-(int)SkipStep.TotalSeconds);
        }

        public int Volume // 0 - 100
        {
            get
            {
                return (int)(outputDevice?.Volume * 100);
            }
            set => outputDevice.Volume = value / 100f;
        }

        public TimeSpan Time
        {
            get
            {
                if (audioProvider.IsInited)
                    return audioProvider.CurrentTime;
                else
                    return TimeSpan.Zero;
            }
            set
            {
                if (audioProvider.IsInited)
                    audioProvider.CurrentTime = value;
            }
        }

        public TimeSpan TotalTime
        {
            get
            {
                if (audioProvider.IsInited)
                    return audioProvider.TotalTime;
                else
                    return TimeSpan.Zero;
            }
        }

        public float Rate
        {
            get
            {
                if (audioProvider.IsInited)
                    return audioProvider.Speed;
                else
                    return 1;
            }
            set
            {
                if (audioProvider.IsInited)
                    audioProvider.Speed = value;
                else
                    playbackRate = value;
            }
        }

        public long Length
        {
            get
            {
                if (audioProvider.IsInited)
                    return audioProvider.Length;
                else
                    return 0;
            }
        }

        public float Position // 0-1
        {
            get
            {
                if (audioProvider.IsInited)
                    if (audioProvider.Length != 0)
                        return (float)audioProvider.Position / audioProvider.Length;
                    else
                        return -1;
                else
                    return 0;
            }
            set
            {
                if (audioProvider.IsInited)
                    audioProvider.Position = (long)(value * audioProvider.Length);
            }
        }

        public void SetPlaybackLoop(TimeSpan start, TimeSpan stop) // 1 is a normal speed
        {
            if (audioProvider.IsInited)
            {
                audioProvider.SetLoop(start, stop - start);
                image.Image = audioProvider.SetLoopBG(start, stop - start);
            }
        }

        public void UnsetPlaybackLoop()
        {
            if (audioProvider.IsInited)
            {
                audioProvider.ResetLoop();
                image.Image = audioProvider.UnsetLoopBG();
            }
        }

        public void Reset()
        {
            audioProvider.Reset(outputDevice);
        }

        public void Dispose()
        {
            Close();
            outputDevice.Dispose();
        }

        public bool IsPlaying
        {
            get
            {
                if (outputDevice?.PlaybackState == PlaybackState.Playing)
                    return true;
                else
                    return false;
            }
        }

        public bool IsPaused
        {
            get
            {
                if (outputDevice?.PlaybackState == PlaybackState.Paused)
                    return true;
                else
                    return false;
            }
        }

        public bool IsMute { get; set; }

        public float LeftVolume
        {
            get
            {
                if (audioProvider.IsInited)
                    return audioProvider.LeftVolume;
                else
                    return 0;
            }
            set
            {
                if (audioProvider.IsInited) audioProvider.LeftVolume = value;
            }
        }

        public float RightVolume
        {
            get
            {
                if (audioProvider.IsInited)
                    return audioProvider.RightVolume;
                else
                    return 0;
            }
            set
            {
                if (audioProvider.IsInited) audioProvider.RightVolume = value;
            }
        }

        public int Channel // 1 - mono, 2 - stereo
        {
            get
            {
                if (audioProvider.IsInited && audioProvider.IsMono)
                    return 1;
                else
                    return 2;
            }
            set
            {
                if (audioProvider.IsInited)
                {
                    if (audioProvider.SourceIsMono && value == 2)
                        audioProvider.SetStereo(outputDevice, LeftVolume, RightVolume);
                    else if (!audioProvider.SourceIsMono && value == 1)
                        audioProvider.SetMono(outputDevice, LeftVolume, RightVolume);
                }
            }
        }

        public event EventHandler Stopped;
    }
}