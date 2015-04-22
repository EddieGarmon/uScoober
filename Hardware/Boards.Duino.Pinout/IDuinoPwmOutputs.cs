namespace uScoober.Hardware.Boards
{
    internal interface IDuinoPwmOutputs
    {
        IPulseWidthModulatedOutput D10 { get; }

        IPulseWidthModulatedOutput D11 { get; }

        IPulseWidthModulatedOutput D3 { get; }

        IPulseWidthModulatedOutput D5 { get; }

        IPulseWidthModulatedOutput D6 { get; }

        IPulseWidthModulatedOutput D9 { get; }
    }
}