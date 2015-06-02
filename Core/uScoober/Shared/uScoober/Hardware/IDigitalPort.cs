namespace uScoober.Hardware
{
    //public delegate IDigitalPort BuildDigitalPort(
    //    Pin pin,
    //    string name = null,
    //    ResistorMode internalResistorMode = ResistorMode.Disabled,
    //    InterruptMode interruptMode = InterruptMode.InterruptNone,
    //    int debounceMilliseconds = 0);

    public interface IDigitalPort : ISignal,
                                    IDigitalInterrupt,
                                    IDigitalOutput { }
}