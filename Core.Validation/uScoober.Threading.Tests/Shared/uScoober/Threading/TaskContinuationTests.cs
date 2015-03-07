using System;
using System.Threading;
using Microsoft.SPOT;
using uScoober.TestFramework;
using uScoober.TestFramework.Assert;

namespace uScoober.Threading
{
    public class TaskContinuationTests : TaskTestBase
    {
        public void ActionTask_ActionContinuationAfterCancellation_Fact() {
            var continuation = Task.Canceled()
                                   .ContinueWith((previous, token) => {
                                                     previous.IsComplete.ShouldBeTrue();
                                                     previous.IsCanceled.ShouldBeTrue();
                                                     previous.IsFaulted.ShouldBeFalse();
                                                     token.IsCancellationRequested.ShouldBeFalse();
                                                 });
            continuation.Wait();
            continuation.IsComplete.ShouldBeTrue();
            continuation.IsCanceled.ShouldBeFalse();
            continuation.IsFaulted.ShouldBeFalse();
            EnsureQuietDisposal(continuation);
        }

        public void ActionTask_ActionContinuationAfterCompletion_Fact() {
            var continuation = Task.Completed()
                                   .ContinueWith((previous, token) => {
                                                     previous.IsComplete.ShouldBeTrue();
                                                     previous.IsCanceled.ShouldBeFalse();
                                                     previous.IsFaulted.ShouldBeFalse();
                                                     token.IsCancellationRequested.ShouldBeFalse();
                                                 });
            continuation.Wait();
            continuation.IsComplete.ShouldBeTrue();
            continuation.IsCanceled.ShouldBeFalse();
            continuation.IsFaulted.ShouldBeFalse();
            EnsureQuietDisposal(continuation);
        }

        public void ActionTask_ActionContinuation_Fact() {
            ActionTask task = null;
            ActionTask continuation = null;
            task = Task.New(() => {
                                task.HasStarted.ShouldBeTrue();
                                continuation.HasStarted.ShouldBeFalse();
                            });
            continuation = task.ContinueWith(previous => {
                                                 previous.Id.ShouldEqual(task.Id);
                                                 previous.IsComplete.ShouldBeTrue();
                                                 previous.Exception.ShouldBeNull();
                                                 continuation.HasStarted.ShouldBeTrue();
                                             });
            task.ShouldNotBeNull();
            continuation.ShouldNotBeNull();
            task.HasStarted.ShouldBeFalse();
            continuation.HasStarted.ShouldBeFalse();
            //NB starting/waiting on first task IS NOT be required!
            continuation.Wait();
            task.Status.ShouldEqual(TaskStatus.RanToCompletion);
            continuation.Status.ShouldEqual(TaskStatus.RanToCompletion);
            EnsureQuietDisposal(task);
            EnsureQuietDisposal(continuation);
        }

        public void ActionTask_CancelTaskAndActionContinuation_Fact() {
            var cs = new CancellationSource();
            var task = Task.Run(token => {
                                    cs.Cancel();
                                    Thread.Sleep(10);
                                    token.ThrowIfCancellationRequested();
                                    throw new Exception("We should have been canceled, and never see this.");
                                },
                                cs);
            var continuation = task.ContinueWith((previous, token) => {
                                                     // we should never see the following exception
                                                     throw new Exception("Pre canceled continuation should not run");
                                                 },
                                                 cs);
            continuation.Wait();
            task.Status.ShouldEqual(TaskStatus.Canceled);
            continuation.Status.ShouldEqual(TaskStatus.Canceled);
            EnsureQuietDisposal(task);
            EnsureQuietDisposal(continuation);
        }

        public void ActionTask_FuncContinuationAfterCancelation_Fact() {
            var continuation = Task.Canceled()
                                   .ContinueWith((previous, token) => {
                                                     previous.IsComplete.ShouldBeTrue();
                                                     previous.IsCanceled.ShouldBeTrue();
                                                     previous.IsFaulted.ShouldBeFalse();
                                                     token.IsCancellationRequested.ShouldBeFalse();
                                                     return "pi are round, not square";
                                                 });
            continuation.Result.ShouldEqual("pi are round, not square");
            continuation.IsComplete.ShouldBeTrue();
            continuation.IsCanceled.ShouldBeFalse();
            continuation.IsFaulted.ShouldBeFalse();
            EnsureQuietDisposal(continuation);
        }

        public void ActionTask_FuncContinuationAfterCompletion_Fact() {
            var continuation = Task.Completed()
                                   .ContinueWith((previous, token) => {
                                                     previous.IsComplete.ShouldBeTrue();
                                                     previous.IsCanceled.ShouldBeFalse();
                                                     previous.IsFaulted.ShouldBeFalse();
                                                     token.IsCancellationRequested.ShouldBeFalse();
                                                     return "from continuation";
                                                 });
            ((string)continuation.Result).ShouldEqual("from continuation");
            continuation.IsComplete.ShouldBeTrue();
            continuation.IsCanceled.ShouldBeFalse();
            continuation.IsFaulted.ShouldBeFalse();
            EnsureQuietDisposal(continuation);
        }

        public void ActionTask_FuncContinuation_Fact() {
            var task = Task.New(() => Debug.Print("Hello from First Task"));
            var continuation = task.ContinueWith(previous => {
                                                     Debug.Print("Return from Continuation Task");
                                                     return 42;
                                                 });
            ((int)continuation.Result).ShouldEqual(42);
            EnsureQuietDisposal(task);
            EnsureQuietDisposal(continuation);
        }

        public void ActionTask_StartingAContinuationThrows_Fact() {
            var continuation = Task.New(() => { })
                                   .ContinueWith(previous => { });
            continuation.HasStarted.ShouldBeFalse();
            var exception = Trap.Exception(continuation.Start);
            exception.Message.ShouldEqual("Cannot explicitly start a continuation");
        }

        public void FuncTask_ActionContinuation_Fact() {
            FuncTask task = null;
            ActionTask continuation = null;
            task = Task.Run(() => {
                                task.HasStarted.ShouldBeTrue();
                                continuation.HasStarted.ShouldBeFalse();
                                return "task";
                            });
            continuation = task.ContinueWith(previous => {
                                                 previous.Id.ShouldEqual(task.Id);
                                                 previous.IsComplete.ShouldBeTrue();
                                                 previous.Result.ShouldEqual("task");
                                                 previous.Exception.ShouldBeNull();
                                                 continuation.HasStarted.ShouldBeTrue();
                                             });
            task.ShouldNotBeNull();
            continuation.ShouldNotBeNull();
            task.HasStarted.ShouldBeTrue();
            continuation.HasStarted.ShouldBeFalse();
            //NB waiting on first task should not be required!
            //task1.Wait();
            //task1.IsComplete.ShouldBeTrue();
            continuation.Wait();
            task.Status.ShouldEqual(TaskStatus.RanToCompletion);
            continuation.Status.ShouldEqual(TaskStatus.RanToCompletion);
            EnsureQuietDisposal(task);
            EnsureQuietDisposal(continuation);
        }

        public void FuncTask_FuncContinuation_Fact() {
            var task = Task.New(() => "3.14");
            var continuation = task.ContinueWith(previous => ((string)previous.Result) + "15926");
            ((string)continuation.Result).ShouldEqual("3.1415926");
            EnsureQuietDisposal(task);
            EnsureQuietDisposal(continuation);
        }
    }
}