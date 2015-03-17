namespace uScoober.IO.Spi
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

        protected void WriteRead(byte[] writeBuffer, byte[] readBuffer) {
            lock (_bus) {
                _bus.WriteRead(_config, writeBuffer, readBuffer);
            }
        }

        protected void WriteRead(ushort[] writeBuffer, ushort[] readBuffer, ByteOrder byteOrder) {
            lock (_bus) {
                _bus.WriteRead(_config, writeBuffer, readBuffer, byteOrder);
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

        void ISpiDevice.WriteRead(byte[] writeBuffer, byte[] readBuffer) {
            WriteRead(writeBuffer, readBuffer);
        }

        void ISpiDevice.WriteRead(ushort[] writeBuffer, ushort[] readBuffer, ByteOrder byteOrder) {
            WriteRead(writeBuffer, readBuffer, byteOrder);
        }
    }
}