using System;
using System.Threading;

namespace uScoober.Threading
{
    public abstract class TaskTestBase
    {
        private AggregateException _unobserved;

        protected TaskTestBase() {
            TaskScheduler.UnobservedExceptionHandler += CaptureUnobservedException;
        }

        protected void EnsureQuietDisposal(Task task) {
            int counter = 0;
            while (!task.IsComplete && counter < 10) {
                Thread.Sleep(25);
                counter++;
            }
            task.Dispose();
            if (_unobserved != null) {
                throw _unobserved;
            }
        }

        protected AggregateException EnsureUnobservedException(Task task) {
            int counter = 0;
            while (!task.IsComplete && counter < 10) {
                Thread.Sleep(25);
                counter++;
            }
            task.Dispose();
            if (_unobserved == null) {
                throw new Exception("No unobserved exception was thrown!");
            }
            return _unobserved;
        }

        private void CaptureUnobservedException(AggregateException exception) {
            _unobserved = exception;
        }

        ~TaskTestBase() {
            TaskScheduler.UnobservedExceptionHandler -= CaptureUnobservedException;
        }
    }
}