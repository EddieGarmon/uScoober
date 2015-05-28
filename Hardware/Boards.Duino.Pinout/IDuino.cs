using uScoober.Hardware.I2C;
using uScoober.Hardware.Light;
using uScoober.Hardware.Spi;

namespace uScoober.Hardware.Boards
{
    internal interface IDuino
    {
        IDuinoAnalogChannels Analog { get; }

        II2CBus I2CBus { get; }

        IDigitalInterrupt OnboardButton { get; }

        IDigitalLed OnboardLed { get; }

        IDuinoPins Pins { get; }

        IDuinoPwmChannels Pwm { get; }

        ISpiBus SpiBus { get; }
    }
}