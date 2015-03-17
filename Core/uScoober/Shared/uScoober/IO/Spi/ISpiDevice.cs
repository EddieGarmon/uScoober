namespace uScoober.IO.Spi
{
    public interface ISpiDevice
    {
        void Read(byte[] buffer);

        void Read(ushort[] buffer, ByteOrder byteOrder);

        void Write(byte[] buffer);

        void Write(ushort[] buffer, ByteOrder byteOrder);

        void WriteRead(byte[] writeBuffer, byte[] readBuffer);

        void WriteRead(ushort[] writeBuffer, ushort[] readBuffer, ByteOrder byteOrder);

        //add offsets overload
    }
}