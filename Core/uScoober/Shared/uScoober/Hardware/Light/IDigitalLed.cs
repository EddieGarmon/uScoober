using System;

namespace uScoober.Hardware.Light
{
    public interface IDigitalLed : IDisposable
    {
        bool IsOn { get; }

        void TurnOff();

        void TurnOn();
    }
}