using System.Threading;
using uScoober.TestFramework.Assert;

namespace uScoober.Threading
{
    public class TaskWaitTests : TaskTestBase
    {
        public void WaitTimeoutOccurs_Fact() {
            var cancellationSource = new CancellationSource();
            var task = Task.Run(token => {
                                    while (!token.IsCancellationRequested) {
                                        Thread.Sleep(10);
                                    }
                                },
                                cancellationSource);
            bool waitSuccess = task.Wait(15);
            waitSuccess.ShouldBeFalse();
            cancellationSource.Cancel();
            EnsureQuietDisposal(task);
        }

        public void WaitingOnContinuationStartsAntecedent_Fact() {
            var task = Task.New(() => { });
            var continuation = task.ContinueWith(previous => { });
            task.HasStarted.ShouldBeFalse();
            continuation.HasStarted.ShouldBeFalse();
            continuation.Wait();
            task.Status.ShouldEqual(TaskStatus.RanToCompletion);
            continuation.Status.ShouldEqual(TaskStatus.RanToCompletion);
            EnsureQuietDisposal(task);
            EnsureQuietDisposal(continuation);
        }

        public void WaitingStartsActionTask_Fact() {
            var task = Task.New(() => { });
            task.ShouldNotBeNull();
            task.HasStarted.ShouldBeFalse();
            task.Wait();
            task.Status.ShouldEqual(TaskStatus.RanToCompletion);
            EnsureQuietDisposal(task);
        }

        public void WaitingStartsFuncTask_Fact() {
            var task = Task.New(() => 42);
            task.ShouldNotBeNull();
            task.HasStarted.ShouldBeFalse();
            task.Wait();
            task.Status.ShouldEqual(TaskStatus.RanToCompletion);
            ((int)task.Result).ShouldEqual(42);
            EnsureQuietDisposal(task);
        }
    }
}