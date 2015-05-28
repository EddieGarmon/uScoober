namespace uScoober.Hardware.Boards
{
    internal interface IDuinoPins
    {
        Pin A0 { get; }

        Pin A1 { get; }

        Pin A2 { get; }

        Pin A3 { get; }

        Pin A4 { get; }

        Pin A5 { get; }

        Pin D0 { get; }

        Pin D1 { get; }

        Pin D10 { get; }

        Pin D11 { get; }

        Pin D12 { get; }

        Pin D13 { get; }

        Pin D2 { get; }

        Pin D3 { get; }

        Pin D4 { get; }

        Pin D5 { get; }

        Pin D6 { get; }

        Pin D7 { get; }

        Pin D8 { get; }

        Pin D9 { get; }

        Pin OnboardButton { get; }

        Pin OnboardLed { get; }
    }
}