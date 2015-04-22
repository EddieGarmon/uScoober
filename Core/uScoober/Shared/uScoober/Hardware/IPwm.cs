namespace uScoober.Hardware
{
    public interface IPulseWidthModulatedOutput : ISignal
    {
        uint Duration { get; }

        double DutyCycle { get; set; }

        double Frequency { get; set; }

        bool IsActive { get; }

        uint Period { get; }

        void RampTo(double finalDutyCycle, ushort durationMilliseconds = 1000, ushort stepCount = 100);

        void SetDutyCycleAndFrequency(double dutyCycle, uint frequencyHz);

        void SetPulse(uint periodNanoseconds, uint highDurationNanoseconds);

        void Start();

        void Stop();
    }
}