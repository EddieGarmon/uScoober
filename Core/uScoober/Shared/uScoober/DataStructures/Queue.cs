using System;
using System.Collections;

namespace uScoober.DataStructures
{
    public class Queue : IQueue
    {
        private readonly Ring _storage;

        public Queue() {
            _storage = new Ring();
        }

        public Queue(params object[] values)
            : this((IEnumerable)values) { }

        public Queue(IEnumerable values) {
            _storage = new Ring(values);
        }

        public int Count {
            get { return _storage.Count; }
        }

        public bool IsEmpty {
            get { return _storage.Count == 0; }
        }

        public void Clear() {
            _storage.Clear();
        }

        public bool Contains(object item) {
            return _storage.Find(item) != null;
        }

        public void CopyTo(Array array, int startingIndex) {
            _storage.CopyToArray(array, startingIndex);
        }

        public object Dequeue() {
            var result = _storage.Head.Value;
            _storage.RemoveHead();
            return result;
        }

        public void Enqueue(object item) {
            _storage.InsertTail(item);
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

        public static implicit operator object[](Queue queue) {
            return queue._storage.ToArray();
        }

        public static implicit operator Queue(object[] array) {
            return new Queue(array);
        }
    }
}