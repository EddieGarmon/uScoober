using System;

namespace uScoober.TestFramework.Core
{
    public class TestCaseResult
    {
        public TestCaseResult(string name) {
            Name = name;
        }

        private TestCaseResult(string name, double setupTime, double testTime, double teardownTime) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            Name = name;
            DurationOfSetup = setupTime;
            DurationOfTest = testTime;
            DurationOfTeardown = teardownTime;
            Passed = true;
        }

        private TestCaseResult(string name, string exception, double setupTime, double testTime, double teardownTime) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            if (exception == null) {
                throw new ArgumentNullException("exception");
            }
            Name = name;
            ExceptionMessage = exception;
            DurationOfSetup = setupTime;
            DurationOfTest = testTime;
            DurationOfTeardown = teardownTime;
            Passed = false;
        }

        public double DurationOfSetup { get; set; }

        public double DurationOfTeardown { get; set; }

        public double DurationOfTest { get; set; }

        public string ExceptionMessage { get; set; }

        public string Name { get; private set; }

        public bool Passed { get; set; }

        public static TestCaseResult Fail(string name, string exception, double setupTime, double testTime, double teardownTime) {
            return new TestCaseResult(name, exception, setupTime, testTime, teardownTime);
        }

        public static TestCaseResult Pass(string name, double setupTime, double testTime, double teardownTime) {
            return new TestCaseResult(name, setupTime, testTime, teardownTime);
        }
    }
}