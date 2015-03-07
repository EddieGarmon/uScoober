using System.Diagnostics;
using uScoober.IO;

namespace uScoober.Hardware.Light
{
    [DebuggerDisplay("DigitalLed: {Id} IsOn:{IsOn}")]
    public class DigitalLed : DisposableBase,
                              IDigitalLed
    {
        private readonly IDigitalOutput _output;

        public DigitalLed(IDigitalOutput output, string id = null) {
            _output = output;
            Id = id ?? _output.Id;
        }

        public string Id { get; private set; }

        public bool IsOn {
            get { return _output.State; }
            set { _output.Write(value); }
        }

        public void TurnOff() {
            _output.Write(false);
        }

        public void TurnOn() {
            _output.Write(true);
        }

        protected override void DisposeManagedResources() {
            _output.Dispose();
        }
    }
}