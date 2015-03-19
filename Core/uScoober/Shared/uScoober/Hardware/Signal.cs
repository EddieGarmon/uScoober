using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware
{
    public class Signal
    {
        public readonly bool ActiveState;
        public readonly string Name;
        public readonly Cpu.Pin Pin;

        public Signal(Cpu.Pin pin, bool activeState, string name = null) {
            Pin = pin;
            ActiveState = activeState;
            Name = name ?? ("Signal: " + pin);
        }
    }
}