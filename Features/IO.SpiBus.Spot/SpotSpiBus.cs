using Microsoft.SPOT.Hardware;

namespace uScoober.IO.SpiBus
{
    public class SpotSpiBus : DisposableBase,
                              ISpiBus
    {
        private SPI _spi;
    }
}