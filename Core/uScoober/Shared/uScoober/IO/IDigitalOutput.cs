using System;

namespace uScoober.IO
{
    public interface IDigitalOutput : IDisposable
    {
        string Id { get; }

        bool State { get; }

        void Write(bool state);
    }
}