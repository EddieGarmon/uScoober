using uScoober.Hardware.Input;
using uScoober.Hardware.Light;

namespace uScoober.Hardware.Boards
{
    internal interface IDuino
    {
        IDuinoAnalogInputs AnalogIn { get; }

        IDuinoDigitalInputs DigitalIn { get; }

        IDuinoDigitalOutputs DigitalOut { get; }

        IButton OnboardButton { get; }

        IDigitalLed OnboardLed { get; }

        IDuinoPwmOutputs PwmOut { get; }
    }
}