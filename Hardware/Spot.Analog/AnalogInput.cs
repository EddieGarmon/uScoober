using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware.Spot
{
    internal class SpotAnalogInput : DisposableBase,
                                     IAnalogInput
    {
        private readonly AnalogInput _port;

        public SpotAnalogInput(Cpu.AnalogChannel analogChannel, string id = null) {
            _port = new AnalogInput(analogChannel, 1.0, 0.0, 12);
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

        ~SpotAnalogInput() {
            Dispose(DisposeReason.Finalizer);
        }
    }
}