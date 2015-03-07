using Microsoft.SPOT.Hardware;
using SpotOutputPort = Microsoft.SPOT.Hardware.OutputPort;

namespace uScoober.IO.Spot
{
    public class DigitalOutput : DisposableBase,
                                 IDigitalOutput
    {
        private readonly SpotOutputPort _port;

        public DigitalOutput(Cpu.Pin pin, bool initialState = false, string id = null) {
            _port = new SpotOutputPort(pin, initialState);
            State = initialState;
            Id = id ?? "DigitalOut-" + pin;
        }

        public string Id { get; private set; }

        public bool State { get; private set; }

        public void Write(bool state) {
            ThrowIfDisposed();
            _port.Write(state);
            State = state;
        }

        protected override void DisposeManagedResources() {
            _port.Dispose();
        }

        ~DigitalOutput() {
            Dispose(DisposeReason.Finalizer);
        }
    }
}