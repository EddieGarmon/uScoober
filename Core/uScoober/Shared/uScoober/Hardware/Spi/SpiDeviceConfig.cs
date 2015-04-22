namespace uScoober.Hardware.Spi
{
    public class SpiDeviceConfig
    {
        public readonly ISignal BusyIndicator;
        public readonly ISignal ChipSelect;
        public readonly bool ChipSelect_ActiveState;
        public readonly uint ChipSelect_HoldTime;
        public readonly uint ChipSelect_SetupTime;
        public readonly bool Clock_Edge;
        public readonly bool Clock_IdleState;
        public readonly uint Clock_RateKHz;
        public readonly byte NoOpCommand;

        public SpiDeviceConfig(ISignal chipSelect,
                               bool chipSelectActiveState,
                               uint chipSelectSetupTime,
                               uint chipSelectHoldTime,
                               bool clockIdleState,
                               bool clockEdge,
                               uint clockRateKHz,
                               byte noOpCommand,
                               ISignal busyIndicator = null) {
            ChipSelect = chipSelect;
            ChipSelect_ActiveState = chipSelectActiveState;
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