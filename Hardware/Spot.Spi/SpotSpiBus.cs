using System.Collections;
using Microsoft.SPOT.Hardware;
using uScoober.Hardware.Spi;

namespace uScoober.Hardware.Spot
{
    internal class SpotSpiBus : DisposableBase,
                                ISpiBus
    {
        //todo validate byte order when working with ushort[], read and write
        //todo implement hardware busy indicators
        private readonly Hashtable _knownConfigs = new Hashtable();
        private readonly SPI.SPI_module _module;
        private readonly SPI _spotSpi;
        private int _lastConfigHash;

        public SpotSpiBus(SPI.SPI_module module) {
            _module = module;
            _spotSpi = new SPI(new SPI.Configuration(Cpu.Pin.GPIO_NONE, true, 0, 0, true, true, 0, module));
        }

        public void Read(SpiDeviceSettings settings, byte[] buffer) {
            ConfigureBusForDevice(settings);
            _spotSpi.WriteRead(settings.NoOpBytes, buffer, 0);
        }

        public void Read(SpiDeviceSettings settings, ushort[] buffer, ByteOrder byteOrder) {
            ConfigureBusForDevice(settings);
            _spotSpi.WriteRead(settings.NoOpShorts, buffer);
        }

        public void Write(SpiDeviceSettings settings, byte[] buffer) {
            ConfigureBusForDevice(settings);
            _spotSpi.Write(buffer);
        }

        public void Write(SpiDeviceSettings settings, ushort[] buffer, ByteOrder byteOrder) {
            ConfigureBusForDevice(settings);
            _spotSpi.Write(buffer);
        }

        public void WriteRead(SpiDeviceSettings settings, byte[] writeBuffer, byte[] readBuffer, int startReadingAtOffset = 0) {
            ConfigureBusForDevice(settings);
            _spotSpi.WriteRead(writeBuffer, readBuffer, startReadingAtOffset);
        }

        public void WriteRead(SpiDeviceSettings settings, ushort[] writeBuffer, ushort[] readBuffer, ByteOrder byteOrder, int startReadingAtOffset = 0) {
            ConfigureBusForDevice(settings);
            _spotSpi.WriteRead(writeBuffer, readBuffer, startReadingAtOffset);
        }

        public void WriteRead(SpiDeviceSettings settings,
                              byte[] writeBuffer,
                              int writeOffset,
                              int writeCount,
                              byte[] readBuffer,
                              int readOffset,
                              int readCount,
                              int startReadingAtOffset = 0) {
            ConfigureBusForDevice(settings);
            _spotSpi.WriteRead(writeBuffer, writeOffset, writeCount, readBuffer, readOffset, readCount, startReadingAtOffset);
        }

        public void WriteRead(SpiDeviceSettings settings,
                              ushort[] writeBuffer,
                              int writeOffset,
                              int writeCount,
                              ushort[] readBuffer,
                              int readOffset,
                              int readCount,
                              ByteOrder byteOrder,
                              int startReadingAtOffset = 0) {
            ConfigureBusForDevice(settings);
            _spotSpi.WriteRead(writeBuffer, writeOffset, writeCount, readBuffer, readOffset, readCount, startReadingAtOffset);
        }

        private void ConfigureBusForDevice(SpiDeviceSettings settings) {
            int newHash = settings.GetHashCode();
            if (newHash == _lastConfigHash) {
                return;
            }
            SPI.Configuration result;
            if (_knownConfigs.Contains(settings)) {
                result = (SPI.Configuration)_knownConfigs[settings];
            }
            else {
                result = new SPI.Configuration(settings.SoftChipSelectEnabled ? Cpu.Pin.GPIO_NONE : (Cpu.Pin)settings.ChipSelectPin,
                                               settings.ChipSelectActiveState,
                                               settings.ChipSelectSetupTime,
                                               settings.ChipSelectHoldTime,
                                               settings.ClockIdleState,
                                               settings.ClockEdge,
                                               settings.ClockRateKHz,
                                               _module);
                _knownConfigs.Add(settings, result);
            }
            _spotSpi.Config = result;
            _lastConfigHash = newHash;
        }
    }
}