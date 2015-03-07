using System;
using System.Threading;
using Microsoft.SPOT.Presentation;
using uScoober.Hardware.Light;

namespace uScoober.TestFramework.Sdk.Runners.Feedback
{
    internal class AnalogLedFeedback : ITestRunFeedback
    {
        private readonly IAnalogLed _led;
        private double _dutyCycle;
        private LedMode _ledMode;
        private TestRun _testRun;
        private Thread _uiUpdater;

        public AnalogLedFeedback(IAnalogLed led) {
            _led = led;
            _led.TurnOn();
            Thread.Sleep(100);
            _led.TurnOff();
            Thread.Sleep(100);
            _led.TurnOn();
        }

        public bool IsGui {
            get { return false; }
        }

        public UIElement InitializeGui() {
            return null;
        }

        public void ScrollDown() { }

        public void ScrollUp() { }

        public void UseTestRun(TestRun testRun) {
            _testRun = testRun;
            _testRun.Updated = () => {
                                   if (_testRun.IsComplete) {
                                       _ledMode = LedMode.ShowErrorCount;
                                       return;
                                   }
                                   if (_testRun.FailedCount > 0) {
                                       if (_ledMode != LedMode.BlinkAndFadeOn && _ledMode != LedMode.BlinkAndFadeOff) {
                                           _ledMode = LedMode.BlinkAndFadeOn;
                                       }
                                       return;
                                   }
                                   if (_ledMode != LedMode.FadeOn && _ledMode != LedMode.FadeOff) {
                                       _ledMode = LedMode.FadeOn;
                                   }
                               };
            _led.TurnOn();
            Thread.Sleep(100);
            _led.TurnOff();
            Thread.Sleep(100);
            _led.TurnOn();
            if (_uiUpdater != null) {
                return;
            }
            _uiUpdater = new Thread(UpdateLed);
            _uiUpdater.Start();
        }

        private void UpdateLed() {
            while (true) {
                switch (_ledMode) {
                    case LedMode.Blink:
                        _led.DutyCycle = _dutyCycle = _dutyCycle > 0 ? 0 : 1;
                        break;
                    case LedMode.FadeOn:
                        _dutyCycle += 0.01;
                        if (_dutyCycle >= 1) {
                            _dutyCycle = 1;
                            _ledMode = LedMode.FadeOff;
                        }
                        _led.DutyCycle = _dutyCycle;
                        break;
                    case LedMode.FadeOff:
                        _dutyCycle -= 0.01;
                        if (_dutyCycle <= 0) {
                            _dutyCycle = 0;
                            _ledMode = LedMode.FadeOn;
                        }
                        _led.DutyCycle = _dutyCycle;
                        break;
                    case LedMode.BlinkAndFadeOn:
                        _dutyCycle += 0.01;
                        if (_dutyCycle >= 1) {
                            _dutyCycle = 1;
                            _ledMode = LedMode.FadeOff;
                        }
                        _led.DutyCycle = ((int)(_dutyCycle * 10)) % 2 == 1 ? 0 : _dutyCycle;
                        break;
                    case LedMode.BlinkAndFadeOff:
                        _dutyCycle -= 0.01;
                        if (_dutyCycle <= 0) {
                            _dutyCycle = 0;
                            _ledMode = LedMode.FadeOn;
                        }
                        _led.DutyCycle = ((int)(_dutyCycle * 10)) % 2 == 1 ? 0 : _dutyCycle;
                        break;
                    case LedMode.ShowErrorCount:
                        if (_testRun.FailedCount == 0) {
                            _led.DutyCycle = _dutyCycle = 0;
                            break;
                        }
                        for (int i = 0; i < _testRun.FailedCount; i++) {
                            _led.DutyCycle = 0;
                            Thread.Sleep(200);
                            _led.DutyCycle = 1;
                            Thread.Sleep(200);
                        }
                        _led.DutyCycle = 0;
                        Thread.Sleep(1000);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Thread.Sleep(10);
            }
        }

        private enum LedMode
        {
            Blink,
            FadeOn,
            FadeOff,
            BlinkAndFadeOn,
            BlinkAndFadeOff,
            ShowErrorCount
        }
    }
}