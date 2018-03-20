using NAudio.Wave;

namespace Visualization
{
    public interface IPeakProvider
    {
        void Init(ISampleProvider reader, int samplesPerPixel);
        PeakInfo GetNextPeak();
    }
}