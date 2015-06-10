namespace uScoober.Hardware.Spi
{
    public class SpiDeviceSettings
    {
        public readonly IDigitalOutput BusyIndicator;
        public readonly IDigitalOutput ChipSelect;
        public readonly bool ChipSelectActiveState;
        public readonly uint ChipSelectHoldTime;
        public readonly Pin ChipSelectPin;
        public readonly uint ChipSelectSetupTime;
        public readonly bool ClockEdge;
        public readonly bool ClockIdleState;
        public readonly uint ClockRateKHz;
        public readonly byte[] NoOpBytes;
        public readonly ushort[] NoOpShorts;
        public readonly bool SoftChipSelectEnabled;

        public SpiDeviceSettings(Pin pin,
                                 bool chipSelectActiveState,
                                 uint chipSelectSetupTime,
                                 uint chipSelectHoldTime,
                                 bool clockIdleState,
                                 bool clockEdge,
                                 uint clockRateKHz,
                                 byte noOpCommand,
                                 IDigitalOutput busyIndicator = null)
            : this(chipSelectActiveState, chipSelectSetupTime, chipSelectHoldTime, clockIdleState, clockEdge, clockRateKHz, noOpCommand, busyIndicator) {
            ChipSelectPin = pin;
            SoftChipSelectEnabled = false;
        }

        public SpiDeviceSettings(IDigitalOutput chipSelect,
                                 bool chipSelectActiveState,
                                 uint chipSelectSetupTime,
                                 uint chipSelectHoldTime,
                                 bool clockIdleState,
                                 bool clockEdge,
                                 uint clockRateKHz,
                                 byte noOpCommand,
                                 IDigitalOutput busyIndicator = null)
            : this(chipSelectActiveState, chipSelectSetupTime, chipSelectHoldTime, clockIdleState, clockEdge, clockRateKHz, noOpCommand, busyIndicator) {
            ChipSelect = chipSelect;
            ChipSelectPin = Pin.None;
            SoftChipSelectEnabled = (chipSelect != null);
        }

        private SpiDeviceSettings(bool chipSelectActiveState,
                                  uint chipSelectSetupTime,
                                  uint chipSelectHoldTime,
                                  bool clockIdleState,
                                  bool clockEdge,
                                  uint clockRateKHz,
                                  byte noOpCommand,
                                  IDigitalOutput busyIndicator = null) {
            ChipSelectActiveState = chipSelectActiveState;
            ChipSelectSetupTime = chipSelectSetupTime;
            ChipSelectHoldTime = chipSelectHoldTime;
            ClockIdleState = clockIdleState;
            ClockEdge = clockEdge;
            ClockRateKHz = clockRateKHz;
            NoOpBytes = new[] {
                noOpCommand
            };
            NoOpShorts = new ushort[] {
                noOpCommand
            };
            BusyIndicator = busyIndicator;
        }
    }
}