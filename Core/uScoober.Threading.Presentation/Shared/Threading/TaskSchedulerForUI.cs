using Microsoft.SPOT;

namespace uScoober.Threading
{
    public class TaskSchedulerForUI : TaskScheduler
    {
        private static readonly TaskScheduler __defaultUIScheduler = new TaskSchedulerForUI();

        public static TaskScheduler DefaultUIScheduler {
            get { return __defaultUIScheduler; }
        }

        public override void Schedule(Task task) {
            Dispatcher.CurrentDispatcher.BeginInvoke(_ => {
                                                         task.SetStatusToScheduled();
                                                         Execute(task);
                                                         return null;
                                                     },
                                                     null);
        }
    }
}