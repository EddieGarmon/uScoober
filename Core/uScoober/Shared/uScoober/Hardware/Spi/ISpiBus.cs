using System;

namespace uScoober.Hardware.Spi
{
    public interface ISpiBus : IDisposable
    {
        void Read(SpiDeviceSettings settings, byte[] buffer);

        void Read(SpiDeviceSettings settings, ushort[] buffer, ByteOrder byteOrder);

        void Write(SpiDeviceSettings settings, byte[] buffer);

        void Write(SpiDeviceSettings settings, ushort[] buffer, ByteOrder byteOrder);

        void WriteRead(SpiDeviceSettings settings, byte[] writeBuffer, byte[] readBuffer, int startReadingAtOffset = 0);

        void WriteRead(SpiDeviceSettings settings, ushort[] writeBuffer, ushort[] readBuffer, ByteOrder byteOrder, int startReadingAtOffset = 0);

        void WriteRead(SpiDeviceSettings settings,
                       byte[] writeBuffer,
                       int writeOffset,
                       int writeCount,
                       byte[] readBuffer,
                       int readOffset,
                       int readCount,
                       int startReadingAtOffset = 0);

        void WriteRead(SpiDeviceSettings settings,
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