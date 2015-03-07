using System;

namespace uScoober.Hardware.Time
{
    public interface IRealTimeClock
    {
        bool IsEnabled { get; set; }

        bool IsSquareWaveOutputEnabled { get; set; }

        ClockFrequency SquareWaveFrequency { get; set; }

        DateTime GetDateTime();

        bool SetDateTime(DateTime value);

        void SyncSystemTime();
    }
}