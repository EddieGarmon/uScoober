namespace uScoober.Hardware.Boards
{
    internal interface IDuinoAnalogInputs
    {
        IAnalogInput A0 { get; }

        IAnalogInput A1 { get; }

        IAnalogInput A2 { get; }

        IAnalogInput A3 { get; }

        IAnalogInput A4 { get; }

        IAnalogInput A5 { get; }
    }
}