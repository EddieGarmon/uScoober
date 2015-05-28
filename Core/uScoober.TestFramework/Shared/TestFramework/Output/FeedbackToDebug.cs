using Microsoft.SPOT;
using uScoober.TestFramework.Core;

namespace uScoober.TestFramework.Output
{
    public class FeedbackToDebug : IRunnerResultProcessor
    {
        public void TestCaseCompleted(TestCaseResult result) {
            Debug.Print((result.Passed ? "[pass]" : "[  **FAIL**  ]"));
            Debug.Print("");
        }

        public void TestCaseStarting(string testName) {
            Debug.Print(testName);
            Debug.Print("[queued]");
        }

        public void TestsCompleted(TestRunResult runResults) {
            Debug.Print("Testing Completed, " + runResults.PassedCount + " passed, " + runResults.FailedCount + " failed.");
            Debug.Print(runResults.DurationSummary);
        }

        public void TestsStarting(TestRunResult runResults) {
            Debug.Print("Testing Starting");
        }
    }
}