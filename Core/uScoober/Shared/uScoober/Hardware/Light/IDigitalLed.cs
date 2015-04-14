using System;

namespace uScoober.Hardware.Light
{
    public interface IDigitalLed : IDisposable
    {
        bool IsOn { get; set; }

        void TurnOff();

        void TurnOn();
    }
}