using System;
using System.Collections;
using System.Diagnostics;

namespace uScoober.DataStructures
{
    [DebuggerDisplay("Count={Count}, EditVersion={EditVersion}")]
    public class List : IList
    {
        private readonly Ring _storage;

        public List() {
            _storage = new Ring();
        }

        public List(params object[] values) {
            _storage = new Ring(values);
        }

        public List(IEnumerable values) {
            _storage = new Ring(values);
        }

        public int Count {
            get { return _storage.Count; }
        }

        public int EditVersion {
            get { return _storage.EditVersion; }
        }

        public virtual object this[int index] {
            get {
                return _storage.FindAtIndex(index)
                               .Value;
            }
            set {
                var findResult = _storage.FindAtIndex(index);
                _storage.IncrementEditVersion();
                findResult.RingLink.Value = value;
            }
        }

        public virtual int Add(object value) {
            _storage.InsertTail(value);
            return _storage.Count;
        }

        public void AddRange(params object[] values) {
            AddRange((IEnumerable)values);
        }

        public virtual void AddRange(IEnumerable values) {
            _storage.InsertRangeAtIndex(_storage.Count, values);
        }

        public virtual void Clear() {
            _storage.Clear();
        }

        public bool Contains(object value) {
            return _storage.Find(value) != null;
        }

        public void CopyTo(Array array, int index) {
            _storage.CopyToArray(array, index);
        }

        public object Find(Predicate whereClause) {
            var indexedLink = _storage.Find(whereClause);
            return (indexedLink == null) ? null : indexedLink.Value;
        }

        public object FindLast(Predicate whereClause) {
            var indexedLink = _storage.FindLast(whereClause);
            return (indexedLink == null) ? null : indexedLink.Value;
        }

        public IEnumerator GetEnumerator() {
            return _storage.GetForwardEnumerator();
        }

        public int IndexOf(object value) {
            var findResult = _storage.Find(value);
            return (findResult == null) ? -1 : findResult.Index;
        }

        public virtual void Insert(int index, object value) {
            _storage.InsertAtIndex(index, value);
        }

        public void InsertRange(int index, params object[] values) {
            _storage.InsertRangeAtIndex(index, values);
        }

        public void InsertRange(int index, IEnumerable values) {
            _storage.InsertRangeAtIndex(index, values);
        }

        public virtual void Move(int index, int newIndex) {
            _storage.IncrementEditVersion();

            //bug: do we really need two enumerations to find the approprate links?
            var moveThis = _storage.FindAtIndex(index)
                                   .RingLink;
            var beforeThis = _storage.FindAtIndex(newIndex)
                                     .RingLink;

            //stitch hole
            moveThis.Previous.Next = moveThis.Next;
            moveThis.Next.Previous = moveThis.Previous;

            //hook previous
            moveThis.Previous = beforeThis.Previous;
            moveThis.Previous.Next = moveThis;

            //hook next
            moveThis.Next = beforeThis;
            beforeThis.Previous = moveThis;

            if (newIndex == 0) {
                _storage.Head = moveThis;
            }
        }

        public virtual void Remove(object value) {
            var findResult = _storage.Find(value);
            if (findResult != null) {
                _storage.RemoveLink(findResult);
            }
        }

        public virtual void RemoveAt(int index) {
            _storage.RemoveAtIndex(index);
        }

        public virtual void RemoveWhere(Predicate whereClause) {
            _storage.RemoveWhere(whereClause);
        }

        public IEnumerable Reversed() {
            return new Enumerable(_storage.GetReverseEnumerator());
        }

        public virtual void Swap(int indexA, int indexB) {
            //bug: do we really need two enumerations to find the approprate links?
            var link1 = _storage.FindAtIndex(indexA);
            var link2 = _storage.FindAtIndex(indexB);
            _storage.SwapValues(link1, link2);
        }

        public object[] ToArray() {
            return _storage.ToArray();
        }

        public Ring ToRing() {
            return _storage;
        }

        bool ICollection.IsSynchronized {
            get { return false; }
        }

        object ICollection.SyncRoot {
            get { return this; }
        }

        bool IList.IsFixedSize {
            get { return false; }
        }

        bool IList.IsReadOnly {
            get { return false; }
        }

        public static implicit operator object[](List list) {
            return list.ToArray();
        }

        public static implicit operator List(object[] array) {
            return new List(array);
        }
    }
}