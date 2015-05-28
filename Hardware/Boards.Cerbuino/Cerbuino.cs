using Microsoft.SPOT.Hardware;
using uScoober.Hardware.I2C;
using uScoober.Hardware.Light;
using uScoober.Hardware.Spi;
using uScoober.Hardware.Spot;

namespace uScoober.Hardware.Boards
{
    internal class Cerbuino : DisposableBase,
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
            get { return _onboardLed ?? (_onboardLed = new DigitalLed(Pins.OnboardLed, "on-board led")); }
        }

        public IDuinoPins Pins {
            get { return DigitalPins.Map; }
        }

        public IDuinoPwmChannels Pwm {
            get { return PwmChannels.Map; }
        }

        public ISpiBus SpiBus {
            get { return _spiBus ?? (_spiBus = new SpotSpiBus(SPI.SPI_module.SPI1)); }
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
                get { return (AnalogChannel)9; }
            }

            public AnalogChannel PinA1 {
                get { return (AnalogChannel)5; }
            }

            public AnalogChannel PinA2 {
                get { return (AnalogChannel)8; }
            }

            public AnalogChannel PinA3 {
                get { return (AnalogChannel)13; }
            }

            public AnalogChannel PinA4 {
                get { return (AnalogChannel)11; }
            }

            public AnalogChannel PinA5 {
                get { return (AnalogChannel)4; }
            }
        }

        internal sealed class DigitalPins : IDuinoPins
        {
            internal static readonly DigitalPins Map = new DigitalPins();

            private DigitalPins() { }

            public Pin A0 {
                get { return (Pin)17; }
            }

            public Pin A1 {
                get { return (Pin)5; }
            }

            public Pin A2 {
                get { return (Pin)16; }
            }

            public Pin A3 {
                get { return (Pin)35; }
            }

            public Pin A4 {
                get { return (Pin)33; }
            }

            public Pin A5 {
                get { return (Pin)4; }
            }

            public Pin D0 {
                get { return (Pin)27; }
            }

            public Pin D1 {
                get { return (Pin)26; }
            }

            public Pin D10 {
                get { return (Pin)15; }
            }

            public Pin D11 {
                get { return (Pin)21; }
            }

            public Pin D12 {
                get { return (Pin)20; }
            }

            public Pin D13 {
                get { return (Pin)19; }
            }

            public Pin D2 {
                get { return (Pin)28; }
            }

            public Pin D3 {
                get { return (Pin)46; }
            }

            public Pin D4 {
                get { return (Pin)47; }
            }

            public Pin D5 {
                get { return (Pin)8; }
            }

            public Pin D6 {
                get { return (Pin)10; }
            }

            public Pin D7 {
                get { return (Pin)36; }
            }

            public Pin D8 {
                get { return (Pin)29; }
            }

            public Pin D9 {
                get { return (Pin)9; }
            }

            public Pin OnboardButton {
                get { return (Pin.None); }
            }

            public Pin OnboardLed {
                get { return (Pin)18; }
            }
        }

        internal sealed class PwmChannels : IDuinoPwmChannels
        {
            internal static readonly PwmChannels Map = new PwmChannels();

            private PwmChannels() { }

            public PwmChannel PinD10 {
                get { return (PwmChannel)13; }
            }

            public PwmChannel PinD11 {
                get { return (PwmChannel)6; }
            }

            public PwmChannel PinD3 {
                get { return PwmChannel.None; }
            }

            public PwmChannel PinD5 {
                get { return (PwmChannel)3; }
            }

            public PwmChannel PinD6 {
                get { return (PwmChannel)11; }
            }

            public PwmChannel PinD9 {
                get { return (PwmChannel)12; }
            }
        }
    }
}