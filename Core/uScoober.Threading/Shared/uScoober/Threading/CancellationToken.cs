using System;

namespace uScoober.Threading
{
    public class CancellationToken
    {
        private readonly CancellationSource _source;

        internal CancellationToken(CancellationSource source) {
            _source = source;
        }

        public bool CanBeCanceled {
            get { return _source != null; }
        }

        public bool IsCancellationRequested {
            get { return _source != null && _source.IsCancelationRequested; }
        }

        public void Register(Action callback) {
            if (!CanBeCanceled) {
                throw new Exception("Non cancellable token will never fire");
            }
            _source.Register(callback);
        }

        public void ThrowIfCancellationRequested() {
            if (IsCancellationRequested) {
                throw new TaskCanceledException(this);
            }
        }

        private static readonly CancellationToken __canceled = new CancellationToken(CancellationSource.Canceled);
        private static readonly CancellationToken __none = new CancellationToken(null);

        public static CancellationToken Canceled {
            get { return __canceled; }
        }

        public static CancellationToken None {
            get { return __none; }
        }

        public static CancellationSource operator +(CancellationSource source1, CancellationToken token2) {
            return CancellationSource.Merge(source1, token2._source);
        }

        public static CancellationSource operator +(CancellationToken token1, CancellationSource source2) {
            return CancellationSource.Merge(token1._source, source2);
        }

        public static CancellationSource operator +(CancellationToken token1, CancellationToken token2) {
            return CancellationSource.Merge(token1._source, token2._source);
        }

        public static implicit operator CancellationToken(CancellationSource source) {
            return source.Token;
        }
    }
}