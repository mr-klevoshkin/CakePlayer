﻿using System;
using System.Runtime.InteropServices;
using NAudio.Wave;

namespace NAudioPlayer
{
    /// <summary>
    /// NAudui WaveStream class for processing audio stream with SoundTouch effects
    /// </summary>
    /// <summary>
    /// NAudui WaveStream class for processing audio stream with SoundTouch effects
    /// </summary>
    public class SoundTouchProvider : WaveStream
    {
        private AudioFileReader inputStr;
        public SoundTouch st;

        private byte[] bytebuffer = new byte[4096];
        private float[] floatbuffer = new float[1024];
        bool endReached = false;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="input">WaveChannel32 stream used for processor stream input</param>
        public SoundTouchProvider(AudioFileReader input)
        {
            inputStr = input;
            //inputStr = input.GetChannel();
            st = new SoundTouch();
            st.SetChannels((uint)input.WaveFormat.Channels);
            st.SetSampleRate((uint)input.WaveFormat.SampleRate);
        }

        /// <summary>
        /// True if end of stream reached
        /// </summary>
        public bool EndReached
        {
            get { return endReached; }
        }


        public override long Length
        {
            get
            {
                return inputStr.Length;
            }
        }


        public override long Position
        {
            get
            {
                return inputStr.Position;
            }

            set
            {
                inputStr.Position = value;
            }
        }


        public override WaveFormat WaveFormat
        {
            get
            {
                return inputStr.WaveFormat;
            }
        }

        /// <summary>
        /// Overridden Read function that returns samples processed with SoundTouch. Returns data in same format as
        /// WaveChannel32 i.e. stereo float samples.
        /// </summary>
        /// <param name="buffer">Buffer where to return sample data</param>
        /// <param name="offset">Offset from beginning of the buffer</param>
        /// <param name="count">Number of bytes to return</param>
        /// <returns>Number of bytes copied to buffer</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                // Iterate until enough samples available for output:
                // - read samples from input stream
                // - put samples to SoundStretch processor
                while (st.NumSamples() < count)
                {
                    int nbytes = inputStr.Read(bytebuffer, 0, bytebuffer.Length);
                    if (nbytes == 0)
                    {
                        // end of stream. flush final samples from SoundTouch buffers to output
                        if (endReached == false)
                        {
                            endReached = true;  // do only once to avoid continuous flushing
                            st.Flush();
                        }
                        break;
                    }

                    // binary copy data from "byte[]" to "float[]" buffer
                    Buffer.BlockCopy(bytebuffer, 0, floatbuffer, 0, nbytes);
                    st.PutSamples(floatbuffer, (uint)(nbytes / 8));
                }

