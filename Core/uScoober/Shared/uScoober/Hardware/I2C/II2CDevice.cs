using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware.I2C
{
    public interface II2CDevice
    {
        ushort Address { get; }

        int TimeoutMilliseconds { get; set; }

        I2CDevice.I2CReadTransaction CreateReadTransaction(params byte[] buffer);

        I2CDevice.I2CWriteTransaction CreateWriteTransaction(params byte[] buffer);

        int Execute(I2CDevice.I2CTransaction action);

        int Execute(I2CDevice.I2CTransaction[] actions);

        bool Read(byte[] readBuffer);

        bool Read(out byte value);

        bool Read(out ushort value, ByteOrder byteOrder);

        bool ReadRegister(byte address, out byte value);

        bool ReadRegister(byte address, out ushort value, ByteOrder byteOrder);

        bool ReadRegister(byte address, byte[] buffer);

        bool Write(byte[] writeBuffer);

        bool Write(byte value);

        bool Write(ushort value, ByteOrder byteOrder);

        bool WriteRead(byte[] writeBuffer, byte[] readBuffer);

        bool WriteRegister(byte address, byte value);

        bool WriteRegister(byte address, ushort value, ByteOrder byteOrder);

        bool WriteRegister(byte address, byte[] buffer);
    }
}