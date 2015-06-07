using System;
using System.Collections;
using System.Diagnostics;

namespace uScoober.DataStructures.Typed
{
    [DebuggerDisplay("Count={Count}; {First}..{Last}")]
    public class StringList : IEnumerable
    {
        private readonly Ring _storage;

        public StringList() {
            _storage = new Ring();
        }

        public StringList(params string[] values) {
            _storage = new Ring(values);
        }

        private StringList(StringList clonefrom) {
            _storage = new Ring(clonefrom);
        }

        public string this[int index] {
            get {
                return (string)_storage.FindAtIndex(index)
                                       .Value;
            }
            set {
                Ring.IndexedLink findResult = _storage.FindAtIndex(index);
                _storage.IncrementEditVersion();
                findResult.RingLink.Value = value;
            }
        }

        public int Count {
            get { return _storage.Count; }
        }

        public int EditVersion {
            get { return _storage.EditVersion; }
        }

        public string First {
            get { return _storage.Head == null ? null : (string)_storage.Head.Value; }
        }

        public string Last {
            get { return _storage.Tail == null ? null : (string)_storage.Tail.Value; }
        }

        public int Add(string value) {
            _storage.InsertTail(value);
            return _storage.Count;
        }

        public void AddRange(params string[] values) {
            _storage.InsertRangeAtIndex(_storage.Count, values);
        }

        public void Clear() {
            _storage.Clear();
        }

        public StringList Clone() {
            return new StringList(this);
        }

        public StringList CloneSublist(int count) {
            if (count < 0 || count > _storage.Count) {
                throw new ArgumentOutOfRangeException("count");
            }
            if (count == Count) {
                return new StringList(this);
            }
            var clone = new StringList();
            var enumerator = new StringListEnumerator(_storage, false);
            while (count > 0) {
                enumerator.MoveNext();
                clone.Add(enumerator.Current);
                count--;
            }
            return clone;
        }

        public bool Contains(string value) {
            return _storage.Find(value) != null;
        }

        public void CopyTo(Array array, int index) {
            _storage.CopyToArray(array, index);
        }

        public StringListEnumerator GetEnumerator() {
            return new StringListEnumerator(_storage, false);
        }

        public int IndexOf(string value) {
            Ring.IndexedLink findResult = _storage.Find(value);
            return (findResult == null) ? -1 : findResult.Index;
        }

        public void Insert(int index, string value) {
            _storage.InsertAtIndex(index, value);
        }

        public void InsertRange(int index, params string[] values) {
            _storage.InsertRangeAtIndex(index, values);
        }

        public bool Remove(string value) {
            Ring.IndexedLink findResult = _storage.Find(value);
            if (findResult == null) {
                return false;
            }
            _storage.RemoveLink(findResult);
            return true;
        }

        public void RemoveAt(int index) {
            _storage.RemoveAtIndex(index);
        }

        public string RemoveFirst() {
            var result = (string)_storage.Head.Value;
            _storage.RemoveHead();
            return result;
        }

        public string RemoveLast() {
            var result = (string)_storage.Tail.Value;
            _storage.RemoveTail();
            return result;
        }

        public IEnumerable Reversed() {
            return new Enumerable(new StringListEnumerator(_storage, true));
        }

        public string[] ToArray() {
            var result = new string[_storage.Count];
            _storage.CopyToArray(result);
            return result;
        }

        public Ring ToRing() {
            return _storage;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public static implicit operator string[](StringList list) {
            return list.ToArray();
        }

        public static implicit operator StringList(string[] array) {
            return new StringList(array);
        }
    }
}