                // ensure that buffer is large enough to receive desired amount of data out
                if (floatbuffer.Length < count / 4)
                {
                    floatbuffer = new float[count / 4];
                }
                // get processed output samples from SoundTouch
                int numsamples = (int)st.ReceiveSamples(floatbuffer, (uint)(count / 8));
                // binary copy data from "float[]" to "byte[]" buffer
                Buffer.BlockCopy(floatbuffer, 0, buffer, offset, numsamples * 8);
                return numsamples * 8;  // number of bytes
            }
            catch (Exception exp)
            {
                //StatusMessage.Write("exception in WaveStreamProcessor.Read: " + exp.Message);
                return 0;
            }
        }

        /// <summary>
        /// Clear the internal processor buffers. Call this if seeking or rewinding to new position within the stream.
        /// </summary>
        public void Clear()
        {
            st.Clear();
            endReached = false;
        }
    }


    public class SoundTouch
    {
        private IntPtr handle;

        public SoundTouch()
        {
            handle = soundtouch_createInstance();
        }


        ~SoundTouch()
        {
            soundtouch_destroyInstance(handle);
        }

        /// <summary>
        /// Get SoundTouch version string
        /// </summary>
        public static String GetVersionString()
        {
            // convert "char *" data to c# string
            return Marshal.PtrToStringAnsi(soundtouch_getVersionString());
        }


        /// <summary>
        /// Returns number of processed samples currently available in SoundTouch for immediate output.
        /// </summary>
        public uint NumSamples()
        {
            return soundtouch_numSamples(handle);
        }

        /// <summary>
        /// Adds 'numSamples' pcs of samples from the 'samples' memory position into
        /// the input of the object. Notice that sample rate _has_to_ be set before
        /// calling this function, otherwise throws a runtime_error exception.
        /// </summary>
        /// <param name="samples">Sample buffer to input</param>
        /// <param name="numSamples">Number of sample frames in buffer. Notice
        /// that in case of multi-channel sound a single sample frame contains 
        /// data for all channels</param>
        public void PutSamples(float[] samples, uint numSamples)
        {
            soundtouch_putSamples(handle, samples, numSamples);
        }


        /// <summary>
        /// Sets the number of channels
        /// </summary>
        /// <param name="numChannels">1 = mono, 2 = stereo, n = multichannel</param>
        public void SetChannels(uint numChannels)
        {
            soundtouch_setChannels(handle, numChannels);
        }


        /// <summary>
        /// Sets sample rate.
        /// </summary>
        /// <param name="srate">Samplerate, e.g. 44100</param>
        public void SetSampleRate(uint srate)
        {
            soundtouch_setSampleRate(handle, srate);
        }


        /// <summary>
        /// Receive processed samples from the processor.
        /// </summary>
        /// <param name="outBuffer">Buffer where to copy output samples</param>
        /// <param name="maxSamples">Max number of sample frames to receive</param>
        /// <returns></returns>
        public uint ReceiveSamples(float[] outBuffer, uint maxSamples)
        {
            return soundtouch_receiveSamples(handle, outBuffer, maxSamples);
        }

        /// <summary>
        /// Flushes the last samples from the processing pipeline to the output.
        /// Clears also the internal processing buffers.
        //
        /// Note: This function is meant for extracting the last samples of a sound
        /// stream. This function may introduce additional blank samples in the end
        /// of the sound stream, and thus it's not recommended to call this function
        /// in the middle of a sound stream.
        /// </summary>
        public void Flush()
        {
            soundtouch_flush(handle);
        }

        /// <summary>
        /// Clears all the samples in the object's output and internal processing
        /// buffers.
        /// </summary>
        public void Clear()
        {
            soundtouch_clear(handle);
        }

        /// <summary>
        /// Sets new tempo control value. 
        /// </summary>
        /// <param name="newTempo">Tempo setting. Normal tempo = 1.0, smaller values
        /// represent slower tempo, larger faster tempo.</param>
        public void SetTempo(float newTempo)
        {
            soundtouch_setTempo(handle, newTempo);
        }

        /// <summary>
        /// Sets new tempo control value as a difference in percents compared
        /// to the original tempo (-50 .. +100 %);
        /// </summary>
        /// <param name="newTempo">Tempo setting in %</param>
        public void SetTempoChange(float newTempo)
        {
            soundtouch_setTempoChange(handle, newTempo);
        }

        /// <summary>
        /// Sets new rate control value. 
        /// </summary>
        /// <param name="newRate">Rate setting. Normal rate = 1.0, smaller values
        /// represent slower rate, larger faster rate.</param>
        public void SetRate(float newRate)
        {
            soundtouch_setTempo(handle, newRate);
        }

        /// <summary>
        /// Sets new rate control value as a difference in percents compared
        /// to the original rate (-50 .. +100 %);
        /// </summary>
        /// <param name="newRate">Rate setting in %</param>
        public void SetRateChange(float newRate)
        {
            soundtouch_setRateChange(handle, newRate);
        }

        /// <summary>
        /// Sets new pitch control value. 
        /// </summary>
        /// <param name="newPitch">Pitch setting. Original pitch = 1.0, smaller values
        /// represent lower pitches, larger values higher pitch.</param>
        public void SetPitch(float newPitch)
        {
            soundtouch_setPitch(handle, newPitch);
        }

        /// <summary>
        /// Sets pitch change in octaves compared to the original pitch  
        /// (-1.00 .. +1.00 for +- one octave);
        /// </summary>
        /// <param name="newPitch">Pitch setting in octaves</param>
        public void SetPitchOctaves(float newPitch)
        {
            soundtouch_setPitchOctaves(handle, newPitch);
        }

        /// <summary>
        /// Sets pitch change in semi-tones compared to the original pitch
        /// (-12 .. +12 for +- one octave);
        /// </summary>
        /// <param name="newPitch">Pitch setting in semitones</param>
        public void SetPitchSemiTones(float newPitch)
        {
            soundtouch_setPitchSemiTones(handle, newPitch);
        }

        /// <summary>
        /// int16 version of soundtouch_putSamples(): This accept int16 (short) sample data
        /// and internally converts it to float format before processing
        /// </summary>
        /// <param name="samples">Sample input buffer.</param>
        /// <param name="numSamples">Number of sample frames in buffer. Notice
        /// that in case of multi-channel sound a single 
        /// sample frame contains data for all channels.</param>
        public void PutSamples_i16(short[] samples, uint numSamples)
        {
            soundtouch_putSamples_i16(handle, samples, numSamples);
        }

        /// <summary>
        /// Changes a setting controlling the processing system behaviour. See the
        /// 'SETTING_...' defines for available setting ID's.
        /// </summary>
        /// <param name="settingId">Setting ID number. see SETTING_... defines.</param>
        /// <param name="value"New setting value></param>
        /// <returns>nonzero if successful, otherwise zero</returns>
        public int SetSetting(int settingId, int value)
        {
            return soundtouch_setSetting(handle, settingId, value);
        }

        /// <summary>
        /// Reads a setting controlling the processing system behaviour. See the
        /// 'SETTING_...' defines for available setting ID's.
        /// </summary>
        /// <param name="settingId">Setting ID number</param>
        /// <returns>The setting value</returns>
        public int soundtouch_getSetting(int settingId)
        {
            return soundtouch_getSetting(handle, settingId);
        }

        /// <summary>
        /// Returns number of samples currently unprocessed in SoundTouch internal buffer
        /// </summary>
        /// <returns>Number of sample frames</returns>
        public uint NumUnprocessedSamples()
        {
            return soundtouch_numUnprocessedSamples(handle);
        }

        /// <summary>
        /// int16 version of soundtouch_receiveSamples(): This converts internal float samples
        /// into int16 (short) return data type
        /// </summary>
        /// <param name="outBuffer">Buffer where to copy output samples.</param>
        /// <param name="maxSamples">How many samples to receive at max.</param>
        /// <returns>Number of received sample frames</returns>
        public uint soundtouch_receiveSamples_i16(short[] outBuffer, uint maxSamples)
        {
            return soundtouch_receiveSamples_i16(handle, outBuffer, maxSamples);
        }

        /// <summary>
        /// Check if there aren't any samples available for outputting.
        /// </summary>
        /// <returns>nonzero if there aren't any samples available for outputting</returns>
        public int IsEmpty()
        {
            return soundtouch_isEmpty(handle);
        }

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "soundtouch_getVersionId")]
        /// <summary>
        /// Get SoundTouch library version Id
        /// </summary>
        public static extern int GetVersionId();


        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr soundtouch_createInstance();

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_destroyInstance(IntPtr h);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr soundtouch_getVersionString();

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_setRate(IntPtr h, float newRate);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_setTempo(IntPtr h, float newTempo);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_setRateChange(IntPtr h, float newRate);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_setTempoChange(IntPtr h, float newTempo);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_setPitch(IntPtr h, float newPitch);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_setPitchOctaves(IntPtr h, float newPitch);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_setPitchSemiTones(IntPtr h, float newPitch);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_setChannels(IntPtr h, uint numChannels);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_setSampleRate(IntPtr h, uint srate);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_flush(IntPtr h);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_putSamples(IntPtr h, float[] samples, uint numSamples);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_putSamples_i16(IntPtr h, short[] samples, uint numSamples);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void soundtouch_clear(IntPtr h);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int soundtouch_setSetting(IntPtr h, int settingId, int value);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int soundtouch_getSetting(IntPtr h, int settingId);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint soundtouch_numUnprocessedSamples(IntPtr h);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint soundtouch_receiveSamples(IntPtr h, float[] outBuffer, uint maxSamples);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint soundtouch_receiveSamples_i16(IntPtr h, short[] outBuffer, uint maxSamples);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint soundtouch_numSamples(IntPtr h);

        [DllImport("SoundTouch.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int soundtouch_isEmpty(IntPtr h);
    }
}
