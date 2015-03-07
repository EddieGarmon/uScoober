using System.Reflection;
using System.Threading;
using Microsoft.SPOT;
using uScoober.TestFramework.Sdk.Runners.Feedback;

namespace uScoober.TestFramework.Sdk.Runners
{
    internal class TestRunner
    {
        private readonly Assembly _assemblyUnderTest;
        private readonly ITestRunFeedback _feedback;
        private readonly bool _isRestartable;
        private readonly Thread _testThread;
        private TestRun _currentRun;

        internal TestRunner(Assembly assemblyUnderTest, ITestRunFeedback feedback, bool isRestartable) {
            _assemblyUnderTest = assemblyUnderTest;
            _feedback = feedback;
            _isRestartable = isRestartable;

            // setup runner
            State = TestRunnerState.Waiting;
            _testThread = new Thread(TestExecutorThreadLoop);
            _testThread.Start();
        }

        public ITestRunFeedback Feedback {
            get { return _feedback; }
        }

        internal TestRunnerState State { get; private set; }

        public void ExecuteTests() {
            if (State == TestRunnerState.Waiting) {
                State = TestRunnerState.StartRequested;
            }
        }

        private void TestExecutorThreadLoop() {
            bool moreTestingPossible = true;
            while (moreTestingPossible) {
                switch (State) {
                    case TestRunnerState.StartRequested:
                        State = TestRunnerState.Running;
                        if (_currentRun != null) {
                            _currentRun.Dispose();
                        }
                        Debug.GC(true);
                        _currentRun = new TestRun();
                        Feedback.UseTestRun(_currentRun);
                        TestCaseFinder.FindAndRunTestCases(_assemblyUnderTest, _currentRun);
                        _currentRun.TestingComplete();
                        // todo: save run history to storage file?
                        Debug.GC(false);
                        if (!_isRestartable) {
                            State = TestRunnerState.Stopped;
                            moreTestingPossible = false;
                        }
                        else {
                            State = TestRunnerState.Waiting;
                        }
                        break;

                    default:
                        Thread.Sleep(100);
                        break;
                }
            }
        }
    }
}