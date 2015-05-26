using System;
using System.Threading;
using uScoober.Hardware.Light;
using uScoober.TestFramework.Core;

namespace uScoober.TestFramework.UI
{
    internal class FeedbackToLed : IRunnerResultProcessor
    {
        private readonly IDigitalLed _led;
        private readonly Thread _uiUpdater;
        private TestRunResult _runResults;
        private TestingStatusMode _testingStatusMode = TestingStatusMode.NotStarted;

        public FeedbackToLed(IDigitalLed led) {
            if (led == null) {
                throw new ArgumentNullException("led");
            }

            _led = led;
            BlinkOn(3, 100);

            _uiUpdater = new Thread(UpdateLed);
            _uiUpdater.Start();
        }

        public void TestCaseCompleted(TestCaseResult result) {
            if (_testingStatusMode == TestingStatusMode.Running && !result.Passed) {
                _testingStatusMode = TestingStatusMode.RunningWithFailures;
            }
        }

        public void TestCaseStarting(string testName) { }

        public void TestsCompleted(TestRunResult runResults) {
            _testingStatusMode = (runResults.FailedCount == 0) ? TestingStatusMode.Complete : TestingStatusMode.CompleteWithFailures;
        }

        public void TestsStarting(TestRunResult runResults) {
            _runResults = runResults;
            BlinkOn(5, 50);
            _testingStatusMode = TestingStatusMode.Running;
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
                        BlinkOn(_runResults.FailedCount, 200);
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