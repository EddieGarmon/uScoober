using Microsoft.SPOT;

namespace uScoober.Threading
{
    public class TaskSchedulerForUI : TaskScheduler
    {
        public override void Schedule(Task task) {
            Dispatcher.CurrentDispatcher.BeginInvoke(_ => {
                                                         task.SetStatusToScheduled();
                                                         Execute(task);
                                                         return null;
                                                     },
                                                     null);
        }

        private static readonly TaskScheduler __defaultUIScheduler = new TaskSchedulerForUI();

        public static TaskScheduler DefaultUIScheduler {
            get { return __defaultUIScheduler; }
        }
    }
}