using System;
using uScoober.TestFramework;
using uScoober.TestFramework.Assert;

namespace uScoober.Threading
{
    public class TaskExceptionTests : TaskTestBase
    {
        public void ActionTask_FiresUnobservedHandler_Fact() {
            ActionTask task = Task.Run(token => {
                                           throw new Exception("thrown in task");
                                           return;
                                       },
                                       CancellationToken.None);
            var unobserved = EnsureUnobservedException(task);
            unobserved[0].Message.ShouldEqual("thrown in task");
        }

        public void ActionTask_ObserveViaContinuation_Fact() {
            ActionTask task = Task.Run(() => {
                                           throw new Exception("thrown in task");
                                           return;
                                       });
            ActionTask continuation = task.ContinueWith(previous => {
                                                            previous.Status.ShouldEqual(TaskStatus.Faulted);
                                                            // observe below
                                                            var aggregateException = previous.Exception;
                                                            aggregateException[0].Message.ShouldEqual("thrown in task");
                                                        });
            continuation.Wait();
            EnsureQuietDisposal(task);
            EnsureQuietDisposal(continuation);
        }

        public void ActionTask_ObserveViaWait_Fact() {
            ActionTask task = Task.Run(() => {
                                           throw new Exception("thrown in task");
                                           return;
                                       });
            var exception = Trap.WaitException(task);
            exception.ShouldNotBeNull();
            task.Status.ShouldEqual(TaskStatus.Faulted);
            exception.InnerExceptionCount.ShouldEqual(1);
            exception[0].Message.ShouldEqual("thrown in task");
            task.Exception[0].Message.ShouldEqual("thrown in task");
            EnsureQuietDisposal(task);
        }

        public void FuncTask_FiresUnobservedHandler_Fact() {
            FuncTask task = Task.Run(token => {
                                         throw new Exception("thrown in task");
                                         return null;
                                     },
                                     CancellationToken.None);
            var unobserved = EnsureUnobservedException(task);
            unobserved[0].Message.ShouldEqual("thrown in task");
        }

        public void FuncTask_ObserveViaContinuation_Fact() {
            FuncTask task = Task.Run(() => {
                                         throw new Exception("thrown in task");
                                         return "we should not see this";
                                     });
            ActionTask continuation = task.ContinueWith(previous => {
                                                            previous.Status.ShouldEqual(TaskStatus.Faulted);
                                                            var aggregateException = previous.Exception;
                                                            aggregateException[0].Message.ShouldEqual("thrown in task");
                                                        });
            continuation.Wait();
            EnsureQuietDisposal(task);
            EnsureQuietDisposal(continuation);
        }

        public void FuncTask_ObserveViaWait_Fact() {
            FuncTask task = Task.Run(() => {
                                         throw new Exception("thrown in task");
                                         return "we should not see this";
                                     });
            var exception = Trap.WaitException(task);
            exception.ShouldNotBeNull();
            task.Status.ShouldEqual(TaskStatus.Faulted);
            task.Result.ShouldBeNull();
            exception.InnerExceptionCount.ShouldEqual(1);
            exception[0].Message.ShouldEqual("thrown in task");
            task.Exception[0].Message.ShouldEqual("thrown in task");
            EnsureQuietDisposal(task);
        }
    }
}