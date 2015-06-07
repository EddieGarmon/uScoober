using uScoober.Hardware.Display;
using uScoober.TestFramework.Core;

namespace uScoober.TestFramework.UI
{
    internal class FeedbackToTextDisplay : IRunnerResultProcessor
    {
        private readonly IDisplayText _lcd;
        private int _lastTestLineCount;
        private TestRunResult _runResults;

        public FeedbackToTextDisplay(IDisplayText lcd) {
            _lcd = lcd;
        }

        public void TestCaseCompleted(TestCaseResult result) { }

        public void TestCaseStarting(string testName) {
            string[] nameParts = testName.Split('\n');
            int lineCount = 0;
            for (int i = 0; i < _lcd.Rows && i < nameParts.Length; i++) {
                _lcd.WriteRow(i, nameParts[i]);
                lineCount++;
            }
            if (lineCount < _lastTestLineCount) {
                for (int i = lineCount; i < _lastTestLineCount; i++) {
                    _lcd.ClearRow(i);
                }
            }
            _lastTestLineCount = lineCount;
        }

        public void TestsCompleted(TestRunResult runResults) {
            _lcd.WriteRow(0, "uTesting Complete");
            _lcd.WriteRow(1, runResults.PassedCount + " pass, " + runResults.FailedCount + " fail");
            _lcd.WriteRow(2, "test " + runResults.DurationSummary);
            _lcd.WriteRow(3, "elap " + runResults.ElapsedSummary);
        }

        public void TestsStarting(TestRunResult runResults) {
            if (_runResults != null) {
                _runResults.Dispose();
            }
            _runResults = runResults;

            _lcd.ClearScreen();
            _lcd.Write("Testing...");
            _lastTestLineCount = 0;
        }
    }
}