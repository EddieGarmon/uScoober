namespace uScoober.Hardware
{
    public delegate IDigitalInput BuildDigitalInput(Pin pin, string name = null, ResistorMode internalResistorMode = ResistorMode.Disabled);

    public interface IDigitalInput : ISignal
    {
        bool InvertReading { get; set; }

        bool Read();
    }
}