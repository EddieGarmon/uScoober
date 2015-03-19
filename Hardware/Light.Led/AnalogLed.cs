namespace uScoober.Hardware.Light
{
    public class AnalogLed : DisposableBase,
                             IAnalogLed,
                             IDigitalLed
    {
        private readonly IPulseWidthModulatedOutput _output;

        public AnalogLed(IPulseWidthModulatedOutput output) {
            _output = output;
        }

        public double DutyCycle {
            get { return _output.DutyCycle; }
            set { _output.DutyCycle = value; }
        }

        public bool IsOn {
            get { return _output.IsActive; }
            set {
                if (value) {
                    TurnOn();
                }
                else {
                    TurnOff();
                }
            }
        }

        public void FadeOff(ushort durationMilliseconds = 1000) {
            _output.RampTo(0, durationMilliseconds);
            _output.Stop();
        }

        public void FadeOn(double dutyCycle = 1, ushort durationMilliseconds = 1000) {
            if (!_output.IsActive) {
                _output.DutyCycle = 0;
                _output.Start();
            }
            _output.RampTo(dutyCycle, durationMilliseconds);
        }

        public void TurnOff() {
            _output.Stop();
        }

        public void TurnOn(double dutyCycle = 1) {
            _output.DutyCycle = dutyCycle;
            _output.Start();
        }

        protected override void DisposeManagedResources() {
            _output.Dispose();
        }

        void IDigitalLed.TurnOn() {
            TurnOn();
        }
    }
}