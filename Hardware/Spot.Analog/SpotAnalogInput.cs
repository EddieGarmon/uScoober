using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware.Spot
{
    internal class SpotAnalogInput : DisposableBase,
                                     IAnalogInput
    {
        private readonly string _name;
        private readonly AnalogInput _port;

        static SpotAnalogInput() {
            Signals.AnalogInput.NewInstance = (channel, name) => new SpotAnalogInput((Cpu.AnalogChannel)channel, name);
        }

        private SpotAnalogInput(Cpu.AnalogChannel analogChannel, string name = null) {
            // _port.Read() should return 0V to 3.3V
            _port = new AnalogInput(analogChannel, 3.3, 0.0, 12);
            _name = name ?? "AnalogIn-" + analogChannel;
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
                return (Pin)_port.Pin;
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