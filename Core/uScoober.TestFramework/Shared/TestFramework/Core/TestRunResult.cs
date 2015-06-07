using System;
using System.Diagnostics;
using System.Text;
using uScoober.DataStructures;

namespace uScoober.TestFramework.Core
{
    public class TestRunResult : DisposableBase
    {
        private readonly List _failures = new List();

        public TestRunResult() {
            RunStarted = DateTime.Now;
        }

        public double DurationOfExecution { get; private set; }

        public double DurationOfSetup { get; private set; }

        public double DurationOfTeardown { get; private set; }

        public string DurationSummary {
            get {
                //mimic format of elapsed summary
                var builder = new StringBuilder(15);
                double executionTime = DurationOfSetup + DurationOfExecution + DurationOfTeardown;

                if (executionTime < 3600) {
                    builder.Append("00:");
                }
                else {
                    int hours = (int)(executionTime / 3600);
                    if (hours < 10) {
                        builder.Append("0");
                    }
                    builder.Append(hours);
                    builder.Append(":");
                    executionTime -= (hours * 3600);
                }

                if (executionTime < 60) {
                    builder.Append("00:");
                }
                else {
                    int minutes = (int)(executionTime / 60);
                    if (minutes < 10) {
                        builder.Append("0");
                    }
                    builder.Append(minutes);
                    builder.Append(":");
                    executionTime -= (minutes * 60);
                }

                if (executionTime < 10) {
                    builder.Append("0");
                }
                builder.Append(executionTime.ToString("F6"));
                return builder.ToString();
            }
        }

        public string ElapsedSummary {
            get { return (RunFinished - RunStarted).ToString(); }
        }

        public int FailedCount { get; private set; }

        public List Failures {
            get { return _failures; }
        }

        public bool IsComplete { get; private set; }

        public int PassedCount { get; private set; }

        public DateTime RunFinished { get; private set; }

        public DateTime RunStarted { get; private set; }

        [DebuggerStepThrough]
        public void Record(TestCaseResult result) {
            if (result.Passed) {
                PassedCount++;
            }
            else {
                FailedCount++;
                _failures.Add(result);
            }
            DurationOfSetup += result.DurationOfSetup;
            DurationOfExecution += result.DurationOfTest;
            DurationOfTeardown += result.DurationOfTeardown;
        }

        public void TestingComplete() {
            RunFinished = DateTime.Now;
            IsComplete = true;
        }

        protected override void DisposeManagedResources() {
            _failures.Clear();
        }
    }
}