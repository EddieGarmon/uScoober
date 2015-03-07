using uScoober.TestFramework.Assert;

namespace uScoober.Threading
{
    public class TaskFactoryTests
    {
        public void NewActionTask_Fact() {
            ActionTask task = Task.New(() => { });
            task.ShouldNotBeNull();
            task.HasStarted.ShouldBeFalse();
        }

        public void NewCancellableActionTask_Fact() {
            ActionTask task = Task.New(token => { });
            task.ShouldNotBeNull();
            task.HasStarted.ShouldBeFalse();
        }

        public void NewCancellableFuncTask_Fact() {
            FuncTask task = Task.New(token => 42);
            task.ShouldNotBeNull();
            task.HasStarted.ShouldBeFalse();
        }

        public void NewFuncTask_Fact() {
            FuncTask task = Task.New(() => 42);
            task.ShouldNotBeNull();
            task.HasStarted.ShouldBeFalse();
        }

        public void RunActionTask_Fact() {
            ActionTask task = Task.Run(() => { });
            task.ShouldNotBeNull();
            task.HasStarted.ShouldBeTrue();
        }

        public void RunCancellableActionTask_Fact() {
            ActionTask task = Task.Run(token => { });
            task.ShouldNotBeNull();
            task.HasStarted.ShouldBeTrue();
        }

        public void RunCancellableFuncTask_Fact() {
            FuncTask task = Task.Run(token => 42);
            task.ShouldNotBeNull();
            task.HasStarted.ShouldBeTrue();
        }

        public void RunFuncTask_Fact() {
            FuncTask task = Task.Run(() => 42);
            task.ShouldNotBeNull();
            task.HasStarted.ShouldBeTrue();
        }
    }
}