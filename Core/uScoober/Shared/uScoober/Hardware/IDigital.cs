using System;
using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware
{
    public interface IDigitalOutput : ISignal
    {
        bool State { get; }

        void Write(bool state);
    }

    public interface IDigitalInput : ISignal
    {
        bool InvertReading { get; set; }

        bool Read();
    }

    public interface IDigitalInterupt : IDigitalInput
    {
        bool InteruptEnabled { get; set; }

        event InteruptHandler OnInterupt;
    }

    public delegate void InteruptHandler(IDigitalInterupt source, bool newPinState, DateTime time);

    public interface IDigitalPort : ISignal,
                                    IDigitalInterupt,
                                    IDigitalOutput { }

    public enum InterruptMode
    {
        InterruptNone = Port.InterruptMode.InterruptNone,
        InterruptEdgeLow = Port.InterruptMode.InterruptEdgeLow,
        InterruptEdgeHigh = Port.InterruptMode.InterruptEdgeHigh,
        InterruptEdgeBoth = Port.InterruptMode.InterruptEdgeBoth,
        InterruptEdgeLevelHigh = Port.InterruptMode.InterruptEdgeLevelHigh,
        InterruptEdgeLevelLow = Port.InterruptMode.InterruptEdgeLevelLow
    }

    public enum ResistorMode : ushort
    {
        Disabled = Port.ResistorMode.Disabled,
        External = Port.ResistorMode.Disabled,
        PullUp = Port.ResistorMode.PullUp,
        PullDown = Port.ResistorMode.PullDown
    }
}