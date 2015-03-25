using System;
using System.Collections;
using Microsoft.SPOT.Hardware;
using NetduinoPlus2.Tests;
using uScoober.Hardware.I2C;
using uScoober.Hardware.Input;
using uScoober.Hardware.Light;
using uScoober.Hardware.Spi;
using uScoober.Hardware.Spot;
using SL = SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace uScoober.Hardware.Boards
{
    internal class NetduinoPlus2 : DisposableBase
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

        public ISpiBus ISpiBus {
            get { return _spiBus ?? (_spiBus = new SpotSpiBus(SL.SPI_Devices.SPI1)); }
        }

        public PushButton OnboardButton {
            get {
                return _onboardButton
                       ?? (_onboardButton =
                           new PushButton(new SpotDigitalInput(SL.Pins.ONBOARD_SW1, "on-board button", ResistorMode.PullUp, InterruptMode.InterruptEdgeLevelLow)));
            }
        }

        public AnalogLed OnboardLed {
            get { return _onboardLed ?? (_onboardLed = new AnalogLed(new SpotPwmOutput(SL.PWMChannels.PWM_ONBOARD_LED))); }
        }

        public PwmOutputs PwmOut {
            get { return _pwmOut ?? (_pwmOut = new PwmOutputs()); }
        }

        protected override void DisposeManagedResources() {
            if (_i2CBus != null) {
                _i2CBus.Dispose();
                _i2CBus = null;
            }
        }

        //todo: expose storage entrypoint (via storage pack?)
        //todo: expose ethernet entrypoint? (via network pack?)

        public sealed class AnalogInputs
        {
            private readonly IAnalogInput[] _store = new IAnalogInput[6];

            public IAnalogInput A0 {
                get { return GetOrCreate(0); }
            }

            public IAnalogInput A1 {
                get { return GetOrCreate(1); }
            }

            public IAnalogInput A2 {
                get { return GetOrCreate(2); }
            }

            public IAnalogInput A3 {
                get { return GetOrCreate(3); }
            }

            public IAnalogInput A4 {
                get { return GetOrCreate(4); }
            }

            public IAnalogInput A5 {
                get { return GetOrCreate(5); }
            }

            public DigitalInputs DigitalInput {
                get { return new DigitalInputs(); }
            }

            public IAnalogInput this[int index] {
                get {
                    if (index < 0 || index > 5) {
                        throw new IndexOutOfRangeException();
                    }
                    return GetOrCreate(index);
                }
            }

            private IAnalogInput GetOrCreate(int index) {
                return _store[index] ?? (_store[index] = new SpotAnalogInput((Cpu.AnalogChannel)index));
            }
        }

        public sealed class DigitalInputs
        {
            //todo: should we really be tracking these?
            private readonly Hashtable _store = new Hashtable();

            public IDigitalInput Bind(Cpu.Pin pin, string id, ResistorMode internalResistorMode, InterruptMode interruptMode, int debounceMilliseconds = 0) {
                var input = new SpotDigitalInput(pin, id, internalResistorMode, interruptMode, debounceMilliseconds);
                _store.Add(pin, input);
                return input;
            }

            public IDigitalInput Get(Cpu.Pin pin) {
                return (IDigitalInput)_store[pin];
            }
        }

        public sealed class DigitalOutputs
        {
            //todo: should we really be tracking these?
            private readonly Hashtable _store = new Hashtable();

            public IDigitalOutput Bind(Cpu.Pin pin, bool initialState = false, string id = null) {
                var output = new SpotDigitalOutput(pin, initialState, id);
                _store.Add(pin, output);
                return output;
            }

            public IDigitalOutput Get(Cpu.Pin pin) {
                return (IDigitalOutput)_store[pin];
            }
        }

        public sealed class PwmOutputs
        {
            private readonly IPulseWidthModulatedOutput[] _store = new IPulseWidthModulatedOutput[6];

            public IPulseWidthModulatedOutput D10 {
                get { return _store[4] ?? (_store[4] = new SpotPwmOutput(SL.PWMChannels.PWM_PIN_D10)); }
            }

            public IPulseWidthModulatedOutput D11 {
                get { return _store[5] ?? (_store[5] = new SpotPwmOutput(SL.PWMChannels.PWM_PIN_D11)); }
            }

            public IPulseWidthModulatedOutput D3 {
                get { return _store[0] ?? (_store[0] = new SpotPwmOutput(SL.PWMChannels.PWM_PIN_D3)); }
            }

            public IPulseWidthModulatedOutput D5 {
                get { return _store[1] ?? (_store[1] = new SpotPwmOutput(SL.PWMChannels.PWM_PIN_D5)); }
            }

            public IPulseWidthModulatedOutput D6 {
                get { return _store[2] ?? (_store[2] = new SpotPwmOutput(SL.PWMChannels.PWM_PIN_D6)); }
            }

            public IPulseWidthModulatedOutput D9 {
                get { return _store[3] ?? (_store[3] = new SpotPwmOutput(SL.PWMChannels.PWM_PIN_D9)); }
            }
        }
    }
}