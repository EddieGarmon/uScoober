namespace uScoober.TestFramework.Core
{
    public interface IRunnerResultProcessor
    {
        void TestCaseCompleted(TestCaseResult result);

        void TestCaseStarting(string testName);

        void TestsCompleted(TestRunResult runResults);

        void TestsStarting(TestRunResult runResults);
    }
}