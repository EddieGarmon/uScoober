using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware.Spot
{
    internal class SpotAnalogInput : DisposableBase,
                                     IAnalogInput
    {
        private readonly string _name;
        private readonly AnalogInput _port;

        public SpotAnalogInput(Cpu.AnalogChannel analogChannel, string id = null) {
            // _port.Read() should return 0V to 3.3V
            _port = new AnalogInput(analogChannel, 3.3, 0.0, 12);
            _name = id ?? "AnalogIn-" + analogChannel;
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
                return (int)_port.Pin;
            }
        }

        public double Read() {
            ThrowIfDisposed();
            return _port.Read();
        }

        protected override void DisposeManagedResources() {
            _port.Dispose();
        }

        ~SpotAnalogInput() {
            Dispose(DisposeReason.Finalizer);
        }
    }
}