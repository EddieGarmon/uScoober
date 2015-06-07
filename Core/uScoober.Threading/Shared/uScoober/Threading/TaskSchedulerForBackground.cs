using System.Threading;
using Microsoft.SPOT;
using uScoober.DataStructures;

namespace uScoober.Threading
{
    internal class TaskSchedulerForBackground : TaskScheduler
    {
        internal const int MaxThreads = 2;
        private readonly object _lock = new object();
        private readonly Thread[] _threads = new Thread[MaxThreads];
        private readonly ManualResetEvent _workAvailable = new ManualResetEvent(false);
        private readonly Queue _workQueue = new Queue();
        //todo optimize startup and cool down of threads
        public override void Schedule(Task task) {
            lock (_lock) {
                for (int i = 0; i < MaxThreads; i++) {
                    var thread = _threads[i];
                    if (thread == null || !thread.IsAlive) {
                        thread = new Thread(ConsumeQueuedWork);
                        Debug.Print("Starting TaskThread[" + i + "] with Id:" + thread.ManagedThreadId);
                        _threads[i] = thread;
                        thread.Start();
                    }
                }
                _workQueue.Enqueue(task);
                task.SetStatusToScheduled();
            }
            _workAvailable.Set();
        }

        private void ConsumeQueuedWork() {
            while (true) {
                _workAvailable.WaitOne(UnusedThreadTimeoutMilliseconds, false);
                Task task = null;
                lock (_lock) {
                    if (!_workQueue.IsEmpty) {
                        task = (Task)_workQueue.Dequeue();
                    }
                    else {
                        _workAvailable.Reset();
                    }
                }
                if (task == null) {
                    if (UnusedThreadTimeoutMilliseconds > 0) {
                        break;
                    }
                }
                else {
                    Execute(task);
                }
            }
        }
    }
}