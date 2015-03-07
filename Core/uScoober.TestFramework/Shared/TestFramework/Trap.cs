using System;
using System.Diagnostics;
using uScoober.Threading;

namespace uScoober.TestFramework
{
    [DebuggerStepThrough]
    public static class Trap
    {
        public static Exception Exception(Action action) {
            try {
                action();
            }
            catch (Exception exception) {
                return exception;
            }
            return null;
        }

        public static AggregateException WaitException(Task task) {
            try {
                task.Wait();
            }
            catch (AggregateException exception) {
                return exception;
            }
            return null;
        }
    }
}