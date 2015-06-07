using System;
using System.Collections;
using uScoober.DataStructures;

namespace uScoober
{
    public class AggregateException : Exception
    {
        private const string UnhandledAggregationOfNothing = "Unhandled Aggregation of nothing!!!";
        private object _storage;

        public AggregateException(Exception exception) {
            if (exception == null) {
                throw new Exception(UnhandledAggregationOfNothing);
            }
            _storage = exception;
        }

        public AggregateException(params Exception[] exceptions) {
            switch (exceptions.Length) {
                case 0:
                    throw new Exception(UnhandledAggregationOfNothing);
                case 1:
                    _storage = exceptions[0];
                    return;
                default:
                    //todo: validate that no exceptions are null!!
                    _storage = new Ring((IEnumerable)exceptions);
                    break;
            }
        }

        public Exception this[int index] {
            get {
                if (index < 0) {
                    throw new IndexOutOfRangeException();
                }
                if (StorageIsRing) {
                    return (Exception)StorageRing.FindAtIndex(index)
                                                 .Value;
                }
                if (index != 0) {
                    throw new IndexOutOfRangeException();
                }
                return (Exception)_storage;
            }
        }

        public int InnerExceptionCount {
            get { return StorageIsRing ? StorageRing.Count : 1; }
        }

        public IEnumerable InnerExceptions {
            get { return StorageIsRing ? (IEnumerable)StorageRing : new Enumerable.Singular(_storage); }
        }

        private bool StorageIsRing {
            get { return _storage is Ring; }
        }

        private Ring StorageRing {
            get { return (Ring)_storage; }
        }

        public AggregateException Flatten() {
            throw new NotImplementedException("AggregateException.Flatten");
        }

        public override string ToString() {
            string value = "AggregateException {Count = " + (StorageIsRing ? StorageRing.Count : 1) + "}";
            foreach (Exception ex in InnerExceptions) {
                value += "\n- " + ex;
            }
            return value;
        }

        internal void Append(Exception exception) {
            if (StorageIsRing) {
                StorageRing.InsertTail(exception);
            }
            _storage = new Ring(_storage, exception);
        }
    }
}