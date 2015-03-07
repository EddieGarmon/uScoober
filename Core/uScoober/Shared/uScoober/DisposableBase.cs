using System;

namespace uScoober
{
    /// <summary>
    ///     A base <see cref="IDisposable" /> implementation.
    ///     Note: If you implement a finalizer, make sure that it calls Dispose(DisposeReason.Finalizer).
    /// </summary>
    public abstract class DisposableBase : IDisposable
    {
        private bool _isDisposed;

        public bool IsDisposed {
            get { return _isDisposed; }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            Dispose(DisposeReason.DisposeMethod);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(DisposeReason reason) {
            if (_isDisposed) {
                return;
            }
            if (reason == DisposeReason.DisposeMethod) {
                DisposeManagedResources();
            }
            DisposeUnmanagedResources();
            _isDisposed = true;
        }

        protected virtual void DisposeManagedResources() { }

        protected virtual void DisposeUnmanagedResources() { }

        protected void ThrowIfDisposed() {
            if (_isDisposed) {
                throw new ObjectDisposedException(GetType()
                                                      .FullName);
            }
        }
    }
}