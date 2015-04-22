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

    internal class NetduinoPlus2 : DisposableBase,
                                   IDuino
    {
        private AnalogInputs _analogIn;
        private DigitalInputs _digitalIn;
        private DigitalOutputs _digitalOut;
        private II2CBus _i2CBus;
        private PushButton _onboardButton;
        private AnalogLed _onboardLed;
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
                           new PushButton(new SpotDigitalInterupt(SL.Pins.ONBOARD_SW1,
                                                                  "on-board button",
                                                                  (Port.ResistorMode)ResistorMode.PullUp,
                                                                  (Port.InterruptMode)InterruptMode.InterruptEdgeLevelLow)));
            }
        }

        public AnalogLed OnboardLed {
            get { return _onboardLed ?? (_onboardLed = new AnalogLed(new SpotPwmOutput(SL.PWMChannels.PWM_ONBOARD_LED))); }
        }

        public PwmOutputs PwmOut {
            get { return _pwmOut ?? (_pwmOut = new PwmOutputs()); }
        }

        public ISpiBus SpiBus {
            get { return _spiBus ?? (_spiBus = new SpotSpiBus(SPI.SPI_module.SPI1)); }
        }

        //todo: expose storage entrypoint (via storage pack?)
        //todo: expose ethernet entrypoint? (via network pack?)

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

        internal sealed class AnalogInputs : SpotDuinoAnalogInputs
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

        internal static class InterruptablePins
        {
            public static readonly Cpu.Pin D0 = SL.Pins.GPIO_PIN_D0;
            public static readonly Cpu.Pin D1 = SL.Pins.GPIO_PIN_D1;
            public static readonly Cpu.Pin D10 = SL.Pins.GPIO_PIN_D10;
            public static readonly Cpu.Pin D11 = SL.Pins.GPIO_PIN_D11;
            public static readonly Cpu.Pin D12 = SL.Pins.GPIO_PIN_D12;
            public static readonly Cpu.Pin D13 = SL.Pins.GPIO_PIN_D13;
            public static readonly Cpu.Pin D2 = SL.Pins.GPIO_PIN_D2;
            public static readonly Cpu.Pin D3 = SL.Pins.GPIO_PIN_D3;
            public static readonly Cpu.Pin D4 = SL.Pins.GPIO_PIN_D4;
            public static readonly Cpu.Pin D5 = SL.Pins.GPIO_PIN_D5;
            public static readonly Cpu.Pin D6 = SL.Pins.GPIO_PIN_D6;
            public static readonly Cpu.Pin D7 = SL.Pins.GPIO_PIN_D7;
            public static readonly Cpu.Pin D8 = SL.Pins.GPIO_PIN_D8;
            public static readonly Cpu.Pin D9 = SL.Pins.GPIO_PIN_D9;
            public static readonly Cpu.Pin OnboardSwitch = SL.Pins.ONBOARD_SW1;
        }

        internal static class Pins
        {
            public static readonly Cpu.Pin A0 = SL.Pins.GPIO_PIN_A0;
            public static readonly Cpu.Pin A1 = SL.Pins.GPIO_PIN_A1;
            public static readonly Cpu.Pin A2 = SL.Pins.GPIO_PIN_A2;
            public static readonly Cpu.Pin A3 = SL.Pins.GPIO_PIN_A3;
            public static readonly Cpu.Pin A4 = SL.Pins.GPIO_PIN_A4;
            public static readonly Cpu.Pin A5 = SL.Pins.GPIO_PIN_A5;
            public static readonly Cpu.Pin I2C_SCL = SL.Pins.GPIO_PIN_SCL;
            public static readonly Cpu.Pin I2C_SDA = SL.Pins.GPIO_PIN_SDA;
            public static readonly Cpu.Pin Led = SL.Pins.ONBOARD_LED;
        }

        public sealed class PwmOutputs : SpotDuinoPwmOutputs
        {
            protected override Cpu.PWMChannel PinD10 {
                get { return Cpu.PWMChannel.PWM_3; }
            }

            protected override Cpu.PWMChannel PinD11 {
                get { return Cpu.PWMChannel.PWM_NONE; }
            }

            protected override Cpu.PWMChannel PinD3 {
                get { return Cpu.PWMChannel.PWM_NONE; }
            }

            protected override Cpu.PWMChannel PinD5 {
                get { return Cpu.PWMChannel.PWM_0; }
            }

            protected override Cpu.PWMChannel PinD6 {
                get { return Cpu.PWMChannel.PWM_1; }
            }

            protected override Cpu.PWMChannel PinD9 {
                get { return Cpu.PWMChannel.PWM_2; }
            }
        }
    }
}