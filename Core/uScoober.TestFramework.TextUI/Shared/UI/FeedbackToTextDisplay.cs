using uScoober.Hardware.Display;
using uScoober.TestFramework.Core;

namespace uScoober.TestFramework.UI
{
    internal class FeedbackToTextDisplay : IRunnerResultProcessor
    {
        private readonly IDisplayText _lcd;
        private TestRunResult _runResults;
        //  private TestingStatusMode _testingStatusMode = TestingStatusMode.NotStarted;

        public FeedbackToTextDisplay(IDisplayText lcd) {
            _lcd = lcd;
        }

        public void TestCaseCompleted(TestCaseResult result) { }

        public void TestCaseStarting(string testName) {
            _lcd.Home();
            _lcd.Write(testName);
        }

        public void TestsCompleted(TestRunResult runResults) {
            _lcd.ClearAll();
            _lcd.Home();
            _lcd.Write("Testing Complete");
        }

        public void TestsStarting(TestRunResult runResults) {
            if (_runResults != null) {
                _runResults.Dispose();
            }
            _runResults = runResults;
            _lcd.ClearAll();
            _lcd.Home();
            _lcd.Write("Testing...");
        }
    }
}