using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware.Spot
{
    internal class SpotDigitalInput : DisposableBase,
                                      IDigitalInput
    {
        private readonly InputPort _input;
        private readonly string _name;
        private bool _invertReading;

        static SpotDigitalInput() {
            Signals.DigitalInput.NewInstance = (pin, name, mode) => new SpotDigitalInput((Cpu.Pin)pin, name, (Port.ResistorMode)mode);
        }

        private SpotDigitalInput(Cpu.Pin pin, string name = null, Port.ResistorMode internalResistorMode = Port.ResistorMode.Disabled) {
            _input = new InputPort(pin, false, internalResistorMode);
            _name = name ?? "DigitalInput-" + pin;
        }

        public bool InvertReading {
            get {
                ThrowIfDisposed();
                return _invertReading;
            }
            set {
                ThrowIfDisposed();
                _invertReading = value;
            }
        }

        public string Name {
            get {
                ThrowIfDisposed();
                return _name;
            }
        }

        public Pin Pin {
            get {
                ThrowIfDisposed();
                return (Pin)_input.Id;
            }
        }

        public bool Read() {
            ThrowIfDisposed();
            bool value = _input.Read();
            return InvertReading ? !value : value;
        }

        protected override void DisposeManagedResources() {
            _input.Dispose();
        }
    }
}