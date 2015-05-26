using System.IO;

namespace uScoober.TestFramework.Core
{
    public class FeedbackToLogFiles : IRunnerResultProcessor
    {
        public const string EmulatorRoot = @"\WINFS\";
        private readonly string _failuresFilename;
        private readonly string _logFilename;
        private readonly string _summaryFilename;

        public FeedbackToLogFiles(string logDirectory = null) {
            logDirectory = logDirectory ?? EmulatorRoot;
            _failuresFilename = logDirectory + "TestFailures.txt";
            _logFilename = logDirectory + "TestLog.txt";
            _summaryFilename = logDirectory + "TestSummary.txt";
        }

        public void TestCaseCompleted(TestCaseResult result) {
            using (FileStream file = File.Open(_logFilename, FileMode.Append)) {
                using (var writer = new StreamWriter(file)) {
                    writer.WriteLine((result.Passed ? "[  pass  ]" : "[**FAIL**]") + result.Name + " in " + result.DurationOfTest);
                    writer.Flush();
                }
            }
        }

        public void TestCaseStarting(string testName) {
            using (FileStream file = File.Open(_logFilename, FileMode.Append)) {
                using (var writer = new StreamWriter(file)) {
                    writer.WriteLine(testName + " [queued] ");
                    writer.Flush();
                }
            }
        }

        public void TestsCompleted(TestRunResult runResults) {
            using (FileStream file = File.Open(_logFilename, FileMode.Append)) {
                using (var writer = new StreamWriter(file)) {
                    writer.WriteLine("Testing done.");
                    writer.WriteLine();
                    writer.Flush();
                }
            }
            using (FileStream file = File.Open(_summaryFilename, FileMode.Create)) {
                using (var writer = new StreamWriter(file)) {
                    writer.WriteLine("Pass: " + runResults.PassedCount + ", Fail: " + runResults.FailedCount + " [" + runResults.DurationSummary + "]");
                    writer.Flush();
                }
            }
            // write out failures file for build automation
            if (runResults.FailedCount > 0) {
                using (FileStream file = File.Open(_failuresFilename, FileMode.Create)) {
                    using (var writer = new StreamWriter(file)) {
                        foreach (TestCaseResult failure in runResults.Failures) {
                            writer.WriteLine(failure.Name);
                            writer.WriteLine(failure.ExceptionMessage);
                            writer.WriteLine();
                            writer.WriteLine("-----------------------");
                            writer.WriteLine();
                            writer.Flush();
                        }
                    }
                }
            }
        }

        public void TestsStarting(TestRunResult runResults) {
            //todo any pre cleanup?
        }
    }
}