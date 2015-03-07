using System;

namespace uScoober.Threading
{
    public class TaskCanceledException : Exception
    {
        private readonly CancellationToken _token;

        public TaskCanceledException(CancellationToken token)
            : base("Operation Canceled") {
            _token = token;
        }

        public CancellationToken Token {
            get { return _token; }
        }
    }
}