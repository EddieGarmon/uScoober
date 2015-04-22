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
                           new PushButton(new SpotDigitalInterupt(InterruptablePins.OnboardSwitch,
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
                get { return Cpu.AnalogChannel.ANALOG_0; }
            }

            protected override Cpu.AnalogChannel PinA1 {
                get { return Cpu.AnalogChannel.ANALOG_1; }
            }

            protected override Cpu.AnalogChannel PinA2 {
                get { return Cpu.AnalogChannel.ANALOG_2; }
            }

            protected override Cpu.AnalogChannel PinA3 {
                get { return Cpu.AnalogChannel.ANALOG_3; }
            }

            protected override Cpu.AnalogChannel PinA4 {
                get { return Cpu.AnalogChannel.ANALOG_4; }
            }

            protected override Cpu.AnalogChannel PinA5 {
                get { return Cpu.AnalogChannel.ANALOG_5; }
            }
        }

        public sealed class DigitalInputs : SpotDuinoDigitalInputs { }

        public sealed class DigitalOutputs : SpotDuinoDigitalOutputs { }

        internal static class InterruptablePins
        {
            public const Cpu.Pin D0 = (Cpu.Pin)27;
            public const Cpu.Pin D1 = (Cpu.Pin)28;
            public const Cpu.Pin D10 = (Cpu.Pin)54;
            public const Cpu.Pin D11 = (Cpu.Pin)17;
            public const Cpu.Pin D12 = (Cpu.Pin)16;
            public const Cpu.Pin D13 = (Cpu.Pin)18;
            public const Cpu.Pin D2 = Cpu.Pin.GPIO_Pin0;
            public const Cpu.Pin D3 = Cpu.Pin.GPIO_Pin1;
            public const Cpu.Pin D4 = Cpu.Pin.GPIO_Pin12;
            public const Cpu.Pin D5 = (Cpu.Pin)51;
            public const Cpu.Pin D6 = (Cpu.Pin)52;
            public const Cpu.Pin D7 = Cpu.Pin.GPIO_Pin3;
            public const Cpu.Pin D8 = Cpu.Pin.GPIO_Pin4;
            public const Cpu.Pin D9 = (Cpu.Pin)53;
            public const Cpu.Pin OnboardSwitch = (Cpu.Pin)29;
        }

        internal static class Pins
        {
            public const Cpu.Pin A0 = (Cpu.Pin)59;
            public const Cpu.Pin A1 = (Cpu.Pin)60;
            public const Cpu.Pin A2 = (Cpu.Pin)61;
            public const Cpu.Pin A3 = (Cpu.Pin)62;
            public const Cpu.Pin A4 = Cpu.Pin.GPIO_Pin10;
            public const Cpu.Pin A5 = Cpu.Pin.GPIO_Pin11;
            public const Cpu.Pin OnboardLed = (Cpu.Pin)55;
        }

        internal sealed class PwmOutputs : SpotDuinoPwmOutputs
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