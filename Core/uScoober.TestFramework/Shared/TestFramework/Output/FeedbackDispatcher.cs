using System;

namespace uScoober.TestFramework.Core
{
    public class FeedbackDispatcher : IRunnerResultProcessor
    {
        private IRunnerResultProcessor[] _processors;

        public FeedbackDispatcher() {
            _processors = new IRunnerResultProcessor[0];
        }

        public FeedbackDispatcher(params IRunnerResultProcessor[] processors) {
            _processors = processors;
        }

        public void Add(IRunnerResultProcessor output) {
            var temp = new IRunnerResultProcessor[_processors.Length + 1];
            Array.Copy(_processors, temp, _processors.Length);
            temp[_processors.Length] = output;
            _processors = temp;
        }

        public void TestCaseCompleted(TestCaseResult result) {
            for (int i = 0; i < _processors.Length; i++) {
                _processors[i].TestCaseCompleted(result);
            }
        }

        public void TestCaseStarting(string testName) {
            for (int i = 0; i < _processors.Length; i++) {
                _processors[i].TestCaseStarting(testName);
            }
        }

        public void TestsCompleted(TestRunResult runResults) {
            for (int i = 0; i < _processors.Length; i++) {
                _processors[i].TestsCompleted(runResults);
            }
        }

        public void TestsStarting(TestRunResult runResults) {
            for (int i = 0; i < _processors.Length; i++) {
                _processors[i].TestsStarting(runResults);
            }
        }
    }
}