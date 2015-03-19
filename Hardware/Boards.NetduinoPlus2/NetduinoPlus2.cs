//using AnalogInput = uScoober.IO.Spot.AnalogInput;
using System;
using Microsoft.SPOT.Hardware;
using NetduinoPlus2.Tests;
using uScoober.Hardware.I2C;
using uScoober.Hardware.Input;
using uScoober.Hardware.Light;
using uScoober.Hardware.Spot;
using SL = SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace uScoober.Hardware.Boards
{
    public class NetduinoPlus2 : DisposableBase
    {
        private AnalogInputs _analogIn;
        private II2CBus _i2CBus;
        private PushButton _onboardButton;
        private AnalogLed _onboardLed;
        private PwmOutputs _pwmOut;

        public AnalogInputs AnalogIn {
            get { return _analogIn ?? (_analogIn = new AnalogInputs()); }
        }

        public II2CBus I2CBus {
            get { return _i2CBus ?? (_i2CBus = new SpotI2CBus()); }
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

        //todo: expose SpiBus
        //todo: expose storage entrypoint (via storage pack?)
        //todo: expose ethernet entrypoint? (via network pack?)

        public sealed class AnalogInputs
        {
            private IAnalogInput _a0;
            private IAnalogInput _a1;
            private IAnalogInput _a2;
            private IAnalogInput _a3;
            private IAnalogInput _a4;
            private IAnalogInput _a5;

            public IAnalogInput A0 {
                get { return _a0 ?? (_a0 = new SpotAnalogInput(Cpu.AnalogChannel.ANALOG_0)); }
            }

            public IAnalogInput A1 {
                get { return _a1 ?? (_a1 = new SpotAnalogInput(Cpu.AnalogChannel.ANALOG_1)); }
            }

            public IAnalogInput A2 {
                get { return _a2 ?? (_a2 = new SpotAnalogInput(Cpu.AnalogChannel.ANALOG_2)); }
            }

            public IAnalogInput A3 {
                get { return _a3 ?? (_a3 = new SpotAnalogInput(Cpu.AnalogChannel.ANALOG_3)); }
            }

            public IAnalogInput A4 {
                get { return _a4 ?? (_a4 = new SpotAnalogInput(Cpu.AnalogChannel.ANALOG_4)); }
            }

            public IAnalogInput A5 {
                get { return _a5 ?? (_a5 = new SpotAnalogInput(Cpu.AnalogChannel.ANALOG_5)); }
            }

            public IAnalogInput this[int index] {
                get {
                    switch (index) {
                        case 0:
                            return A0;
                        case 1:
                            return A1;
                        case 2:
                            return A2;
                        case 3:
                            return A3;
                        case 4:
                            return A4;
                        case 5:
                            return A5;
                        default:
                            throw new IndexOutOfRangeException();
                    }
                }
            }
        }

        public sealed class PwmOutputs
        {
            private IPulseWidthModulatedOutput _d10;
            private IPulseWidthModulatedOutput _d11;
            private IPulseWidthModulatedOutput _d3;
            private IPulseWidthModulatedOutput _d5;
            private IPulseWidthModulatedOutput _d6;
            private IPulseWidthModulatedOutput _d9;

            public IPulseWidthModulatedOutput D10 {
                get { return _d10 ?? (_d10 = new SpotPwmOutput(SL.PWMChannels.PWM_PIN_D10)); }
            }

            public IPulseWidthModulatedOutput D11 {
                get { return _d11 ?? (_d11 = new SpotPwmOutput(SL.PWMChannels.PWM_PIN_D11)); }
            }

            public IPulseWidthModulatedOutput D3 {
                get { return _d3 ?? (_d3 = new SpotPwmOutput(SL.PWMChannels.PWM_PIN_D3)); }
            }

            public IPulseWidthModulatedOutput D5 {
                get { return _d5 ?? (_d5 = new SpotPwmOutput(SL.PWMChannels.PWM_PIN_D5)); }
            }

            public IPulseWidthModulatedOutput D6 {
                get { return _d6 ?? (_d6 = new SpotPwmOutput(SL.PWMChannels.PWM_PIN_D6)); }
            }

            public IPulseWidthModulatedOutput D9 {
                get { return _d9 ?? (_d9 = new SpotPwmOutput(SL.PWMChannels.PWM_PIN_D9)); }
            }

            public IPulseWidthModulatedOutput this[int index] {
                get {
                    switch (index) {
                        case 3:
                            return D3;
                        case 5:
                            return D5;
                        case 6:
                            return D6;
                        case 9:
                            return D9;
                        case 10:
                            return D10;
                        case 11:
                            return D11;
                        default:
                            throw new IndexOutOfRangeException();
                    }
                }
            }
        }
    }
}