using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware.Spot
{
    internal class SpotDigitalPort : DisposableBase,
                                     IDigitalPort
    {
        private readonly int _debounceMilliseconds;
        private readonly string _name;
        private readonly TristatePort _tristate; // = new TristatePort(Cpu.Pin);

        public SpotDigitalPort(Cpu.Pin pin,
                               bool initialState = false,
                               string name = null,
                               Port.ResistorMode internalResistorMode = Port.ResistorMode.Disabled,
                               InterruptMode interruptMode = InterruptMode.InterruptNone,
                               int debounceMilliseconds = 0) {
            _tristate = new TristatePort(pin, initialState, false, internalResistorMode);
            _name = name ?? "DigitalPort-" + pin;
            //_tristate.
        }

        public int DebounceMilliseconds {
            get {
                ThrowIfDisposed();
                return _debounceMilliseconds;
            }
        }

        public bool InteruptEnabled { get; set; }

        public bool InvertReading { get; set; }

        public string Name {
            get { return _name; }
        }

        public int Pin { get; private set; }

        public bool State { get; private set; }

        public event InteruptHandler OnInterupt;

        public bool Read() {
            ThrowIfDisposed();
            if (_tristate.Active) {
                //tristate.Active will not throw in 4.4
                _tristate.Active = false;
            }
            return _tristate.Read();
        }

        public void Write(bool state) {
            ThrowIfDisposed();
            if (!_tristate.Active) {
                //tristate.Active will not throw in 4.4
                _tristate.Active = true;
            }
            _tristate.Write(state);
        }
    }
}