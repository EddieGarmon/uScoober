using Microsoft.SPOT.Hardware;
using uScoober.Hardware.Boards.Spot;
using uScoober.Hardware.I2C;
using uScoober.Hardware.Input;
using uScoober.Hardware.Light;
using uScoober.Hardware.Spi;
using uScoober.Hardware.Spot;

namespace uScoober.Hardware.Boards
{
    using SL = SecretLabs.NETMF.Hardware.Netduino;

    internal class Netduino : DisposableBase,
                              IDuino
    {
        private AnalogInputs _analogIn;
        private DigitalInputs _digitalIn;
        private DigitalOutputs _digitalOut;
        private II2CBus _i2CBus;
        private PushButton _onboardButton;
        private DigitalLed _onboardLed;
        private PwmOutputs _pwmOut;
        private ISpiBus _spiBus;

        public AnalogInputs AnalogIn {
            get { return _analogIn ?? (_analogIn = new AnalogInputs()); }
        }

        public DigitalInputs DigitalIn {
            get { return _digitalIn ?? (_digitalIn = new DigitalInputs()); }
        }

        public DigitalOutputs DigitalOut {
            get { return _digitalOut ?? (_digitalOut = new DigitalOutputs()); }
        }

        public II2CBus I2CBus {
            get { return _i2CBus ?? (_i2CBus = new SpotI2CBus()); }
        }

        public PushButton OnboardButton {
            get {
                return _onboardButton
                       ?? (_onboardButton =
                           new PushButton(new SpotDigitalInterupt(SL.Pins.ONBOARD_BTN,
                                                                  "on-board button",
                                                                  (Port.ResistorMode)ResistorMode.PullUp,
                                                                  (Port.InterruptMode)InterruptMode.InterruptEdgeLevelLow)));
            }
        }

        public DigitalLed OnboardLed {
            get { return _onboardLed ?? (_onboardLed = new DigitalLed(new SpotDigitalOutput(SL.Pins.ONBOARD_LED))); }
        }

        public PwmOutputs PwmOut {
            get { return _pwmOut ?? (_pwmOut = new PwmOutputs()); }
        }

        public ISpiBus SpiBus {
            get { return _spiBus ?? (_spiBus = new SpotSpiBus(SL.SPI_Devices.SPI1)); }
        }

        IDuinoAnalogInputs IDuino.AnalogIn {
            get { return AnalogIn; }
        }

        IDuinoDigitalInputs IDuino.DigitalIn {
            get { return DigitalIn; }
        }

        IDuinoDigitalOutputs IDuino.DigitalOut {
            get { return DigitalOut; }
        }

        IButton IDuino.OnboardButton {
            get { return OnboardButton; }
        }

        IDigitalLed IDuino.OnboardLed {
            get { return OnboardLed; }
        }

        IDuinoPwmOutputs IDuino.PwmOut {
            get { return PwmOut; }
        }

        protected override void DisposeManagedResources() {
            if (_analogIn != null) {
                _analogIn.Dispose();
                _analogIn = null;
            }
            if (_digitalIn != null) {
                _digitalIn.Dispose();
                _digitalIn = null;
            }
            if (_digitalOut != null) {
                _digitalOut.Dispose();
                _digitalOut = null;
            }
            if (_pwmOut != null) {
                _pwmOut.Dispose();
                _pwmOut = null;
            }
            if (_i2CBus != null) {
                _i2CBus.Dispose();
                _i2CBus = null;
            }
            if (_spiBus != null) {
                _spiBus.Dispose();
                _spiBus = null;
            }
            if (_onboardButton != null) {
                _onboardButton.Dispose();
                _onboardButton = null;
            }
            if (_onboardLed != null) {
                _onboardLed.Dispose();
                _onboardLed = null;
            }
        }

        public sealed class AnalogInputs : SpotDuinoAnalogInputs
        {
            protected override Cpu.AnalogChannel PinA0 {
                get { return SL.AnalogChannels.ANALOG_PIN_A0; }
            }

            protected override Cpu.AnalogChannel PinA1 {
                get { return SL.AnalogChannels.ANALOG_PIN_A1; }
            }

            protected override Cpu.AnalogChannel PinA2 {
                get { return SL.AnalogChannels.ANALOG_PIN_A2; }
            }

            protected override Cpu.AnalogChannel PinA3 {
                get { return SL.AnalogChannels.ANALOG_PIN_A3; }
            }

            protected override Cpu.AnalogChannel PinA4 {
                get { return SL.AnalogChannels.ANALOG_PIN_A4; }
            }

            protected override Cpu.AnalogChannel PinA5 {
                get { return SL.AnalogChannels.ANALOG_PIN_A5; }
            }
        }

        public sealed class DigitalInputs : SpotDuinoDigitalInputs { }

        public sealed class DigitalOutputs : SpotDuinoDigitalOutputs { }

        internal sealed class PwmOutputs : SpotDuinoPwmOutputs
        {
            protected override Cpu.PWMChannel PinD10 {
                get { return SL.PWMChannels.PWM_PIN_D10; }
            }

            protected override Cpu.PWMChannel PinD11 {
                get { return SL.PWMChannels.PWM_PIN_D11; }
            }

            protected override Cpu.PWMChannel PinD3 {
                get { return SL.PWMChannels.PWM_PIN_D3; }
            }

            protected override Cpu.PWMChannel PinD5 {
                get { return SL.PWMChannels.PWM_PIN_D5; }
            }

            protected override Cpu.PWMChannel PinD6 {
                get { return SL.PWMChannels.PWM_PIN_D6; }
            }

            protected override Cpu.PWMChannel PinD9 {
                get { return SL.PWMChannels.PWM_PIN_D9; }
            }
        }
    }
}