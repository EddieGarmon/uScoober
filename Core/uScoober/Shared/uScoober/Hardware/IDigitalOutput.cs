using System;

namespace uScoober.Hardware
{
    public interface IDigitalOutput : IDisposable
    {
        string Id { get; }

        bool State { get; }

        void Write(bool state);
    }
}