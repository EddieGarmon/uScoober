using System;

namespace uScoober.Hardware.Input
{
    internal interface IButton : IDisposable
    {
        event Action ButtonDown;

        event Action ButtonUp;
    }
}