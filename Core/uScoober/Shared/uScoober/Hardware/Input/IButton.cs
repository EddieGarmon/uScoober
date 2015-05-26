using System;

namespace uScoober.Hardware.Input
{
    public interface IButton : IDisposable
    {
        event Action ButtonDown;

        event Action ButtonUp;
    }
}