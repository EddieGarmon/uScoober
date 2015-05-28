namespace uScoober.Hardware.Boards
{
    internal interface IDuinoPwmChannels
    {
        PwmChannel PinD10 { get; }

        PwmChannel PinD11 { get; }

        PwmChannel PinD3 { get; }

        PwmChannel PinD5 { get; }

        PwmChannel PinD6 { get; }

        PwmChannel PinD9 { get; }
    }
}