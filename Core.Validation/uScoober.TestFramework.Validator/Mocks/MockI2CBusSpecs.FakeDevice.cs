using Microsoft.SPOT.Hardware;
using uScoober.Hardware.I2C;

namespace uScoober.TestFramework.Mocks
{
    public partial class MockI2CBusSpecs
    {
        private class FakeDevice : I2CDeviceCore
        {
            public FakeDevice(MockI2CBus bus, ushort address, int clockRateKhz)
                : base(bus, address, clockRateKhz) { }

            public new ushort Address {
                get { return base.Address; }
            }

            public new int TimeoutMilliseconds {
                get { return base.TimeoutMilliseconds; }
                set { base.TimeoutMilliseconds = value; }
            }

            public new I2CDevice.I2CReadTransaction CreateReadTransaction(params byte[] buffer) {
                return base.CreateReadTransaction(buffer);
            }

            public new I2CDevice.I2CWriteTransaction CreateWriteTransaction(params byte[] buffer) {
                return base.CreateWriteTransaction(buffer);
            }

            public new int Execute(I2CDevice.I2CTransaction action) {
                return base.Execute(action);
            }

            public new int Execute(I2CDevice.I2CTransaction[] actions) {
                return base.Execute(actions);
            }

            public new bool Read(byte[] readBuffer) {
                return base.Read(readBuffer);
            }

            public new bool Read(out byte value) {
                return base.Read(out value);
            }

            public new bool Read(out ushort value, ByteOrder byteOrder) {
                return base.Read(out value, byteOrder);
            }

            public new bool ReadRegister(byte address, out byte value) {
                return base.ReadRegister(address, out value);
            }

            public new bool ReadRegister(byte address, out ushort value, ByteOrder byteOrder) {
                return base.ReadRegister(address, out value, byteOrder);
            }

            public new bool ReadRegister(byte address, byte[] buffer) {
                return base.ReadRegister(address, buffer);
            }

            public new bool Write(byte[] writeBuffer) {
                return base.Write(writeBuffer);
            }

            public new bool Write(byte value) {
                return base.Write(value);
            }

            public new bool Write(ushort value, ByteOrder byteOrder) {
                return base.Write(value, byteOrder);
            }

            public new bool WriteRead(byte[] writeBuffer, byte[] readBuffer) {
                return base.WriteRead(writeBuffer, readBuffer);
            }

            public new bool WriteRegister(byte address, byte value) {
                return base.WriteRegister(address, value);
            }

            public new bool WriteRegister(byte address, ushort value, ByteOrder byteOrder) {
                return base.WriteRegister(address, value, byteOrder);
            }

            public new bool WriteRegister(byte address, byte[] buffer) {
                return base.WriteRegister(address, buffer);
            }
        }
    }
}