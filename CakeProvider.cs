using System;
using System.Drawing;
using NAudio.Wave;
using Speed;
using Equalization;
using Visualization;

/// <summary>
/// CakeProvider for manipulating audio:
///     speed;
///     loop;
///     equalizer;
///     sterem - mono transformations.
///     
/// Depends: 
///     Librarys:
///         NAudio.dll ; SoundTouch.dll
///     Directorys:
///         .\Speed, .\Equalization, .\Visualisation
/// </summary>

namespace NAudioPlayer
{
    public class CakeProvider : ISampleProvider, IDisposable
    {
        private ISampleProvider sourceProvider;
        private WaveStream sourceStream;

        private MonoAndStereo channelFile;
        private LoopSampleProvider loopFile;
        private VarispeedSampleProvider speedControl;
        private Equalizer equalizedFile;
        private ISampleProvider outputFile;
        private Vistalizator visualizator;
        
        private bool _IsSpeedModulated;
        private bool _IsLooped;
        private bool _IsMono;
        private static bool _SourceIsMono;
        private bool _IsEqualized;
        private bool _IsInited = false;
        private bool _IsInitedDrawing = false;
        public bool ReachEOF { get; private set; }

        public CakeProvider() {  }

        public CakeProvider(ISampleProvider source, WaveStream stream)
        {
            Init(source, stream);
        }
        public CakeProvider(WaveStream stream)
        {
            Init(WaveExtensionMethods.ToSampleProvider(stream), stream);
        }

        /*
        public CakeProvider (SimplePrihdWaveStreamRequest stream)
        {
            Init(stream);
        }

        public void Init(SimplePrihdWaveStreamRequest readerStream)
        {
            Init(RM.Wave.SampleProviderConverters.ConvertWaveProviderIntoSampleProvider(readerStream), readerStream);
        }
        */


        public void InitDrawing(WaveStream stream, ImageSettings imageInfo)
        {
            visualizator = new Vistalizator(stream, imageInfo);
            _IsInitedDrawing = true;
        }

        public void Init(WaveStream stream)
        {
            Init(WaveExtensionMethods.ToSampleProvider(stream), stream);
        }

        public void Init(ISampleProvider source, WaveStream stream)
        {
            this.sourceProvider = source;
            this.sourceStream = stream;

            speedControl = new VarispeedSampleProvider(sourceProvider, 100, new SoundTouchProfile(true, false));
            loopFile = new LoopSampleProvider(sourceStream, speedControl);
            equalizedFile = new Equalizer(loopFile);
            channelFile = new MonoAndStereo(equalizedFile);
            outputFile = channelFile;

            Volume = 1f;
            _IsSpeedModulated = false;
            _IsLooped = false;
            if (sourceProvider.WaveFormat.Channels == 1)
            {
                _SourceIsMono = true;
                _IsMono = true;
            }
            else
            {
                _SourceIsMono = false;
                _IsMono = false;
            }
            _IsInited = true;
            ReachEOF = false;
        }


        public void Dispose()
        {
            _IsInited = false;
        }

        // ===================== ISampleProvider =====================
        public int Read(float[] buffer, int offset, int count)
        {
            if (IsInited)
            {
                int samplesRead = outputFile.Read(buffer, offset, count);
                if (samplesRead == 0)
                    ReachEOF = true;
                if (Volume != 1f)
                {
                    for (int n = 0; n < count; n++)
                    {
                        buffer[offset + n] *= Volume;
                    }
                }
                return samplesRead;
            }
            else
                throw new NullReferenceException("Nothing to read from! Init first!");   
        }
        public WaveFormat WaveFormat
        {
            get { return outputFile?.WaveFormat; }
        }

        // ===================== Functions =====================
        public bool IsInited
        {
            get { return _IsInited; }
        }
        public bool IsInitedDrawing
        {
            get { return _IsInitedDrawing; }
        }
        public void Skip(int seconds)
        {
            if (IsInited)
                sourceStream.Skip(seconds);
        }
        public bool SourceIsMono
        {
            get { return _SourceIsMono; }
        }

