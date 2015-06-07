using System.Diagnostics;
using uScoober.DataStructures;

namespace uScoober.Threading
{
    [DebuggerDisplay("[{CallbackCount}] Canceled: {IsCancelationRequested}")]
    public class CancellationSource : DisposableBase
    {
        private static readonly CancellationSource __canceled = new CancellationSource {
            IsCancelationRequested = true
        };
        private readonly CancellationToken _token;
        private Queue _callbacks;

        public CancellationSource() {
            _token = new CancellationToken(this);
        }

        public bool IsCancelationRequested { get; private set; }

        public CancellationToken Token {
            get { return _token; }
        }

        private int CallbackCount {
            get { return _callbacks == null ? 0 : _callbacks.Count; }
        }

        public static CancellationSource Canceled {
            get { return __canceled; }
        }

        public void Cancel() {
            ThrowIfDisposed();
            IsCancelationRequested = true;
            if (_callbacks == null) {
                return;
            }
            lock (this) {
                while (!_callbacks.IsEmpty) {
                    var action = (Action)_callbacks.Dequeue();
                    action();
                }
                _callbacks = null;
            }
        }

        internal void Register(Action callback) {
            ThrowIfDisposed();
            if (!IsCancelationRequested) {
                lock (this) {
                    if (_callbacks == null) {
                        _callbacks = new Queue();
                    }
                    _callbacks.Enqueue(callback);
                }
            }
            else {
                //cancelation has already been requested, immediatly call back
                callback();
            }
        }

        protected override void DisposeManagedResources() {
            if (_callbacks == null) {
                return;
            }
            _callbacks.Clear();
            _callbacks = null;
        }

        /// <summary>
        /// Merges two CancellationSources. Treat null as NON-cancellable. 
        /// </summary>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        /// <returns></returns>
        public static CancellationSource Merge(CancellationSource source1, CancellationSource source2) {
            if (source1 == null) {
                return source2;
            }
            return source2 == null ? source1 : new Merged(source1, source2);
        }

        public static CancellationSource operator +(CancellationSource left, CancellationSource right) {
            return Merge(left, right);
        }

        private class Merged : CancellationSource
        {
            public Merged(CancellationSource source1, CancellationSource source2) {
                RegisterWithSource(source1);
                RegisterWithSource(source2);
            }

            protected override void DisposeManagedResources() {
                //todo handle dispose
                base.DisposeManagedResources();
            }

            private void RegisterWithSource(CancellationSource source) {
                if (source == null) {
                    return; //todo: throw?
                }
                source.Register(Cancel);
            }
        }
    }
}