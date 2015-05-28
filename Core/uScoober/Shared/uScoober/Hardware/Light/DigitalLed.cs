using System.Diagnostics;
using System.Threading;

namespace uScoober.Hardware.Light
{
    [DebuggerDisplay("DigitalLed: {Id} IsOn:{IsOn}")]
    public class DigitalLed : DisposableBase,
                              IDigitalLed
    {
        private readonly IDigitalOutput _output;

        public DigitalLed(Pin pin, string name = null)
            : this(Signals.DigitalOutput.Bind(pin, name)) { }

        public DigitalLed(IDigitalOutput output, string name = null) {
            _output = output;
            Id = name ?? _output.Name;
        }

        public string Id { get; private set; }

        public bool IsOn {
            get { return _output.State; }
            set { _output.Write(value); }
        }

        /// <summary>
        /// Beware: THIS BLOCKS!
        /// </summary>
        /// <param name="cycleCount"></param>
        /// <param name="onTimeMilliseconds"></param>
        public void Blink(int cycleCount, int onTimeMilliseconds = 500) {
            for (int i = 0; i < cycleCount; i++) {
                TurnOn();
                Thread.Sleep(onTimeMilliseconds);
                TurnOff();
                Thread.Sleep(onTimeMilliseconds);
            }
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