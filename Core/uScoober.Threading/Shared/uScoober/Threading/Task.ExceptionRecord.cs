using System;

namespace uScoober.Threading
{
    public abstract partial class Task
    {
        private class ExceptionRecord : IDisposable
        {
            private readonly AggregateException _exceptions;
            private bool _isObserved;

            public ExceptionRecord(Exception initialException) {
                _exceptions = new AggregateException(initialException);
                SetObserved(initialException is TaskCanceledException);
            }

            public AggregateException Exceptions {
                get {
                    SetObserved(true);
                    return _exceptions;
                }
            }

            public void Append(Exception additionalException) {
                Exceptions.Append(additionalException);
                SetObserved(_isObserved && !(additionalException is TaskCanceledException));
            }

            public void Dispose() {
                EnsureExceptionIsObserved();
                GC.SuppressFinalize(this);
            }

            private void EnsureExceptionIsObserved() {
                if (_isObserved) {
                    return;
                }
                TaskScheduler.PublishUnobservedException(_exceptions);
                //todo: validate it was handeled OR WHAT?
                //mark as handled to we dont fire multiple times
                _isObserved = true;
            }

            private void SetObserved(bool value) {
                _isObserved = value;
                if (value) {
                    GC.SuppressFinalize(this);
                }
                else {
                    GC.ReRegisterForFinalize(this);
                }
            }

            ~ExceptionRecord() {
                EnsureExceptionIsObserved();
            }
        }
    }
}