        public void Reset(WaveOutEvent outputDevice)
        {
            ResetEqualizer();
            ResetLoop();
            ResetSpeed();
            ResetStereo(outputDevice);
        }
        public long Position
        {
            get
            {
                if (IsInited)
                    return sourceStream.Position;
                else
                    return -1;
            }
            set
            {
                if (IsInited)
                {
                    if (value % sourceStream.BlockAlign == 0)
                        sourceStream.Position = value;
                    else
                        sourceStream.Position = value - (value % sourceStream.BlockAlign);
                    if (value < sourceStream.Length)
                        ReachEOF = false;
                }
            }
        }

        public long Length
        {
            get
            {
                if (IsInited)
                    return sourceStream.Length;
                else
                    return 0;
            }
        }

        public float Volume { get; set;}

        public TimeSpan CurrentTime
        {
            get
            {
                if (IsInited)
                    return sourceStream.CurrentTime;
                else
                    return TimeSpan.Zero;
            }
            set
            {
                if (IsInited)
                    sourceStream.CurrentTime = value;
            }
        }

        public TimeSpan TotalTime
        {
            get
            {
                if (IsInited)
                    return sourceStream.TotalTime;
                else
                    return TimeSpan.Zero;
            }
        }

        // ===================== Visualisation =====================

        public object[] GetRenderVariants()
        {
            if (visualizator != null)
                return visualizator.GetRenderVariants();
            else
                return null;
        }

        public Image GetImage ()
        {
            if (visualizator != null)
                return visualizator.GetImage();
            else
                return null;
        }
        public Image SetLoopBG(int beginPosition, int lenght) => visualizator.SetLoopBG(beginPosition, lenght);
        public Image SetLoopBG(TimeSpan begin, TimeSpan lenght) => visualizator.SetLoopBG(begin, lenght);
        public Image UnsetLoopBG() => visualizator.UnsetLoopBG();

        // ===================== Looping =====================
        public void SetLoop(TimeSpan begin, TimeSpan length)
        {
            if (IsInited)
            {
                loopFile.SetLoopTime(begin, length);
                _IsLooped = true;
            }
            else
                throw new NullReferenceException("Init() First!");
        }

        public void SetLoop(int begin, int length)
        {
            if (IsInited)
            {
                loopFile.SetLoopTime(begin, length);
                _IsLooped = true;
            }
            else
                throw new NullReferenceException("Init() First!");
        }

        public void ResetLoop()
        {
            if (IsInited)
            {
                loopFile.UnsetLoop();
                _IsLooped = false;
            }
            else
                throw new NullReferenceException("Init() First!");
        }

        public bool IsLooped
        {
            get
            {
                if (IsInited)
                    return _IsLooped;
                else
                    throw new NullReferenceException("Init() First!");
            }
            private set { _IsLooped = value; }
        }

        // ===================== Speed =====================
        public bool IsSpeedModulated
        {
            get
            {
                if (IsInited)
                    return _IsSpeedModulated;
                else
                    throw new NullReferenceException("Init() First!");
            }
            private set { _IsSpeedModulated = value; }
        }

        public float Speed
        {
            get
            {
                if (IsInited)
                    return speedControl.PlaybackRate;
                else
                    throw new NullReferenceException("Init() First!");
            }
            set
            {
                if (IsInited)
                {
                    speedControl.PlaybackRate = value; // value = '1' is a normal temp
                    if (value != 1f)
                        _IsSpeedModulated = true;
                    else
                        _IsSpeedModulated = false;
                }
                else
                    throw new NullReferenceException("Init() First!");
            }
        }
        public void ResetSpeed()
        {
            if (IsInited)
            {
                speedControl.PlaybackRate = 1;
                _IsSpeedModulated = false;
            }
            else
                throw new NullReferenceException("Init() First!");
        }

