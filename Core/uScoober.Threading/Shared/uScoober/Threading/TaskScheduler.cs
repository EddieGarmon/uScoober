using System;
using System.Threading;

namespace uScoober.Threading
{
    public abstract class TaskScheduler
    {
        public abstract void Schedule(Task task);

        protected void Execute(Task task) {
            if (task.IsCancellationRequested) {
                task.CancelBeforeExecution();
                return;
            }
            if (task.Status != TaskStatus.Scheduled) {
                throw new Exception("Unscheduled task executing!!!");
            }
            try {
                task.ExecuteNow();
            }
            catch (Exception ex) {
                task.RecordException(ex);
            }
            finally {
                task.Finish();
            }
        }

        private static readonly TaskScheduler __defaultScheduler = new TaskSchedulerForBackground();
        private static readonly object __unobservedLock = new object();
        public static int UnusedThreadTimeoutMilliseconds = Timeout.Infinite;

        public static TaskScheduler DefaultScheduler {
            get { return __defaultScheduler; }
        }

        public static event UnobservedTaskExceptionHandler UnobservedExceptionHandler {
            add {
                lock (__unobservedLock) {
                    _unobservedExceptionHandler += value;
                }
            }
            remove {
                lock (__unobservedLock) {
                    _unobservedExceptionHandler -= value;
                }
            }
        }

        private static event UnobservedTaskExceptionHandler _unobservedExceptionHandler;

        internal static void PublishUnobservedException(AggregateException unobservedException) {
            lock (__unobservedLock) {
                if (_unobservedExceptionHandler != null) {
                    _unobservedExceptionHandler(unobservedException);
                }
            }
        }
    }
}