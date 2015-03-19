namespace uScoober.Hardware.Spi
{
    public interface ISpiDevice
    {
        void Read(byte[] buffer);

        void Read(ushort[] buffer, ByteOrder byteOrder);

        void Write(byte[] buffer);

        void Write(ushort[] buffer, ByteOrder byteOrder);

        void WriteRead(byte[] writeBuffer, byte[] readBuffer, int startReadingAtOffset = 0);

        void WriteRead(ushort[] writeBuffer, ushort[] readBuffer, ByteOrder byteOrder, int startReadingAtOffset = 0);

        void WriteRead(byte[] writeBuffer, int writeOffset, int writeCount, byte[] readBuffer, int readOffset, int readCount, int startReadingAtOffset = 0);

        void WriteRead(ushort[] writeBuffer,
                       int writeOffset,
                       int writeCount,
                       ushort[] readBuffer,
                       int readOffset,
                       int readCount,
                       ByteOrder byteOrder,
                       int startReadingAtOffset = 0);
    }
}