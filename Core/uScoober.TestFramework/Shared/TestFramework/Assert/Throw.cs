using System;
using System.Diagnostics;

namespace uScoober.TestFramework.Assert
{
    [DebuggerStepThrough]
    internal static class Throw
    {
        internal static void EmptyException() {
            throw new AssertionFailureException("Enumerable is Empty");
        }

        internal static void InterfaceMissingException(Type actualType, Type expectedInterface) {
            throw new AssertionFailureException(actualType + " does not implement interface: " + expectedInterface);
        }

        internal static void NotEmptyException() {
            throw new AssertionFailureException("Enumerable is Not Empty");
        }

        internal static void NotEqualAtException(object actual, object expected, int index) {
            throw new AssertionFailureException("Enumerable is Not Equal at index " + index + "\nActual:      " + (actual ?? "{null}") + "\nExpected: "
                                                + (expected ?? "{null}"));
        }

        internal static void NotEqualException(object actual, object expected) {
            throw new AssertionFailureException("Not Equal\nActual:      " + (actual ?? "{null}") + "\nExpected: " + (expected ?? "{null}"));
        }

        internal static void NotFalseException() {
            throw new AssertionFailureException("Value is not false");
        }

        internal static void NotNullException() {
            throw new AssertionFailureException("Value is not null");
        }

        internal static void NotTheSameInstanceException() {
            throw new AssertionFailureException("Not the same instance.");
        }

        internal static void NotTrueException() {
            throw new AssertionFailureException("Value is not true");
        }

        internal static void NullException() {
            throw new AssertionFailureException("Value is null");
        }
    }
}