        // ===================== Mono and Stereo =====================
        public bool IsMono
        {
            get
            {
                if (IsInited)
                    return _IsMono;
                else
                    throw new NullReferenceException("Init() First!");
            }
            private set { _IsMono = value; }
        }
        public void ResetStereo(WaveOutEvent outputDevice)
        {
            if (IsInited)
            {
                channelFile.MonoAndStereoMode = MonoAndStereoMode.None;
                _IsMono = _SourceIsMono;

                ReInit(outputDevice);
            }
            else
                throw new NullReferenceException("Init() First!");
        }
        private void ReInit(WaveOutEvent outputDevice)
        {
            bool onPlaying = (outputDevice?.PlaybackState != PlaybackState.Stopped);
            if (IsInited)
            {
                if (onPlaying)
                    outputDevice.Stop();   
                outputDevice.Dispose();
                outputDevice.Init(outputFile);
                if (IsLooped)
                    SetLoop(loopFile.loopBegin, loopFile.loopLength);
                if (onPlaying)
                    outputDevice.Play();
            }
            else
                throw new NullReferenceException("Init() First!");
        }

        public void SetMono(WaveOutEvent outputDevice, float leftChannel, float rightChannel)
        {
            if (IsInited)
            {
                if (_SourceIsMono)
                    channelFile.MonoAndStereoMode = MonoAndStereoMode.None;
                else
                    channelFile.MonoAndStereoMode = MonoAndStereoMode.StereoToMono;
                _IsMono = true;
                channelFile.LeftVolume = leftChannel;
                channelFile.RightVolume = rightChannel;

                ReInit(outputDevice);
            }
            else
                throw new NullReferenceException("Init() First!");
        }

        public void SetStereo(WaveOutEvent outputDevice, float leftChannel, float rightChannel)
        {
            if(IsInited)
            {
                if (!_SourceIsMono)
                    channelFile.MonoAndStereoMode = MonoAndStereoMode.None;
                else
                    channelFile.MonoAndStereoMode = MonoAndStereoMode.MonoToStereo;
                _IsMono = false;
                channelFile.LeftVolume = leftChannel;
                channelFile.RightVolume = rightChannel;

                ReInit(outputDevice);
            } 
            else
                throw new NullReferenceException("Init() First!");
        }

        public float RightVolume
        {
            get {
                if (IsInited)
                    return channelFile.RightVolume;
                else
                    throw new NullReferenceException("Init() First!");
            }
            set {
                if (IsInited)
                    channelFile.RightVolume = value;
            }
        }

        public float LeftVolume
        {
            get
            {
                if (IsInited)
                    return channelFile.LeftVolume;
                else
                    throw new NullReferenceException("Init() First!");
            }
            set
            {
                if (IsInited)
                    channelFile.LeftVolume = value;
            }
        }

        // ===================== Equalizer =====================
        public bool IsEqualized
        {
            get
            {
                if (IsInited)
                {
                    _IsEqualized = equalizedFile.IsEqualized();
                    return _IsEqualized;
                }
                else
                    throw new NullReferenceException("Init() First!");
            }
            private set { _IsEqualized = value; }
        }

        public Equalizer GetEqualizer()
        {
            return equalizedFile;
        }

        public float[] ShowFreqsCount
        {
            get
            {
                if (IsInited)
                    return equalizedFile.Freqs;
                else
                    throw new NullReferenceException("Init() First!"); 
            }
        }

        public EqualizerBand[] Bands
        {
            get
            {
                if (IsInited)
                    return equalizedFile.GetAmps();
                else
                    throw new NullReferenceException("Init() First!");
            }
            set
            {
                if (IsInited)
                    equalizedFile.SetAmps(value);
                else
                    throw new NullReferenceException("Init() First!");
            }
        }

        public void SetBandAmp(int bandNum, float gain, string flag = "both")
        {
            if (IsInited)
            {
                equalizedFile.SetBandAmp(bandNum, gain, flag);
                equalizedFile.Update();
            }
            else
                throw new NullReferenceException("Init() First!");
        }

        public float GetBandAmp(int bandNum, string flag = "both")
        {
            if (IsInited)
                return equalizedFile.GetBandAmp(bandNum, flag);
            else
                throw new NullReferenceException("Init() First!");
        }

        public void ResetEqualizer()
        {
            if (IsInited)
            {
                equalizedFile.SetZeroAmps();
                equalizedFile.Update();
            }
            else
                throw new NullReferenceException("Init() First!");
        }

        public void SetVoiceAmps()
        {
            if (IsInited)
            {
                equalizedFile.SetVoiceAmps();
                equalizedFile.Update();
            }
            else
                throw new NullReferenceException("Init() First!");
        }
    }
}