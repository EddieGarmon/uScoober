using System;

namespace uScoober.Hardware.Light
{
    public interface IAnalogLed : IDigitalLed,
                                  IDisposable
    {
        double DutyCycle { get; set; }

        void FadeOff(ushort durationMilliseconds = 1000);

        void FadeOn(double dutyCycle = 1, ushort durationMilliseconds = 1000);

        void TurnOn(double dutyCycle = 1);
    }
}