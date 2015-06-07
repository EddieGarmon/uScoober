using System;
using System.Threading;

namespace uScoober.Threading
{
    public abstract partial class Task : IDisposable
    {
        private static int __idCounter;
        private readonly CancellationToken _cancellationToken;
        private readonly ManualResetEvent _completionHandle;
        private readonly TaskScheduler _scheduler;
        private ExceptionRecord _exceptionRecord;
        private int _id;
        private TaskStatus _status;

        /// <summary>
        /// Creates an unstarted task.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="scheduler"></param>
        protected Task(CancellationToken token, TaskScheduler scheduler) {
            _cancellationToken = token ?? CancellationToken.None;
            _scheduler = scheduler ?? TaskScheduler.DefaultScheduler;
            _completionHandle = new ManualResetEvent(false);
            _status = TaskStatus.WaitingForAcivation;
        }

        /// <summary>
        /// Creates a finished task as canceled or completed.
        /// </summary>
        /// <param name="markAsCanceled"></param>
        protected Task(bool markAsCanceled) {
            _status = markAsCanceled ? TaskStatus.Canceled : TaskStatus.RanToCompletion;
        }

        /// <summary>
        /// Creates a finished exceptional task
        /// </summary>
        /// <param name="exception"></param>
        protected Task(Exception exception) {
            RecordException(exception);
            _status = TaskStatus.Faulted;
        }

        public AggregateException Exception {
            get { return _exceptionRecord == null ? null : _exceptionRecord.Exceptions; }
        }

        public bool HasStarted {
            get {
                switch (_status) {
                    case TaskStatus.Scheduled:
                    case TaskStatus.UserWorkStarted:
                    case TaskStatus.UserWorkFinished:
                    case TaskStatus.CancelPending:
                    case TaskStatus.FaultPending:
                    case TaskStatus.RanToCompletion:
                    case TaskStatus.Canceled:
                    case TaskStatus.Faulted:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public int Id {
            get {
                //NB: only init when required, use while to handle rollover
                while (_id == 0) {
                    _id = Interlocked.Increment(ref __idCounter);
                }
                return _id;
            }
        }

        public bool IsCanceled {
            get { return _status == TaskStatus.Canceled; }
        }

        public bool IsComplete {
            get {
                switch (_status) {
                    case TaskStatus.RanToCompletion:
                    case TaskStatus.Canceled:
                    case TaskStatus.Faulted:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool IsFaulted {
            get { return _status == TaskStatus.Faulted; }
        }

        public TaskStatus Status {
            get { return _status; }
            private set { _status = value; }
        }

        internal ManualResetEvent CompletionHandle {
            get { return _completionHandle; }
        }

        internal bool IsCancellationRequested {
            get { return _cancellationToken.IsCancellationRequested; }
        }

        public void Dispose() {
            if (!IsComplete) {
                throw new Exception("Disposing a non completed Task");
            }
            if (_exceptionRecord != null) {
                _exceptionRecord.Dispose();
                _exceptionRecord = null;
            }
            _status = TaskStatus.Disposed;
        }

        public void Start() {
            switch (_status) {
                case TaskStatus.WaitingForAcivation:
                    Schedule();
                    return;
                case TaskStatus.WaitingForAnticedent:
                    throw new Exception("Cannot explicitly start a continuation");
                case TaskStatus.Disposed:
                    throw new ObjectDisposedException();
                default:
                    throw new Exception("Task in a non-startable state: " + _status);
            }
        }

        public bool Wait(CancellationToken cancellationToken) {
            return Wait(-1, cancellationToken);
        }

        /// <summary>
        /// Note that a canceled task will raise an OperationCanceledException wrapped in an AggregateException, 
        /// whereas a canceled wait will raise an unwrapped OperationCanceledException.
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public bool Wait(int millisecondsTimeout = -1, CancellationToken cancellationToken = null) {
            if (cancellationToken == null) {
                cancellationToken = CancellationToken.None;
            }
            //todo: ThrowIfDisposed();?
            if (IsComplete) {
                return true;
            }
            switch (_status) {
                case TaskStatus.WaitingForAcivation:
                    Schedule();
                    break;
                case TaskStatus.WaitingForAnticedent:
                    TaskContinuationManager.ScheduleAntecedent(this);
                    break;
            }
            bool waitSuccess = _completionHandle.WaitOne(millisecondsTimeout, false);
            if (_exceptionRecord != null) {
                throw _exceptionRecord.Exceptions;
            }
            return waitSuccess;
        }

        internal void CancelBeforeExecution() {
            _status = TaskStatus.CancelPending;
            Finish();
        }

        internal void ExecuteNow() {
            Status = TaskStatus.UserWorkStarted;
            ExecuteUserWork(_cancellationToken);
            Status = TaskStatus.UserWorkFinished;
        }

        internal void Finish() {
            switch (_status) {
                case TaskStatus.UserWorkFinished:
                    Status = TaskStatus.RanToCompletion;
                    break;
                case TaskStatus.CancelPending:
                    Status = TaskStatus.Canceled;
                    break;
                case TaskStatus.FaultPending:
                    Status = TaskStatus.Faulted;
                    break;
                default:
                    RecordException(new ArgumentOutOfRangeException("Finishing a task with unexpected status: " + _status));
                    Status = TaskStatus.Faulted;
                    break;
            }
            _completionHandle.Set();
            TaskContinuationManager.ContinueFrom(this);
        }

        internal void RecordException(Exception exception) {
            if (_exceptionRecord == null) {
                _exceptionRecord = new ExceptionRecord(exception);
            }
            else {
                _exceptionRecord.Append(exception);
            }
            _status = exception is TaskCanceledException ? TaskStatus.CancelPending : TaskStatus.FaultPending;
        }

        internal void Schedule() {
            try {
                if (_cancellationToken.IsCancellationRequested) {
                    Status = TaskStatus.CancelPending;
                    Finish();
                    return;
                }
                //todo: note: only allow to start/schedule once, period. 
                _scheduler.Schedule(this);
            }
            catch (ThreadAbortException ex) {
                //todo: whats special about abort that we can work with?
                RecordException(ex);
                //todo mark exception as observed?
                Finish();
            }
            catch (Exception ex) {
                RecordException(new TaskSchedulerException(ex));
                Finish();
            }
        }

        internal void SetStatusToScheduled() {
            switch (_status) {
                case TaskStatus.WaitingForAcivation:
                case TaskStatus.WaitingForAnticedent:
                    _status = TaskStatus.Scheduled;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        protected abstract void ExecuteUserWork(CancellationToken cancellationToken);

        protected void SetStatusToUnstartedContinuation() {
            Status = TaskStatus.WaitingForAnticedent;
        }
    }
}