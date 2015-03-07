using System;
using System.Threading;
using uScoober.TestFramework;
using uScoober.TestFramework.Assert;

namespace uScoober.Threading
{
    internal class TaskCancellationTests : TaskTestBase
    {
        public void CancelActionTask_NoisyWait_Fact() {
            var cancellationSource = new CancellationSource();
            var task = Task.Run(token => {
                                    for (int i = 0; i < 1000000; i++) {
                                        token.ThrowIfCancellationRequested();
                                        Thread.Sleep(10);
                                    }
                                },
                                cancellationSource);
            Thread.Sleep(15);
            cancellationSource.Cancel();
            var waitException = Trap.WaitException(task);
            ValidateCancellationException(waitException);
            ValidateCanceledTask(task);
            EnsureQuietDisposal(task);
        }

        public void CancelActionTask_QuietBackground_Fact() {
            var cancellationSource = new CancellationSource();
            var task = Task.Run(token => {
                                    for (int i = 0; i < 1000000; i++) {
                                        token.ThrowIfCancellationRequested();
                                        Thread.Sleep(10);
                                    }
                                },
                                cancellationSource);
            Thread.Sleep(15);
            cancellationSource.Cancel();
            while (!task.IsComplete) {
                Thread.Sleep(5);
            }
            ValidateCanceledTask(task);
            EnsureQuietDisposal(task);
        }

        public void CancelFuncTask_NoisyWait_Fact() {
            var cancellationSource = new CancellationSource();
            var task = Task.Run(token => {
                                    for (int i = 0; i < 1000000; i++) {
                                        token.ThrowIfCancellationRequested();
                                        Thread.Sleep(10);
                                    }
                                    return "we should not see this";
                                },
                                cancellationSource);
            Thread.Sleep(15);
            cancellationSource.Cancel();
            var waitException = Trap.WaitException(task);
            ValidateCancellationException(waitException);
            ValidateCanceledTask(task);
            EnsureQuietDisposal(task);
        }

        public void CancelFuncTask_QuietBackground_Fact() {
            var cancellationSource = new CancellationSource();
            var task = Task.Run(token => {
                                    for (int i = 0; i < 1000000; i++) {
                                        token.ThrowIfCancellationRequested();
                                        Thread.Sleep(10);
                                    }
                                    return "we should not see this";
                                },
                                cancellationSource);
            Thread.Sleep(15);
            cancellationSource.Cancel();
            while (!task.IsComplete) {
                Thread.Sleep(5);
            }
            ValidateCanceledTask(task);
            EnsureQuietDisposal(task);
        }

        public void PreCanceledActionTaskShouldNotStart_Fact() {
            var task = Task.Run(token => { throw new Exception("Task with already canceled token should not start."); }, CancellationToken.Canceled);
            var waitException = Trap.WaitException(task);
            waitException.ShouldBeNull();
            ValidateCanceledTask(task);
            EnsureQuietDisposal(task);
        }

        public void PreCanceledFuncTaskShouldNotStart_Fact() {
            var task = Task.Run(token => { throw new Exception("Task with already canceled token should not start."); }, CancellationToken.Canceled);
            Thread.Sleep(15);
            var waitException = Trap.WaitException(task);
            waitException.ShouldBeNull();
            ValidateCanceledTask(task);
            EnsureQuietDisposal(task);
        }

        private static void ValidateCanceledTask(Task task) {
            task.Status.ShouldEqual(TaskStatus.Canceled);
        }

        private static void ValidateCancellationException(AggregateException waitException) {
            waitException.ShouldNotBeNull();
            waitException[0].ShouldBeOfType(typeof(TaskCanceledException));
        }
    }
}