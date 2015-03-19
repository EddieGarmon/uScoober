using Microsoft.SPOT.Hardware;
using uScoober.Hardware.Spi;
using uScoober.TestFramework.Assert;

namespace uScoober.Hardware.Spot
{
    internal class TestDevice : SpiDeviceCore
    {
        public TestDevice(ISpiBus bus, Cpu.Pin chipSelect)
            : base(bus, new SpiDeviceConfig(new Signal(chipSelect, true), 1, 1, true, true, 400, 0x00)) { }

        public int GetExampleValue() {
            var buffer = new byte[4];
            Read(buffer);
            //make into an integer and return
            return 0;
        }
    }

    public class SpotSpiBusTests
    {
        public void API_Fact() {
            var bus = new SpotSpiBus(SPI.SPI_module.SPI1);

            var testDevice = new TestDevice(bus, Cpu.Pin.GPIO_Pin1);

            testDevice.GetExampleValue()
                      .ShouldEqual(0);
        }
    }
}