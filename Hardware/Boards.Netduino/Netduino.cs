using uScoober.Hardware.I2C;
using uScoober.Hardware.Light;
using uScoober.Hardware.Spi;
using uScoober.Hardware.Spot;

namespace uScoober.Hardware.Boards
{
    using SL = SecretLabs.NETMF.Hardware.Netduino;

    internal class Netduino : DisposableBase,
                              IDuino
    {
        private II2CBus _i2CBus;
        private DigitalLed _onboardLed;
        private ISpiBus _spiBus;

        public IDuinoAnalogChannels Analog {
            get { return AnalogChannels.Map; }
        }

        public II2CBus I2CBus {
            get { return _i2CBus ?? (_i2CBus = new SpotI2CBus()); }
        }

        public IDigitalInterrupt OnboardButton {
            get {
                return Signals.DigitalInterrupt.Get(Pins.OnboardButton)
                       ?? Signals.DigitalInterrupt.Bind(Pins.OnboardButton, "on-board button", ResistorMode.PullUp, InterruptMode.InterruptEdgeBoth);
            }
        }

        public DigitalLed OnboardLed {
            get { return _onboardLed ?? (_onboardLed = new DigitalLed(Pins.OnboardLed)); }
        }

        public IDuinoPins Pins {
            get { return DigitalPins.Map; }
        }

        public IDuinoPwmChannels Pwm {
            get { return PwmChannels.Map; }
        }

        public ISpiBus SpiBus {
            get { return _spiBus ?? (_spiBus = new SpotSpiBus(SL.SPI_Devices.SPI1)); }
        }

        IDigitalLed IDuino.OnboardLed {
            get { return OnboardLed; }
        }

        protected override void DisposeManagedResources() {
            if (_i2CBus != null) {
                _i2CBus.Dispose();
                _i2CBus = null;
            }
            if (_spiBus != null) {
                _spiBus.Dispose();
                _spiBus = null;
            }
            if (_onboardLed != null) {
                _onboardLed.Dispose();
                _onboardLed = null;
            }
        }

        internal sealed class AnalogChannels : IDuinoAnalogChannels
        {
            internal static readonly AnalogChannels Map = new AnalogChannels();

            private AnalogChannels() { }

            public AnalogChannel PinA0 {
                get { return (AnalogChannel)SL.AnalogChannels.ANALOG_PIN_A0; }
            }

            public AnalogChannel PinA1 {
                get { return (AnalogChannel)SL.AnalogChannels.ANALOG_PIN_A1; }
            }

            public AnalogChannel PinA2 {
                get { return (AnalogChannel)SL.AnalogChannels.ANALOG_PIN_A2; }
            }

            public AnalogChannel PinA3 {
                get { return (AnalogChannel)SL.AnalogChannels.ANALOG_PIN_A3; }
            }

            public AnalogChannel PinA4 {
                get { return (AnalogChannel)SL.AnalogChannels.ANALOG_PIN_A4; }
            }

            public AnalogChannel PinA5 {
                get { return (AnalogChannel)SL.AnalogChannels.ANALOG_PIN_A5; }
            }
        }

        internal sealed class DigitalPins : IDuinoPins
        {
            internal static readonly DigitalPins Map = new DigitalPins();

            private DigitalPins() { }

            public Pin A0 {
                get { return (Pin)SL.Pins.GPIO_PIN_A0; }
            }

            public Pin A1 {
                get { return (Pin)SL.Pins.GPIO_PIN_A1; }
            }

            public Pin A2 {
                get { return (Pin)SL.Pins.GPIO_PIN_A2; }
            }

            public Pin A3 {
                get { return (Pin)SL.Pins.GPIO_PIN_A3; }
            }

            public Pin A4 {
                get { return (Pin)SL.Pins.GPIO_PIN_A4; }
            }

            public Pin A5 {
                get { return (Pin)SL.Pins.GPIO_PIN_A5; }
            }

            public Pin D0 {
                get { return (Pin)SL.Pins.GPIO_PIN_D0; }
            }

            public Pin D1 {
                get { return (Pin)SL.Pins.GPIO_PIN_D1; }
            }

            public Pin D10 {
                get { return (Pin)SL.Pins.GPIO_PIN_D10; }
            }

            public Pin D11 {
                get { return (Pin)SL.Pins.GPIO_PIN_D11; }
            }

            public Pin D12 {
                get { return (Pin)SL.Pins.GPIO_PIN_D12; }
            }

            public Pin D13 {
                get { return (Pin)SL.Pins.GPIO_PIN_D13; }
            }

            public Pin D2 {
                get { return (Pin)SL.Pins.GPIO_PIN_D2; }
            }

            public Pin D3 {
                get { return (Pin)SL.Pins.GPIO_PIN_D3; }
            }

            public Pin D4 {
                get { return (Pin)SL.Pins.GPIO_PIN_D4; }
            }

            public Pin D5 {
                get { return (Pin)SL.Pins.GPIO_PIN_D5; }
            }

            public Pin D6 {
                get { return (Pin)SL.Pins.GPIO_PIN_D6; }
            }

            public Pin D7 {
                get { return (Pin)SL.Pins.GPIO_PIN_D7; }
            }

            public Pin D8 {
                get { return (Pin)SL.Pins.GPIO_PIN_D8; }
            }

            public Pin D9 {
                get { return (Pin)SL.Pins.GPIO_PIN_D9; }
            }

            public Pin OnboardButton {
                get { return (Pin)SL.Pins.ONBOARD_BTN; }
            }

            public Pin OnboardLed {
                get { return (Pin)SL.Pins.ONBOARD_LED; }
            }
        }

        internal sealed class PwmChannels : IDuinoPwmChannels
        {
            internal static readonly PwmChannels Map = new PwmChannels();

            private PwmChannels() { }

            public PwmChannel PinD10 {
                get { return (PwmChannel)SL.PWMChannels.PWM_PIN_D10; }
            }

            public PwmChannel PinD11 {
                get { return (PwmChannel)SL.PWMChannels.PWM_PIN_D11; }
            }

            public PwmChannel PinD3 {
                get { return (PwmChannel)SL.PWMChannels.PWM_PIN_D3; }
            }

            public PwmChannel PinD5 {
                get { return (PwmChannel)SL.PWMChannels.PWM_PIN_D5; }
            }

            public PwmChannel PinD6 {
                get { return (PwmChannel)SL.PWMChannels.PWM_PIN_D6; }
            }

            public PwmChannel PinD9 {
                get { return (PwmChannel)SL.PWMChannels.PWM_PIN_D9; }
            }
        }
    }
}