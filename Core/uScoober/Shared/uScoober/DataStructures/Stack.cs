using System;
using System.Collections;

namespace uScoober.DataStructures
{
    public class Stack : IStack
    {
        private readonly Ring _storage;

        public Stack() {
            _storage = new Ring();
        }

        public Stack(IEnumerable values) {
            _storage = new Ring(values);
        }

        public int Count {
            get { return _storage.Count; }
        }

        public void Clear() {
            _storage.Clear();
        }

        public bool Contains(object item) {
            return _storage.Find(item) != null;
        }

        public void CopyTo(Array array, int arrayIndex) {
            _storage.CopyToArray(array, arrayIndex);
        }

        public IEnumerator GetEnumerator() {
            return _storage.GetForwardEnumerator();
        }

        public object Peek(int index = 0) {
            if (index < 0 || Count <= index) {
                throw new InvalidOperationException("Noting to peek at.");
            }
            return _storage.FindAtIndex(index)
                           .Value;
        }

        public object Pop() {
            var result = _storage.Head.Value;
            _storage.RemoveHead();
            return result;
        }

        public void Push(object item) {
            _storage.InsertHead(item);
        }

        public static implicit operator object[](Stack stack) {
            return stack._storage.ToArray();
        }

        public static implicit operator Stack(object[] array) {
            return new Stack(array);
        }
    }
}