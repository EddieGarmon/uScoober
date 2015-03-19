using System;

namespace uScoober.Hardware
{
    public interface IDigitalInput : IDisposable
    {
        string Id { get; }

        bool InteruptEnabled { get; set; }

        bool InvertReading { get; set; }

        event InteruptHandler OnInterupt;

        bool Read();
    }

    public delegate void InteruptHandler(IDigitalInput source, bool newPinState, DateTime time);
}