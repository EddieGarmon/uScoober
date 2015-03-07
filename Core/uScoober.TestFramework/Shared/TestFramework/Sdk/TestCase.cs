using System;
using System.Diagnostics;

namespace uScoober.TestFramework.Sdk
{
    internal abstract partial class TestCase
    {
        public double DurationOfExecution { get; private set; }

        public double DurationOfSetup { get; private set; }

        public double DurationOfTeardown { get; private set; }

        public string ExceptionMessage { get; private set; }

        public abstract string Name { get; }

        public bool Passed { get; private set; }

        [DebuggerStepThrough]
        public void Run() {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try {
                RunSetup();
                DurationOfSetup = stopwatch.Split();
                RunTest();
                Passed = true;
                DurationOfExecution = stopwatch.Split();
            }
            catch (Exception ex) {
                ExceptionMessage = ex.ToString();
                Passed = false;
                DurationOfExecution = stopwatch.Split();
            }
            finally {
                RunTeardown();
                DurationOfTeardown = stopwatch.Split();
            }
        }

        protected abstract void RunSetup();

        protected abstract void RunTeardown();

        protected abstract void RunTest();

        protected static readonly object[] EmptyObjects = new object[0];
    }
}