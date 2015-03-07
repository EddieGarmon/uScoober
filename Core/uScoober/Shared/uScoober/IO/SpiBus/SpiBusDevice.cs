using Microsoft.SPOT.Hardware;

namespace uScoober.IO.SpiBus
{
    public abstract class SpiBusDevice : DisposableBase
    {
        private readonly byte[] _tempBuffer = new byte[1];
        private SPI.Configuration _configuration;

        protected SpiBusDevice(SPI.Configuration configuration) {
            _configuration = configuration;
            var x = new SPI(_configuration);
            //Microsoft.SPOT.Hardware.SPI.SPI_module spiModule = SecretLabs.NETMF.Hardware.Netduino.SPI_Devices.SPI1;
            //spiModule.
        }
    }
}