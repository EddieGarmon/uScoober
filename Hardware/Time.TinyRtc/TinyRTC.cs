using System;
using uScoober.IO.I2CBus;

namespace uScoober.Hardware.Time
{
    public class TinyRTC : DisposableBase,
                           IRealTimeClock,
                           IHaveDefaultSetup,
                           IHaveAddressedMemory
    {
        private readonly DS1307 _clock;
        private object TemperatureSensor;
        private AddressMap _addressedMemoryMap;
        private IHaveAddressedMemory _eeprom;

        public TinyRTC(II2CBus bus, byte clockI2CAddress = 0x68, byte eepromI2CAddress = 0x50, int clockRateKhz = 100) {
            _clock = new DS1307(bus, clockI2CAddress, clockRateKhz);
            //_eeprom = new _eeprom(bus, eepromI2CAddress, clockRateKhz);
            _addressedMemoryMap = new AddressMap();
            // 4k byte of ram on eeprom >> Virtual Address 0..3999
            // 56 byte of ram on clock >> Virtual Address 5000..5055
            _addressedMemoryMap.MapRange(0, 3999, address => address, _eeprom);
        }

        public bool IsEnabled {
            get { return _clock.IsEnabled; }
            set { _clock.IsEnabled = value; }
        }

        public bool IsSquareWaveOutputEnabled {
            get { return _clock.IsSquareWaveOutputEnabled; }
            set { _clock.IsSquareWaveOutputEnabled = value; }
        }

        public ClockFrequency SquareWaveFrequency {
            get { return _clock.SquareWaveFrequency; }
            set { _clock.SquareWaveFrequency = value; }
        }

        public int TotalBytesAvailable { get; private set; }

        public DateTime GetDateTime() {
            return _clock.GetDateTime();
        }

        public byte ReadMemory(ushort address) {
            throw new NotImplementedException("TinyRTC.ReadMemory");
        }

        public void ReadMemory(ushort startAddress, byte[] buffer, int bufferIndex = 0, int length = -1) {
            throw new NotImplementedException("TinyRTC.ReadMemory");
        }

        public bool SetDateTime(DateTime value) {
            return _clock.SetDateTime(value);
        }

        public void SetupDefaults() {
            throw new NotImplementedException("TinyRTC.SetupDefaults");
        }

        public void SyncSystemTime() {
            _clock.SyncSystemTime();
        }

        public void WriteMemory(ushort address, byte value) {
            throw new NotImplementedException("TinyRTC.WriteMemory");
        }

        public void WriteMemory(ushort startAddress, byte[] buffer, int bufferStartIndex = 0, int length = -1) {
            throw new NotImplementedException("TinyRTC.WriteMemory");
        }

        protected override void DisposeManagedResources() {
            _clock.Dispose();
            //_eeprom.Dispose();
        }
    }
}