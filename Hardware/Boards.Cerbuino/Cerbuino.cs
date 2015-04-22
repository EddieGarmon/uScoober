using Microsoft.SPOT.Hardware;
using uScoober.Hardware.Boards.Spot;
using uScoober.Hardware.I2C;
using uScoober.Hardware.Input;
using uScoober.Hardware.Light;
using uScoober.Hardware.Spi;
using uScoober.Hardware.Spot;

namespace uScoober.Hardware.Boards
{
    internal class Cerbuino : DisposableBase,
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
                           new PushButton(new SpotDigitalInterupt(InterruptablePins.OnboardButton,
                                                                  "on-board button",
                                                                  (Port.ResistorMode)ResistorMode.PullUp,
                                                                  (Port.InterruptMode)InterruptMode.InterruptEdgeLevelLow)));
            }
        }

        public DigitalLed OnboardLed {
            get { return _onboardLed ?? (_onboardLed = new DigitalLed(new SpotDigitalOutput(Pins.OnboardLed, false, "on-board led"))); }
        }

        public PwmOutputs PwmOut {
            get { return _pwmOut ?? (_pwmOut = new PwmOutputs()); }
        }

        public ISpiBus SpiBus {
            get { return _spiBus ?? (_spiBus = new SpotSpiBus(SPI.SPI_module.SPI1)); }
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

        internal sealed class AnalogInputs : SpotDuinoAnalogInputs
        {
            protected override Cpu.AnalogChannel PinA0 {
                get { return (Cpu.AnalogChannel)9; }
            }

            protected override Cpu.AnalogChannel PinA1 {
                get { return (Cpu.AnalogChannel)5; }
            }

            protected override Cpu.AnalogChannel PinA2 {
                get { return (Cpu.AnalogChannel)8; }
            }

            protected override Cpu.AnalogChannel PinA3 {
                get { return (Cpu.AnalogChannel)13; }
            }

            protected override Cpu.AnalogChannel PinA4 {
                get { return (Cpu.AnalogChannel)11; }
            }

            protected override Cpu.AnalogChannel PinA5 {
                get { return (Cpu.AnalogChannel)4; }
            }
        }

        internal sealed class DigitalInputs : SpotDuinoDigitalInputs { }

        internal sealed class DigitalOutputs : SpotDuinoDigitalOutputs { }

        internal static class InterruptablePins
        {
            public const Cpu.Pin D0 = (Cpu.Pin)27;
            public const Cpu.Pin D1 = (Cpu.Pin)26;
            public const Cpu.Pin D10 = Cpu.Pin.GPIO_Pin15;
            public const Cpu.Pin D11 = (Cpu.Pin)21;
            public const Cpu.Pin D12 = (Cpu.Pin)20;
            public const Cpu.Pin D13 = (Cpu.Pin)19;
            public const Cpu.Pin D2 = (Cpu.Pin)28;
            public const Cpu.Pin D3 = (Cpu.Pin)46;
            public const Cpu.Pin D4 = (Cpu.Pin)47;
            public const Cpu.Pin D5 = Cpu.Pin.GPIO_Pin8;
            public const Cpu.Pin D6 = Cpu.Pin.GPIO_Pin10;
            public const Cpu.Pin D7 = (Cpu.Pin)36;
            public const Cpu.Pin D8 = (Cpu.Pin)29;
            public const Cpu.Pin D9 = Cpu.Pin.GPIO_Pin9;
            public const Cpu.Pin OnboardButton = Cpu.Pin.GPIO_NONE;
        }

        internal static class Pins
        {
            public const Cpu.Pin A0 = (Cpu.Pin)17;
            public const Cpu.Pin A1 = Cpu.Pin.GPIO_Pin5;
            public const Cpu.Pin A2 = (Cpu.Pin)16;
            public const Cpu.Pin A3 = (Cpu.Pin)35;
            public const Cpu.Pin A4 = (Cpu.Pin)33;
            public const Cpu.Pin A5 = Cpu.Pin.GPIO_Pin4;
            public const Cpu.Pin OnboardLed = (Cpu.Pin)18;
        }

        internal sealed class PwmOutputs : SpotDuinoPwmOutputs
        {
            private IPulseWidthModulatedOutput _a0;
            private IPulseWidthModulatedOutput _a2;
            private IPulseWidthModulatedOutput _d0;
            private IPulseWidthModulatedOutput _d1;
            private IPulseWidthModulatedOutput _d13;

            public IPulseWidthModulatedOutput A0 {
                get { return _a0 ?? (_a0 = Create((Cpu.PWMChannel)5)); }
            }

            public IPulseWidthModulatedOutput A2 {
                get { return _a2 ?? (_a2 = Create((Cpu.PWMChannel)4)); }
            }

            public IPulseWidthModulatedOutput D0 {
                get { return _d0 ?? (_d0 = Create((Cpu.PWMChannel)9)); }
            }

            public IPulseWidthModulatedOutput D1 {
                get { return _d1 ?? (_d1 = Create((Cpu.PWMChannel)10)); }
            }

            public IPulseWidthModulatedOutput D13 {
                get { return _d13 ?? (_d13 = Create((Cpu.PWMChannel)8)); }
            }

            protected override Cpu.PWMChannel PinD10 {
                get { return (Cpu.PWMChannel)13; }
            }

            protected override Cpu.PWMChannel PinD11 {
                get { return (Cpu.PWMChannel)6; }
            }

            //todo: should this throw?
            protected override Cpu.PWMChannel PinD3 {
                get { return Cpu.PWMChannel.PWM_NONE; }
            }

            protected override Cpu.PWMChannel PinD5 {
                get { return (Cpu.PWMChannel)3; }
            }

            protected override Cpu.PWMChannel PinD6 {
                get { return (Cpu.PWMChannel)11; }
            }

            protected override Cpu.PWMChannel PinD9 {
                get { return (Cpu.PWMChannel)12; }
            }
        }
    }
}