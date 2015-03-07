namespace uScoober.Threading
{
    public static class UITask
    {
        public static ActionTask ContinueOnUIWith(this ActionTask task, ActionTask.BasicContinuation.Action next) {
            return task.ContinueWith(next, TaskScheduler.DefaultScheduler);
        }

        public static ActionTask ContinueOnUIWith(this ActionTask task,
                                                  ActionTask.CancellableContinuation.Action next,
                                                  CancellationToken cancellationToken = null) {
            return task.ContinueWith(next, cancellationToken, TaskScheduler.DefaultScheduler);
        }

        public static ActionTask Run(Action doThis) {
            return Task.Run(doThis, TaskScheduler.DefaultScheduler);
        }

        public static ActionTask Run(CancellableAction doThis, CancellationToken cancellationToken = null) {
            return Task.Run(doThis, cancellationToken, TaskScheduler.DefaultScheduler);
        }
    }
}