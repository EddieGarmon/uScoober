using System;
using uScoober.TestFramework.Assert;

namespace uScoober.TestFramework.Mocks
{
    public partial class MockI2CBusSpecs
    {
        public void AllFunctionsPerformedByMultipleDevices_Fact() {
            //todo refactor this test into many
            var bus = new MockI2CBus();

            var device1 = new FakeDevice(bus, 0x01, 400);
            var device2 = new FakeDevice(bus, 0x02, 400);

            bus.BufferInputFor(device1.Address, 0xAA, 0x13);
            bus.BufferInputFor(device2.Address, 0x12);

            var temp = new byte[1];

            device1.WriteRegister(0xAA, 0x05);
            bus.ShouldObserveOutput(device1.Address, 0xAA, 0x05);

            device2.Write(0x50);
            bus.ShouldObserveOutput(device2.Address, 0x50);

            var device1Output = new byte[] {
                0xAB
            };
            device1.WriteRead(device1Output, temp);
            bus.ShouldObserveOutput(device1.Address, 0xAB);
            temp[0].ShouldEqual(0xAA);

            device2.Read(temp);
            temp[0].ShouldEqual(0x12);

            device1.ReadRegister(0x15, temp);
            bus.ShouldObserveOutput(device1.Address, 0x15);
            temp[0].ShouldEqual(0x13);
        }

        public void InputBufferNotQueued_Fact() {
            var bus = new MockI2CBus();
            var device1 = new FakeDevice(bus, 0x01, 400);
            var temp = new byte[1];

            var e = Trap.Exception(() => device1.Read(temp));
            e.ShouldBeOfType(typeof(IndexOutOfRangeException));
        }
    }
}