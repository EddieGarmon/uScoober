using System;

namespace uScoober.IO.Spi.Bitbang
{
    internal class SoftSpiBus :ISpiBus{
        public void Read(SpiDeviceConfig config, byte[] buffer) {
            throw new NotImplementedException();
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