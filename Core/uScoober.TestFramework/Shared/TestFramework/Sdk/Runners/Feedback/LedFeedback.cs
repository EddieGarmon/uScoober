using System;
using System.Threading;
using Microsoft.SPOT.Presentation;
using uScoober.Hardware.Light;

namespace uScoober.TestFramework.Sdk.Runners.Feedback
{
    internal class LedFeedback : ITestRunFeedback
    {
        private readonly IDigitalLed _led;
        private readonly Thread _uiUpdater;
        private TestingStatusMode _testingStatusMode = TestingStatusMode.NotStarted;
        private TestRun _testRun;

        public LedFeedback(IDigitalLed led) {
            if (_led == null) {
                throw new ArgumentNullException("led");
            }

            _led = led;
            BlinkOn(3, 100);

            _uiUpdater = new Thread(UpdateLed);
            _uiUpdater.Start();
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
            if (_testRun != null) {
                _testRun.Updated = null;
            }

            _testRun = testRun;
            _testRun.Updated = () => {
                                   if (_testRun.IsComplete) {
                                       _testingStatusMode = (_testRun.FailedCount == 0) ? TestingStatusMode.Complete : TestingStatusMode.CompleteWithFailures;
                                   }
                                   else {
                                       _testingStatusMode = (_testRun.FailedCount == 0) ? TestingStatusMode.Running : TestingStatusMode.RunningWithFailures;
                                   }
                               };

            BlinkOn(5, 50);

            _testingStatusMode = TestingStatusMode.NotStarted;
        }

        private void BlinkOn(int count, int millisecondDuration) {
            for (int i = 0; i < count; i++) {
                _led.TurnOn();
                Thread.Sleep(millisecondDuration);
                _led.TurnOff();
                Thread.Sleep(100);
            }
        }

        private void UpdateLed() {
            while (true) {
                switch (_testingStatusMode) {
                    case TestingStatusMode.NotStarted:
                        if (_led.IsOn) {
                            _led.TurnOff();
                        }
                        Thread.Sleep(10);
                        break;

                    case TestingStatusMode.Running:
                        if (!_led.IsOn) {
                            _led.TurnOn();
                        }
                        Thread.Sleep(10);
                        break;

                    case TestingStatusMode.RunningWithFailures:
                        BlinkOn(1, 500);
                        break;

                    case TestingStatusMode.Complete:
                        if (_led.IsOn) {
                            _led.TurnOff();
                        }
                        break;

                    case TestingStatusMode.CompleteWithFailures:
                        BlinkOn(_testRun.FailedCount, 200);
                        Thread.Sleep(1000);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private enum TestingStatusMode
        {
            NotStarted, //Off
            Running, //On
            RunningWithFailures, //BlinkSlow
            Complete, //Off
            CompleteWithFailures //Blink error count and pause
        }
    }
}