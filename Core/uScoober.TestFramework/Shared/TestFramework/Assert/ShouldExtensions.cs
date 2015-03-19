using System;
using System.Collections;
using uScoober.Extensions;

namespace uScoober.TestFramework.Assert
{
    public static class ShouldExtensions
    {
        private const string ActualOutOfElements = "Actual Out Of Elements";
        private const string ExpectedOutOfElements = "Expected Out Of Elements";

        public static void ShouldBeEmpty(this IEnumerable values) {
            if (values == null) {
                throw new ArgumentNullException("values");
            }
            if (values.GetEnumerator()
                      .MoveNext()) {
                Throw.NotEmptyException();
            }
        }

        public static void ShouldBeFalse(this bool value) {
            if (value) {
                Throw.NotFalseException();
            }
        }

        public static void ShouldBeNull(this object value) {
            if (value != null) {
                Throw.NotNullException();
            }
        }

        public static void ShouldBeOfType(this object actual, Type expectedClassType) {
            if (!expectedClassType.IsClass) {
                throw new Exception("ShouldBeOfType() requires a class, not an interface. Use ShouldHaveInterface() instead.");
            }
            Type actualType = actual.GetType();
            if (actualType != expectedClassType) {
                Throw.NotEqualException(actualType, expectedClassType);
            }
        }

        public static void ShouldBeTrue(this bool value) {
            if (!value) {
                Throw.NotTrueException();
            }
        }

        public static void ShouldEnumerateEqual(this byte[] actual, params byte[] expected) {
            int i = 0;
            for (; i < actual.Length && i < expected.Length; i++) {
                if (actual[i] != expected[i]) {
                    Throw.NotEqualAtException(actual[i], expected[i], i);
                }
            }
            if (i != actual.Length) {
                Throw.NotEqualAtException(actual[i], ExpectedOutOfElements, i);
            }
            if (i != expected.Length) {
                Throw.NotEqualAtException(ActualOutOfElements, expected[i], i);
            }
        }

        public static void ShouldEnumerateEqual(this IEnumerable actual, params object[] expected) {
            ShouldEnumerateEqual(actual, (IEnumerable)expected);
        }

        public static void ShouldEnumerateEqual(this IEnumerable actual, IEnumerable expected) {
            int counter = 0;

            IEnumerator actualEnumerator = actual.GetEnumerator();
            IEnumerator expectedEnumerator = expected.GetEnumerator();

            bool hasActual = actualEnumerator.MoveNext();
            bool hasExpected = expectedEnumerator.MoveNext();

            while (hasActual && hasExpected) {
                if (!actualEnumerator.Current.Equals(expectedEnumerator.Current)) {
                    Throw.NotEqualAtException(actualEnumerator.Current, expectedEnumerator.Current, counter);
                }
                counter++;
                hasActual = actualEnumerator.MoveNext();
                hasExpected = expectedEnumerator.MoveNext();
            }
            if (hasActual) {
                Throw.NotEqualAtException(actualEnumerator.Current, ExpectedOutOfElements, counter);
            }
            if (hasExpected) {
                Throw.NotEqualAtException(ActualOutOfElements, expectedEnumerator.Current, counter);
            }
        }

        public static void ShouldEqual(this object actual, object expected) {
            if (!actual.Equals(expected)) {
                Throw.NotEqualException(actual, expected);
            }
        }

        public static void ShouldEqual(this byte actual, byte expected) {
            if (actual != expected) {
                Throw.NotEqualException(actual, expected);
            }
        }

        public static void ShouldEqual(this ushort actual, ushort expected) {
            if (actual != expected) {
                Throw.NotEqualException(actual, expected);
            }
        }

        public static void ShouldEqual(this char actual, char expected) {
            if (actual != expected) {
                Throw.NotEqualException(actual, expected);
            }
        }

        public static void ShouldEqual(this int actual, int expected) {
            if (actual != expected) {
                Throw.NotEqualException(actual, expected);
            }
        }

        public static void ShouldEqual(this string actual, string expected) {
            if (actual != expected) {
                Throw.NotEqualException(actual, expected);
            }
        }

        public static void ShouldHaveInterface(this object actual, Type @interface) {
            Type actualType = actual.GetType();
            Type[] allInterfaces = actualType.GetInterfaces();
            if (!allInterfaces.Contains(@interface)) {
                Throw.InterfaceMissingException(actualType, @interface);
            }
        }

        public static void ShouldNotBeEmpty(this IEnumerable values) {
            if (values == null) {
                throw new ArgumentNullException("values");
            }
            if (!values.GetEnumerator()
                       .MoveNext()) {
                Throw.EmptyException();
            }
        }

        public static void ShouldNotBeNull(this object value) {
            if (value == null) {
                Throw.NullException();
            }
        }

        public static void ShouldReference(this object actual, object expected) {
            if (!ReferenceEquals(actual, expected)) {
                Throw.NotTheSameInstanceException();
            }
        }
    }
}