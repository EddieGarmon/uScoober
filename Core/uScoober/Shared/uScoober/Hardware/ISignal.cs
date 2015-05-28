using System;

namespace uScoober.Hardware
{
    public interface ISignal : IDisposable
    {
        string Name { get; }

        Pin Pin { get; }
    }
}