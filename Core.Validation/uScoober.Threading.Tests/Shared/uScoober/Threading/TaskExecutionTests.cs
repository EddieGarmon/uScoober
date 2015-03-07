using uScoober.TestFramework.Assert;

namespace uScoober.Threading
{
    public class TaskExecutionTests : TaskTestBase
    {
        public void MoreTasksThanThreads_Fact() {
            const int taskCount = 10;
            var tasks = new Task[taskCount];
            for (int i = 0; i < taskCount; i++) {
                tasks[i] = Task.New(() => {
                                        int sum = 0;
                                        for (int j = 0; j < 10000; j++) {
                                            sum += j;
                                        }
                                    });
            }
            for (int i = 0; i < taskCount; i++) {
                tasks[i].Start();
            }
            for (int i = 0; i < taskCount; i++) {
                tasks[i].Wait();
                tasks[i].IsComplete.ShouldBeTrue();
                tasks[i].IsFaulted.ShouldBeFalse();
                tasks[i].IsCanceled.ShouldBeFalse();
                EnsureQuietDisposal(tasks[i]);
            }
        }

        public void SimpleActionTask_Fact() {
            bool flag = false;
            var task = Task.Run(() => flag = true);
            task.Wait();
            task.Status.ShouldEqual(TaskStatus.RanToCompletion);
            task.Exception.ShouldBeNull();
            flag.ShouldBeTrue();
            EnsureQuietDisposal(task);
        }

        public void SimpleFuncTask_Fact() {
            bool flag = false;
            var task = Task.Run(() => {
                                    flag = true;
                                    return "task result";
                                });
            task.Wait();
            task.Status.ShouldEqual(TaskStatus.RanToCompletion);
            task.Exception.ShouldBeNull();
            flag.ShouldBeTrue();
            task.Result.ToString()
                .ShouldEqual("task result");
            EnsureQuietDisposal(task);
        }
    }
}