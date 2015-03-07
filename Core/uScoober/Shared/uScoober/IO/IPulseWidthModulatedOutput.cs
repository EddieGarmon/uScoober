using System;

namespace uScoober.IO
{
    public interface IPulseWidthModulatedOutput : IDisposable
    {
        uint Duration { get; }

        double DutyCycle { get; set; }

        double Frequency { get; set; }

        string Id { get; }

        bool IsActive { get; }

        uint Period { get; }

        void RampTo(double finalDutyCycle, ushort durationMilliseconds = 1000, ushort stepCount = 100);

        void SetDutyCycleAndFrequency(double dutyCycle, uint frequencyHz);

        void SetPulse(uint periodNanoseconds, uint highDurationNanoseconds);

        void Start();

        void Stop();
    }
}