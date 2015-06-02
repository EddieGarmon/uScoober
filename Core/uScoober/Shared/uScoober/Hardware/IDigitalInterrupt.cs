using System;

namespace uScoober.Hardware
{
    public delegate void InterruptHandler(IDigitalInterrupt source, bool newPinState, DateTime time);

    public delegate IDigitalInterrupt BuildDigitalInterrupt(
        Pin pin,
        string name = null,
        ResistorMode internalResistorMode = ResistorMode.Disabled,
        InterruptMode interruptMode = InterruptMode.InterruptNone,
        int debounceMilliseconds = DigitalInterupt.DebounceDefault);

    public interface IDigitalInterrupt : IDigitalInput
    {
        bool InterruptEnabled { get; set; }

        event InterruptHandler OnInterrupt;
    }
}