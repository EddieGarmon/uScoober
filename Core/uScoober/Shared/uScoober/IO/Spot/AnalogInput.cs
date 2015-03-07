using Microsoft.SPOT.Hardware;
using SpotAnalogInput = Microsoft.SPOT.Hardware.AnalogInput;

namespace uScoober.IO.Spot
{
    public class AnalogInput : DisposableBase,
                               IAnalogInput
    {
        private readonly SpotAnalogInput _port;

        public AnalogInput(Cpu.AnalogChannel analogChannel, string id = null) {
            _port = new SpotAnalogInput(analogChannel, 1.0, 0.0, 12);
            Id = id ?? "AnalogIn-" + analogChannel;
        }

        public string Id { get; private set; }

        public double Read() {
            ThrowIfDisposed();
            return _port.Read() * 3.3;
        }

        protected override void DisposeManagedResources() {
            _port.Dispose();
        }

        ~AnalogInput() {
            Dispose(DisposeReason.Finalizer);
        }
    }
}