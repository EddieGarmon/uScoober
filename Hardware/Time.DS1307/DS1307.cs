using System;
using Microsoft.SPOT.Hardware;
using uScoober.Binary;
using uScoober.IO.I2CBus;

namespace uScoober.Hardware.Time
{
    public class DS1307 : I2CBusDevice,
                          IRealTimeClock,
                          IHaveDefaultSetup,
                          IHaveAddressedMemory
    {
        public DS1307(II2CBus bus, ushort address = DefaultDeviceI2CAddress, int clockRateKhz = DefaultDeviceI2CClockRateKhz)
            : base(bus, address, clockRateKhz) { }

        public bool IsEnabled {
            get {
                byte temp;
                ReadRegister(RegisterSeconds, out temp);
                return BitMask.IsAllOff(temp, ConfigClockHaltMask);
            }
            set {
                byte temp;
                ReadRegister(RegisterSeconds, out temp);
                temp = value ? BitMask.TurnOffBits(temp, ConfigClockHaltMask) : BitMask.TurnOnBits(temp, ConfigClockHaltMask);
                WriteRegister(RegisterSeconds, temp);
            }
        }

        public bool IsSquareWaveOutputEnabled {
            get {
                byte config;
                ReadRegister(RegisterConfig, out config);
                return BitMask.IsAllOn(config, ConfigOutputSquareWaveMask);
            }
            set {
                byte config;
                ReadRegister(RegisterConfig, out config);
                config = value ? BitMask.TurnOnBits(config, ConfigOutputSquareWaveMask) : BitMask.TurnOffBits(config, ConfigOutputSquareWaveMask);
                WriteRegister(RegisterConfig, config);
            }
        }

        public ClockFrequency SquareWaveFrequency {
            get {
                byte config;
                ReadRegister(RegisterConfig, out config);
                return (ClockFrequency)BitMask.GetValue(config, ConfigFrequencyMask);
            }
            set {
                byte config;
                ReadRegister(RegisterConfig, out config);
                config = (byte)(BitMask.TurnOffBits(config, ConfigFrequencyMask) | (byte)value);
                WriteRegister(RegisterConfig, config);
            }
        }

        public int TotalBytesAvailable {
            get { return 56; }
        }

        public DateTime GetDateTime() {
            var buffer = new byte[7];
            ReadRegister(RegisterSeconds, buffer);
            // ignore clock halt bit
            buffer[RegisterSeconds] = BitMask.TurnOffBits(buffer[RegisterSeconds], ConfigClockHaltMask);
            int seconds = Bcd.Unpack(buffer[RegisterSeconds]);
            int minutes = Bcd.Unpack(buffer[RegisterMinutes]);
            // avoid 12/24 hour bit (forced 24 hour)
            buffer[RegisterHours] = (byte)(buffer[RegisterHours] & ~ConfigTwelveHourClockMask);
            int hours = Bcd.Unpack(buffer[RegisterHours]);
            int days = Bcd.Unpack(buffer[RegisterDays]);
            int months = Bcd.Unpack(buffer[RegisterMonths]);
            int years = Bcd.Unpack(buffer[RegisterYears]) + 2000;
            return new DateTime(years, months, days, hours, minutes, seconds);
        }

        public byte ReadMemory(ushort address) {
            throw new NotImplementedException("DS1307.ReadMemory");
        }

        public void ReadMemory(ushort startAddress, byte[] buffer, int bufferIndex = 0, int length = -1) {
            throw new NotImplementedException("DS1307.ReadMemory");
        }

        public bool SetDateTime(DateTime value) {
            byte temp;
            ReadRegister(RegisterSeconds, out temp);
            var transaction = CreateWriteTransaction(RegisterSeconds,
                                                     (byte)(BitMask.GetValue(temp, ConfigClockHaltMask) | Bcd.Pack(value.Second)),
                                                     Bcd.Pack(value.Minute),
                                                     Bcd.Pack(value.Hour),
                                                     Bcd.Pack((int)(value.DayOfWeek + 1)),
                                                     Bcd.Pack(value.Day),
                                                     Bcd.Pack(value.Month),
                                                     Bcd.Pack(value.Year - 2000));
            return Execute(transaction) >= 8;
        }

        public void SetupDefaults() {
            byte temp;
            // 24 hour clock >> address 0x02, bit 6, low
            ReadRegister(RegisterHours, out temp);
            temp = BitMask.TurnOffBits(temp, ConfigTwelveHourClockMask);
            WriteRegister(RegisterHours, temp);
            // low when square off >> address 0x07, bit 7 low
            // square off >> address 0x07, bit 4 low
            // 1 Hz >> address 0x07, bits 1 and 0 low
            WriteRegister(RegisterConfig, 0x00);
        }

        public void SyncSystemTime() {
            if (IsEnabled) {
                Utility.SetLocalTime(GetDateTime());
            }
        }

        public void WriteMemory(ushort address, byte value) {
            throw new NotImplementedException("DS1307.WriteMemory");
        }

        public void WriteMemory(ushort startAddress, byte[] buffer, int bufferStartIndex = 0, int length = -1) {
            throw new NotImplementedException("DS1307.WriteMemory");
        }

        public const byte DS1307_RAM_END_ADDRESS = 0x3f;
        public const byte DS1307_RAM_START_ADDRESS = 0x08;

        private const byte ConfigClockHaltMask = (1 << 7);
        private const byte ConfigFrequencyMask = 0x03;
        private const byte ConfigOutputSquareWaveMask = (1 << 4); // enable oscillator output
        private const byte ConfigTwelveHourClockMask = (1 << 6); // 12/25 hour mode (1 --> 12 hour, 0 --> 24 hour)

        private const ushort DefaultDeviceI2CAddress = 0x68; // 1101000 (see on datasheet)
        private const int DefaultDeviceI2CClockRateKhz = 100; // DS1307 works only in I2C standard mode (100 Khz)

        private const byte RegisterConfig = 7;
        private const byte RegisterDays = 4;
        private const byte RegisterGeneral = 8;
        private const byte RegisterHours = 2;
        private const byte RegisterMinutes = 1;
        private const byte RegisterMonths = 5;
        private const byte RegisterSeconds = 0;
        private const byte RegisterYears = 6;
    }
}