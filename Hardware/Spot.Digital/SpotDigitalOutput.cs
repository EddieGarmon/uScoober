using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware.Spot
{
    internal class SpotDigitalOutput : DisposableBase,
                                       IDigitalOutput
    {
        private readonly OutputPort _port;

        public SpotDigitalOutput(Cpu.Pin pin, bool initialState = false, string id = null) {
            _port = new OutputPort(pin, initialState);
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

        ~SpotDigitalOutput() {
            Dispose(DisposeReason.Finalizer);
        }
    }
}