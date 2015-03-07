using System;
using System.Collections;
using uScoober.DataStructures;

namespace uScoober.Storage
{
    public class FileCollection : IEnumerable
    {
        private readonly Ring _storage;

        public FileCollection() {
            _storage = new Ring();
        }

        public FileCollection(params IFile[] values) {
            _storage = new Ring(values);
        }

        public IFile this[int index] {
            get {
                var result = _storage.FindAtIndex(index);
                if (result == null) {
                    throw new IndexOutOfRangeException();
                }
                return (IFile)result.Value;
            }
            set {
                var findResult = _storage.FindAtIndex(index);
                _storage.IncrementEditVersion();
                findResult.RingLink.Value = value;
            }
        }

        public DateTime TimeStamp { get; private set; }

        public int Add(IFile file) {
            _storage.InsertTail(file);
            return _storage.Count;
        }

        public FileCollectionEnumerator GetEnumerator(bool isReversed = false) {
            return new FileCollectionEnumerator(_storage, isReversed);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    public class FileCollectionEnumerator : Ring.Enumerator
    {
        internal FileCollectionEnumerator(Ring storage, bool isReversed = false)
            : base(storage, isReversed) { }

        public new IList Current {
            get { return (IList)base.Current; }
        }
    }
}