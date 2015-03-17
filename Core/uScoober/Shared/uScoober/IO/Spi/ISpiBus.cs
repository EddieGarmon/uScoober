namespace uScoober.IO.Spi
{
    public interface ISpiBus
    {
        void Read(SpiDeviceConfig config, byte[] buffer);

        void Read(SpiDeviceConfig config, ushort[] buffer, ByteOrder byteOrder);

        void Write(SpiDeviceConfig config, byte[] buffer);

        void Write(SpiDeviceConfig config, ushort[] buffer, ByteOrder byteOrder);

        void WriteRead(SpiDeviceConfig config, byte[] writeBuffer, byte[] readBuffer, int startReadingAtOffset = 0);

        void WriteRead(SpiDeviceConfig config, ushort[] writeBuffer, ushort[] readBuffer, ByteOrder byteOrder, int startReadingAtOffset = 0);

        void WriteRead(SpiDeviceConfig config,
                       byte[] writeBuffer,
                       int writeOffset,
                       int writeCount,
                       byte[] readBuffer,
                       int readOffset,
                       int readCount,
                       int startReadingAtOffset = 0);

        void WriteRead(SpiDeviceConfig config,
                       ushort[] writeBuffer,
                       int writeOffset,
                       int writeCount,
                       ushort[] readBuffer,
                       int readOffset,
                       int readCount,
                       ByteOrder byteOrder,
                       int startReadingAtOffset = 0);
    }
}