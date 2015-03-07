using System;

namespace uScoober.Threading
{
    public sealed class WaitTask : Task
    {
        private readonly WaitList _waitList;
        private readonly WaitOn _waitOn;
        private Task _singleResult;

        internal WaitTask(WaitList tasks, WaitOn waitOn, CancellationToken token)
            : base(token, null) {
            _waitList = tasks;
            switch (waitOn) {
                case WaitOn.Single:
                case WaitOn.All:
                    _waitOn = waitOn;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("waitOn");
            }
        }

        public Task SingleResult {
            get {
                switch (_waitOn) {
                    case WaitOn.Single:
                        if (!IsComplete) {
                            Wait();
                        }
                        return _singleResult;
                    default:
                        throw new InvalidOperationException("Waiting on: " + _waitOn + ", not " + WaitOn.Single);
                }
            }
        }

        protected override void ExecuteUserWork(CancellationToken cancellationToken) {
            //todo: pass cancelation into wait list wait function...
            switch (_waitOn) {
                case WaitOn.Single:
                    _singleResult = _waitList.WaitAny();
                    break;
                case WaitOn.All:
                    //todo: short out here for exceptions/cancelation...
                    _waitList.WaitAll();
                    break;
            }
        }
    }
}