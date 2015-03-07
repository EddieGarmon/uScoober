using System;

namespace uScoober.Threading
{
    public sealed class FuncTask : Task
    {
        private object _result;
        private CancellableFunc _work;

        /// <summary>
        /// Creates a finished Task
        /// </summary>
        /// <param name="completedResult"></param>
        internal FuncTask(object completedResult)
            : base(false) {
            _result = completedResult;
        }

        /// <summary>
        /// Creates an unstarted Task
        /// </summary>
        /// <param name="doThis"></param>
        /// <param name="scheduler"></param>
        internal FuncTask(Func doThis, TaskScheduler scheduler = null)
            : base(CancellationToken.None, scheduler) {
            if (doThis == null) {
                throw new ArgumentNullException("doThis");
            }
            _work = token => doThis();
        }

        /// <summary>
        /// Creates an unstarted Task
        /// </summary>
        /// <param name="doThis"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="scheduler"></param>
        internal FuncTask(CancellableFunc doThis, CancellationToken cancellationToken = null, TaskScheduler scheduler = null)
            : base(cancellationToken, scheduler) {
            if (doThis == null) {
                throw new ArgumentNullException("doThis");
            }
            _work = doThis;
        }

        public object Result {
            get {
                if (!IsComplete) {
                    Wait();
                }
                return _result;
            }
        }

        public ActionTask ContinueWith(BasicContinuation.Action next, TaskScheduler scheduler = null) {
            if (next == null) {
                throw new ArgumentNullException("next");
            }
            return ActionTask.BuildContinuation(this, token => next(this), CancellationToken.None, scheduler);
        }

        public ActionTask ContinueWith(CancellableContinuation.Action next, CancellationToken cancellationToken = null, TaskScheduler scheduler = null) {
            if (next == null) {
                throw new ArgumentNullException("next");
            }
            return ActionTask.BuildContinuation(this, token => next(this, token), cancellationToken, scheduler);
        }

        public FuncTask ContinueWith(BasicContinuation.Func next, TaskScheduler scheduler = null) {
            if (next == null) {
                throw new ArgumentNullException("next");
            }
            return BuildContinuation(this, token => next(this), CancellationToken.None, scheduler);
        }

        public FuncTask ContinueWith(CancellableContinuation.Func next, CancellationToken cancellationToken = null, TaskScheduler scheduler = null) {
            if (next == null) {
                throw new ArgumentNullException("next");
            }
            return BuildContinuation(this, token => next(this, token), cancellationToken, scheduler);
        }

        protected override void ExecuteUserWork(CancellationToken cancellationToken) {
            _result = _work(cancellationToken);
            _work = null;
        }

        internal static FuncTask BuildContinuation(Task antecedent, CancellableFunc doThis, CancellationToken cancellationToken, TaskScheduler scheduler = null) {
            var continuation = new FuncTask(doThis, cancellationToken, scheduler);
            continuation.SetStatusToUnstartedContinuation();
            TaskContinuationManager.RegisterContinuation(antecedent, continuation);
            return continuation;
        }

        public static class BasicContinuation
        {
            public delegate void Action(FuncTask previous);

            public delegate object Func(FuncTask previous);
        }

        public static class CancellableContinuation
        {
            public delegate void Action(FuncTask previous, CancellationToken token);

            public delegate object Func(FuncTask previous, CancellationToken token);
        }
    }
}