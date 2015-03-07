using System;
using System.Threading;

namespace uScoober
{
    public static class Disposable
    {
        /// <summary>
        ///     Creates a disposable object that invokes the specified action when disposed.
        /// </summary>
        /// <param name="dispose">
        ///     Action to run during the first call to <see cref="M:System.IDisposable.Dispose" />. The action is guaranteed to be
        ///     run at most once.
        /// </param>
        /// <returns>
        ///     The disposable object that runs the given action upon disposal.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="dispose" /> is null.
        /// </exception>
        public static IDisposable Create(Action dispose) {
            if (dispose == null) {
                throw new ArgumentNullException("dispose");
            }
            return new AnonymousDisposable(dispose);
        }

        private sealed class AnonymousDisposable : IDisposable
        {
            private readonly Action _dispose;
            private int _flag = 1;

            /// <summary>
            ///     Constructs a new disposable with the given action used for disposal.
            /// </summary>
            /// <param name="dispose">Disposal action which will be run upon calling Dispose.</param>
            public AnonymousDisposable(Action dispose) {
                _dispose = dispose;
            }

            /// <summary>
            ///     Gets a value that indicates whether the object is disposed.
            /// </summary>
            public bool IsDisposed {
                get { return _dispose == null; }
            }

            /// <summary>
            ///     Calls the disposal action if and only if the current instance hasn't been disposed yet.
            /// </summary>
            public void Dispose() {
                if (Interlocked.CompareExchange(ref _flag, 0, 1) == 1) {
                    _dispose();
                }
            }
        }
    }
}