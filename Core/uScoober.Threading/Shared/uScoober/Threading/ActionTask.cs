using System;

namespace uScoober.Threading
{
    public sealed class ActionTask : Task
    {
        private CancellableAction _userWork;

        /// <summary>
        /// Creates a completed Task.
        /// </summary>
        /// <param name="markAsCanceled"></param>
        internal ActionTask(bool markAsCanceled)
            : base(markAsCanceled) { }

        /// <summary>
        /// Creates a faulted completed Task.
        /// </summary>
        /// <param name="exception"></param>
        internal ActionTask(Exception exception)
            : base(exception) { }

        /// <summary>
        /// Creates an unstarted Task.
        /// </summary>
        /// <param name="doThis"></param>
        /// <param name="scheduler"></param>
        internal ActionTask(Action doThis, TaskScheduler scheduler = null)
            : base(CancellationToken.None, scheduler) {
            if (doThis == null) {
                throw new ArgumentNullException("doThis");
            }
            _userWork = token => doThis();
        }

        /// <summary>
        /// Creates an unstarted Task.
        /// </summary>
        /// <param name="doThis"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="scheduler"></param>
        internal ActionTask(CancellableAction doThis, CancellationToken cancellationToken = null, TaskScheduler scheduler = null)
            : base(cancellationToken, scheduler) {
            if (doThis == null) {
                throw new ArgumentNullException("doThis");
            }
            _userWork = doThis;
        }

        public ActionTask ContinueWith(BasicContinuation.Action next, TaskScheduler scheduler = null) {
            if (next == null) {
                throw new ArgumentNullException("next");
            }
            return BuildContinuation(this, token => next(this), CancellationToken.None, scheduler);
        }

        public ActionTask ContinueWith(CancellableContinuation.Action next, CancellationToken cancellationToken = null, TaskScheduler scheduler = null) {
            if (next == null) {
                throw new ArgumentNullException("next");
            }
            return BuildContinuation(this, token => next(this, token), cancellationToken, scheduler);
        }

        public FuncTask ContinueWith(BasicContinuation.Func next, TaskScheduler scheduler = null) {
            if (next == null) {
                throw new ArgumentNullException("next");
            }
            return FuncTask.BuildContinuation(this, token => next(this), CancellationToken.None, scheduler);
        }

        public FuncTask ContinueWith(CancellableContinuation.Func next, CancellationToken cancellationToken = null, TaskScheduler scheduler = null) {
            if (next == null) {
                throw new ArgumentNullException("next");
            }
            return FuncTask.BuildContinuation(this, token => next(this, token), cancellationToken, scheduler);
        }

        protected override void ExecuteUserWork(CancellationToken cancellationToken) {
            _userWork(cancellationToken);
            _userWork = null;
        }

        internal static ActionTask BuildContinuation(Task antecedent,
                                                     CancellableAction doThis,
                                                     CancellationToken cancellationToken,
                                                     TaskScheduler scheduler = null) {
            var continuation = new ActionTask(doThis, cancellationToken, scheduler);
            continuation.SetStatusToUnstartedContinuation();
            TaskContinuationManager.RegisterContinuation(antecedent, continuation);
            return continuation;
        }

        public static class BasicContinuation
        {
            public delegate void Action(ActionTask previous);

            public delegate object Func(ActionTask previous);
        }

        public static class CancellableContinuation
        {
            public delegate void Action(ActionTask previous, CancellationToken token);

            public delegate object Func(ActionTask previous, CancellationToken token);
        }
    }
}