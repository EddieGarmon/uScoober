using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware.Spot
{
    internal class SpotDigitalOutput : DisposableBase,
                                       IDigitalOutput
    {
        private readonly string _name;
        private readonly OutputPort _port;
        private bool _state;

        static SpotDigitalOutput() {
            Signals.DigitalOutput.NewInstance = (pin, name, state) => new SpotDigitalOutput((Cpu.Pin)pin, name, state);
        }

        private SpotDigitalOutput(Cpu.Pin pin, string name = null, bool initialState = false) {
            _port = new OutputPort(pin, initialState);
            _name = name ?? "DigitalOut-" + pin;
            Write(initialState);
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
                return (Pin)_port.Id;
            }
        }

        public bool State {
            get {
                ThrowIfDisposed();
                return _state;
            }
        }

        public void Write(bool state) {
            ThrowIfDisposed();
            _state = state;
            _port.Write(state);
        }

        protected override void DisposeManagedResources() {
            _port.Dispose();
        }

        ~SpotDigitalOutput() {
            Dispose(DisposeReason.Finalizer);
        }
    }
}