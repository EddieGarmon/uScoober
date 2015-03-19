using System.IO;

namespace uScoober.TestFramework.Sdk.Runners.Feedback
{
    internal class BuildRunnerFeedback : GuiFeedback
    {
        private int _updateCount;

        public override void UseTestRun(TestRun testRun) {
            base.UseTestRun(testRun);

            Action baseUpdate = testRun.Updated;
            testRun.Updated = () => {
                                  baseUpdate();
                                  using (FileStream file = File.Open(LogFilename, FileMode.Append)) {
                                      using (var writer = new StreamWriter(file)) {
                                          writer.WriteLine("Update " + _updateCount++);
                                          writer.WriteLine("Pass: " + testRun.PassedCount + ", Fail: " + testRun.FailedCount);
                                          writer.WriteLine(testRun.IsComplete ? "Testing done." : ("Testing: " + testRun.CurrentTestName));
                                          writer.WriteLine();
                                          writer.Flush();
                                      }
                                  }

                                  if (testRun.IsComplete) {
                                      using (FileStream file = File.Open(SummaryFilename, FileMode.Create)) {
                                          using (var writer = new StreamWriter(file)) {
                                              writer.WriteLine("Pass: " + testRun.PassedCount + ", Fail: " + testRun.FailedCount + " ["
                                                               + testRun.DurationSummary + "]");
                                          }
                                      }
                                      // write out failures file for build automation
                                      if (testRun.FailedCount > 0) {
                                          using (FileStream file = File.Open(FailuresFilename, FileMode.Create)) {
                                              using (var writer = new StreamWriter(file)) {
                                                  foreach (TestCase failure in testRun.Failures) {
                                                      writer.WriteLine(failure.Name);
                                                      writer.WriteLine(failure.ExceptionMessage);
                                                      writer.WriteLine();
                                                      writer.WriteLine("-----------------------");
                                                      writer.WriteLine();
                                                  }
                                              }
                                          }
                                      }
                                  }
                              };
        }

        public const string BuildMarkerFilename = @"\WINFS\BuildTesting.txt";
        private const string FailuresFilename = @"\WINFS\TestFailures.txt";
        private const string LogFilename = @"\WINFS\TestLog.txt";
        private const string SummaryFilename = @"\WINFS\TestSummary.txt";
    }
}