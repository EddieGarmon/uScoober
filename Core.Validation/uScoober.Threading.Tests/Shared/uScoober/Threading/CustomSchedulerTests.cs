using uScoober.TestFramework.Assert;

namespace uScoober.Threading
{
    public class CustomSchedulerTests : TaskTestBase
    {
        public void ExplicitScheduler_Fact() {
            bool pass = false;
            var scheduler = new TestScheduler();
            var task = Task.Run(() => pass = true, scheduler);
            task.Wait();
            pass.ShouldBeTrue();
            EnsureQuietDisposal(task);
        }

        private class TestScheduler : TaskScheduler
        {
            public override void Schedule(Task task) {
                DefaultScheduler.Schedule(task);
            }
        }
    }
}