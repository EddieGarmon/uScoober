using System;

namespace uScoober.Hardware.Time
{
    [Flags]
    public enum ClockFrequency : byte
    {
        _1_Hz = 0,
        _4_kHz = 1,
        _8_kHz = 2,
        _32_kHz = 3
    }
}