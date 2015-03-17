using System;
using Microsoft.SPOT.Hardware;

namespace uScoober.IO.Spi.Spot
{
    public class SpotSpiBus : DisposableBase,
                              ISpiBus
    {

        private readonly byte[] _noOpBuffer = new byte[1];
        private readonly SPI.Configuration _spotConfig;
        private readonly SPI _spotSpi;

        private int _lastConfigHash;
        private SPI.SPI_module _module;

        public SpotSpiBus(SPI.SPI_module module) {
            _module = module;
            _spotConfig = new SPI.Configuration(Cpu.Pin.GPIO_NONE, true, 0, 0, true, true, 400, module);
            _spotSpi = new SPI(_spotConfig);
        }

        private void ConfigureBusForDevice(SpiDeviceConfig config) {
            int newHash = config.GetHashCode();
            if (newHash == _lastConfigHash) { return;}
            //todo optomize better by building a simple registry table
            _spotSpi.Config = new SPI.Configuration(
                config.ChipSelect.Pin,
                config.ChipSelect.ActiveState,
                config.ChipSelect_SetupTime,
                config.ChipSelect_HoldTime,
                config.Clock_IdleState,
                config.Clock_Edge,
                config.Clock_RateKHz,
                _module
                );
            _noOpBuffer[0] = config.NoOpCommand;
            _lastConfigHash = newHash;
        }

        public void Read(SpiDeviceConfig config, byte[] buffer) {
            ConfigureBusForDevice(config);
            _spotSpi.WriteRead(_noOpBuffer, buffer, 0);
        }

        public void Read(SpiDeviceConfig config, ushort[] buffer, ByteOrder byteOrder) {
            throw new NotImplementedException();
        }

        public void Write(SpiDeviceConfig config, byte[] buffer) {
            throw new NotImplementedException();
        }

        public void Write(SpiDeviceConfig config, ushort[] buffer, ByteOrder byteOrder) {
            throw new NotImplementedException();
        }

        public void WriteRead(SpiDeviceConfig config, byte[] writeBuffer, byte[] readBuffer, int startReadingAtOffset = 0) {
            throw new NotImplementedException();
        }

        public void WriteRead(SpiDeviceConfig config, ushort[] writeBuffer, ushort[] readBuffer, ByteOrder byteOrder, int startReadingAtOffset = 0) {
            throw new NotImplementedException();
        }

        public void WriteRead(SpiDeviceConfig config,
                              byte[] writeBuffer,
                              int writeOffset,
                              int writeCount,
                              byte[] readBuffer,
                              int readOffset,
                              int readCount,
                              int startReadingAtOffset = 0) {
            throw new NotImplementedException();
        }

        public void WriteRead(SpiDeviceConfig config,
                              ushort[] writeBuffer,
                              int writeOffset,
                              int writeCount,
                              ushort[] readBuffer,
                              int readOffset,
                              int readCount,
                              ByteOrder byteOrder,
                              int startReadingAtOffset = 0) {
            throw new NotImplementedException();
        }
    }
}