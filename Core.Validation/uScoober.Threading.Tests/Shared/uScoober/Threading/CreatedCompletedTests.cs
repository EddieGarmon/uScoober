using System;
using uScoober.TestFramework.Assert;

namespace uScoober.Threading
{
    public class CreatedCompletedTests
    {
        public void Canceled_Fact() {
            var task = Task.Canceled();
            task.ShouldNotBeNull();
            task.Status.ShouldEqual(TaskStatus.Canceled);
            task.IsComplete.ShouldBeTrue();
            task.IsCanceled.ShouldBeTrue();
            task.IsFaulted.ShouldBeFalse();
        }

        public void Completed_Fact() {
            var task = Task.Completed();
            task.ShouldNotBeNull();
            task.Status.ShouldEqual(TaskStatus.RanToCompletion);
            task.IsComplete.ShouldBeTrue();
            task.IsCanceled.ShouldBeFalse();
            task.IsFaulted.ShouldBeFalse();
        }

        public void CompletedWithResult_Fact() {
            var task = Task.Completed(":)");
            task.ShouldNotBeNull();
            task.Result.ShouldEqual(":)");
            task.Status.ShouldEqual(TaskStatus.RanToCompletion);
            task.IsComplete.ShouldBeTrue();
            task.IsCanceled.ShouldBeFalse();
            task.IsFaulted.ShouldBeFalse();
        }

        public void FaultedWithException_Fact() {
            var task = Task.Faulted(new Exception("boo hiss"));
            task.ShouldNotBeNull();
            Exception ex = task.Exception;
            ex.ShouldNotBeNull();
            task.Status.ShouldEqual(TaskStatus.Faulted);
            task.IsComplete.ShouldBeTrue();
            task.IsCanceled.ShouldBeFalse();
            task.IsFaulted.ShouldBeTrue();
        }

        public void FaultedWithMessage_Fact() {
            var task = Task.Faulted("boo hiss");
            task.ShouldNotBeNull();
            Exception ex = task.Exception;
            ex.ShouldNotBeNull();
            task.Status.ShouldEqual(TaskStatus.Faulted);
            task.IsComplete.ShouldBeTrue();
            task.IsCanceled.ShouldBeFalse();
            task.IsFaulted.ShouldBeTrue();
        }
    }
}