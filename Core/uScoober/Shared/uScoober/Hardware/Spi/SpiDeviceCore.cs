namespace uScoober.Hardware.Spi
{
    public abstract class SpiDeviceCore : DisposableBase,
                                          ISpiDevice
    {
        private readonly ISpiBus _bus;
        private readonly SpiDeviceConfig _config;

        protected SpiDeviceCore(ISpiBus bus, SpiDeviceConfig config) {
            _bus = bus;
            _config = config;
        }

        protected void Read(byte[] buffer) {
            lock (_bus) {
                _bus.Read(_config, buffer);
            }
        }

        protected void Read(ushort[] buffer, ByteOrder byteOrder) {
            lock (_bus) {
                _bus.Read(_config, buffer, byteOrder);
            }
        }

        protected void Write(byte[] buffer) {
            lock (_bus) {
                _bus.Write(_config, buffer);
            }
        }

        protected void Write(ushort[] buffer, ByteOrder byteOrder) {
            lock (_bus) {
                _bus.Write(_config, buffer, byteOrder);
            }
        }

        protected void WriteRead(byte[] writeBuffer, byte[] readBuffer, int startReadingAtOffset = 0) {
            lock (_bus) {
                _bus.WriteRead(_config, writeBuffer, readBuffer, startReadingAtOffset);
            }
        }

        protected void WriteRead(ushort[] writeBuffer, ushort[] readBuffer, ByteOrder byteOrder, int startReadingAtOffset = 0) {
            lock (_bus) {
                _bus.WriteRead(_config, writeBuffer, readBuffer, byteOrder, startReadingAtOffset);
            }
        }

        protected void WriteRead(byte[] writeBuffer,
                                 int writeOffset,
                                 int writeCount,
                                 byte[] readBuffer,
                                 int readOffset,
                                 int readCount,
                                 int startReadingAtOffset = 0) {
            lock (_bus) {
                _bus.WriteRead(_config, writeBuffer, writeOffset, writeCount, readBuffer, readOffset, readCount, startReadingAtOffset);
            }
        }

        protected void WriteRead(ushort[] writeBuffer,
                                 int writeOffset,
                                 int writeCount,
                                 ushort[] readBuffer,
                                 int readOffset,
                                 int readCount,
                                 ByteOrder byteOrder,
                                 int startReadingAtOffset) {
            lock (_bus) {
                _bus.WriteRead(_config, writeBuffer, writeOffset, writeCount, readBuffer, readOffset, readCount, byteOrder, startReadingAtOffset);
            }
        }

        void ISpiDevice.Read(byte[] buffer) {
            Read(buffer);
        }

        void ISpiDevice.Read(ushort[] buffer, ByteOrder byteOrder) {
            Read(buffer, byteOrder);
        }

        void ISpiDevice.Write(byte[] buffer) {
            Write(buffer);
        }

        void ISpiDevice.Write(ushort[] buffer, ByteOrder byteOrder) {
            Write(buffer, byteOrder);
        }

        void ISpiDevice.WriteRead(byte[] writeBuffer, byte[] readBuffer, int startReadingAtOffset) {
            WriteRead(writeBuffer, readBuffer, startReadingAtOffset);
        }

        void ISpiDevice.WriteRead(ushort[] writeBuffer, ushort[] readBuffer, ByteOrder byteOrder, int startReadingAtOffset) {
            WriteRead(writeBuffer, readBuffer, byteOrder, startReadingAtOffset);
        }

        void ISpiDevice.WriteRead(byte[] writeBuffer,
                                  int writeOffset,
                                  int writeCount,
                                  byte[] readBuffer,
                                  int readOffset,
                                  int readCount,
                                  int startReadingAtOffset) {
            WriteRead(writeBuffer, writeOffset, writeCount, readBuffer, readOffset, readCount, startReadingAtOffset);
        }

        void ISpiDevice.WriteRead(ushort[] writeBuffer,
                                  int writeOffset,
                                  int writeCount,
                                  ushort[] readBuffer,
                                  int readOffset,
                                  int readCount,
                                  ByteOrder byteOrder,
                                  int startReadingAtOffset) {
            WriteRead(writeBuffer, writeOffset, writeCount, readBuffer, readOffset, readCount, byteOrder, startReadingAtOffset);
        }
    }
}