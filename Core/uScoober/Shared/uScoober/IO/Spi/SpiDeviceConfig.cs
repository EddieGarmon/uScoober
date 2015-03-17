namespace uScoober.IO.Spi
{
    public class SpiDeviceConfig
    {
        public readonly Signal BusyIndicator;
        public readonly Signal ChipSelect;
        public readonly uint ChipSelect_HoldTime;
        public readonly uint ChipSelect_SetupTime;
        public readonly bool Clock_Edge;
        public readonly bool Clock_IdleState;
        public readonly uint Clock_RateKHz;
        public readonly byte NoOpCommand;

        public SpiDeviceConfig(Signal chipSelect,
                               uint chipSelectSetupTime,
                               uint chipSelectHoldTime,
                               bool clockIdleState,
                               bool clockEdge,
                               uint clockRateKHz,
                               byte noOpCommand,
                               Signal busyIndicator = null) {
            ChipSelect = chipSelect;
            ChipSelect_SetupTime = chipSelectSetupTime;
            ChipSelect_HoldTime = chipSelectHoldTime;
            Clock_IdleState = clockIdleState;
            Clock_Edge = clockEdge;
            Clock_RateKHz = clockRateKHz;
            NoOpCommand = noOpCommand;
            BusyIndicator = busyIndicator;
        }
    }
}