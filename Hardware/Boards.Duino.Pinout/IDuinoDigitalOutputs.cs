using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware.Boards
{
    internal interface IDuinoDigitalOutputs
    {
        IDigitalOutput Bind(Cpu.Pin pin, bool initialState = false, string id = null);

        IDigitalOutput Get(Cpu.Pin pin);
    }
}