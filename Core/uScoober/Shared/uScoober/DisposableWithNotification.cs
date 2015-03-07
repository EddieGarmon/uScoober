using System.Runtime.CompilerServices;
using Microsoft.SPOT;

namespace uScoober
{
    public class DisposableWithNotification : DisposableBase
    {
        public delegate void DisposedHandler(object sender);

        private DisposedHandler _handler;

        /// <summary>
        ///     Event occurs when disposed.
        /// </summary>
        public event DisposedHandler Disposed {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add { _handler = (DisposedHandler)WeakDelegate.Combine(_handler, value); }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove { _handler = (DisposedHandler)WeakDelegate.Remove(_handler, value); }
        }

        protected new void Dispose(DisposeReason reason) {
            DisposedHandler disposedHandler = _handler;
            base.Dispose(reason);
            if (disposedHandler != null) {
                disposedHandler(this);
            }
        }
    }
}