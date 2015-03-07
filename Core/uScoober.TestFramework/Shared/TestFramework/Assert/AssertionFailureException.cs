using System;

namespace uScoober.TestFramework.Assert
{
    internal class AssertionFailureException : Exception
    {
        public AssertionFailureException(string message)
            : base(message) { }

        public override string ToString() {
            return "Assert Failure: " + Message;
        }
    }
}