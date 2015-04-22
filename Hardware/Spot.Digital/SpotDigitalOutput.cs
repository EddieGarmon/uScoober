using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware.Spot
{
    internal class SpotDigitalOutput : DisposableBase,
                                       IDigitalOutput
    {
        private readonly string _name;
        private readonly OutputPort _port;
        private bool _state;

        public SpotDigitalOutput(Cpu.Pin pin, 
            bool initialState = false, string name = null) {
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

        public int Pin {
            get {
                ThrowIfDisposed();
                return (int)_port.Id;
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