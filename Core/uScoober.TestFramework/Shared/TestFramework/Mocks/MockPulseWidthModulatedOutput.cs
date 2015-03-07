using System;
using uScoober.IO;

namespace uScoober.TestFramework.Mocks
{
    public class MockPulseWidthModulatedOutput : DisposableBase,
                                                 IPulseWidthModulatedOutput
    {
        public MockPulseWidthModulatedOutput(string id) {
            Id = id;
        }

        public uint Duration { get; private set; }

        public double DutyCycle {
            get { return (double)Duration / Period; }
            set {
                if (value < 0 || value > 1) {
                    throw new Exception("Duty cycle must be in the range [0,1].");
                }
                Duration = (uint)(Period * value);
            }
        }

        public double Frequency {
            get { return (double)Nanoseconds / Period; }
            set {
                double newPeriod = Nanoseconds / value;
                double newDuration = Period * DutyCycle;
                Period = (uint)newPeriod;
                Duration = (uint)newDuration;
            }
        }

        public string Id { get; private set; }

        public bool IsActive { get; private set; }

        public uint Period { get; private set; }

        public void RampTo(double finalDutyCycle, ushort durationMilliseconds = 1000, ushort stepCount = 100) {
            throw new NotImplementedException("MockPulseWidthModulatedOutput.RampTo");
        }

        public void SetDutyCycleAndFrequency(double dutyCycle, uint frequencyHz) {
            double newPeriod = (double)Nanoseconds / frequencyHz;
            double newDuration = dutyCycle * newPeriod;
            Period = (uint)newPeriod;
            Duration = (uint)newDuration;
        }

        public void SetPulse(uint periodNanoseconds, uint highDurationNanoseconds) {
            Period = periodNanoseconds;
            Duration = highDurationNanoseconds;
        }

        public void Start() {
            IsActive = true;
        }

        public void Stop() {
            IsActive = false;
        }

        private const uint Nanoseconds = 1000000000U;
    }
}