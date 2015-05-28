namespace uScoober.Hardware
{
    public delegate IDigitalOutput BuildDigitalOutput(Pin pin, string name = null, bool initialState = false);

    public interface IDigitalOutput : ISignal
    {
        bool State { get; }

        void Write(bool state);
    }
}