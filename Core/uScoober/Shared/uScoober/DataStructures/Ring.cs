using System;
using System.Collections;

namespace uScoober.DataStructures
{
    public partial class Ring : IEnumerable
    {
        public Ring() { }

        public Ring(params object[] values)
            : this((IEnumerable)values) { }

        public Ring(IEnumerable values) {
            if (values != null) {
                foreach (object value in values) {
                    InsertTail(value);
                }
            }
            EditVersion = 0;
        }

        public int Count { get; private set; }

        public int EditVersion { get; private set; }

        /// <summary>
        ///     Null when Count == 0, Head otherwise
        /// </summary>
        public Link Head { get; internal set; }

        public bool IsEmpty {
            get { return Head == null; }
        }

        /// <summary>
        ///     Null when Count &lt; 2, Tail otherwise
        /// </summary>
        public Link Tail {
            get { return Head == null ? null : Head.Previous == Head ? null : Head.Previous; }
        }

        public void Clear() {
            EditVersion++;
            Link next = Head;
            for (int i = 0; i < Count; i++) {
                Link current = next;
                next = next.Next;
                current.Invalidate();
            }
            Head = null;
            Count = 0;
        }

        public IndexedLink Find(object value) {
            return FindAndIndex(value, GetForwardEnumerator());
        }

        public IndexedLink Find(Predicate whereClause) {
            return FindAndIndex(whereClause, GetForwardEnumerator());
        }

        public IndexedLink FindAtIndex(int index) {
            if (index < 0 || index >= Count) {
                throw new IndexOutOfRangeException();
            }
            //quick indexes
            if (index == 0) {
                return new IndexedLink(Head, 0);
            }
            if (index == 1) {
                return new IndexedLink(Head.Next, 1);
            }
            if (index + 2 == Count) {
                return new IndexedLink(Tail.Previous, index);
            }
            if (index + 1 == Count) {
                return new IndexedLink(Tail, index);
            }
            // enumerate to find
            var enumerator = new Enumerator(this, (index > Count / 2));
            do {
                enumerator.MoveNext();
            }
            while (enumerator.CurrentIndex != index);
            return new IndexedLink(enumerator.CurrentLink, index);
        }

        public IndexedLink[] FindAtIndexes(params int[] indexes) {
            for (int i = 0; i < indexes.Length; i++) {
                int index = indexes[i];
                if ((index < 0) || (index >= Count)) {
                    throw new IndexOutOfRangeException();
                }
            }
            //one walk forward or backward
            // : forward enumeration if (smallestIndex)<(Count-largestIndex)
            // : reverse enumeration if (count-largest)<(smallest)
            // NB: indexes can be non sorted, and should have the same order in the results

            //build a simple map from sorted input value location (to match enumeration order) 
            //  to output index, to support out of order filling
            throw new NotImplementedException("Ring.FindAtIndexes");
        }

        public IndexedLink FindLast(object value) {
            return FindAndIndex(value, GetReverseEnumerator());
        }

        public IndexedLink FindLast(Predicate whereClause) {
            return FindAndIndex(whereClause, GetReverseEnumerator());
        }

        public Enumerator GetForwardEnumerator() {
            return new Enumerator(this);
        }

        public Enumerator GetReverseEnumerator() {
            return new Enumerator(this, true);
        }

        public void IncrementEditVersion() {
            EditVersion++;
        }

        public Link InsertAfter(Link previous, object value) {
            EditVersion++;
            var link = new Link(value, previous);
            Count++;
            return link;
        }

        public Link InsertAtIndex(int index, object value) {
            if (index == 0) {
                return InsertHead(value);
            }
            if (index == Count) {
                return InsertTail(value);
            }
            IndexedLink findResult = FindAtIndex(index);
            Link result = InsertAfter(findResult.RingLink.Previous, value);
            if (index == 0) {
                Head = findResult.RingLink.Previous;
            }
            return result;
        }

        public Link InsertHead(object value) {
            EditVersion++;
            Head = Head == null ? new Link(value) : new Link(value, Head.Previous);
            Count++;
            return Head;
        }

        public void InsertRangeAtIndex(int index, IEnumerable values) {
            if (index == Count) {
                foreach (object value in values) {
                    InsertTail(value);
                }
                return;
            }
            IndexedLink findResult = FindAtIndex(index);
            Link tail = (index == 0) ? Head.Previous : null;
            Link link = findResult.RingLink.Previous;
            foreach (object value in values) {
                link = InsertAfter(link, value);
            }
            if (tail != null) {
                Head = tail.Next;
            }
        }

        public Link InsertTail(object value) {
            EditVersion++;
            Link result;
            if (Head == null) {
                result = Head = new Link(value);
            }
            else {
                result = new Link(value, Head.Previous);
            }
            Count++;
            return result;
        }

        public object RemoveAtIndex(int index) {
            IndexedLink findResult = FindAtIndex(index);
            object result = findResult.Value;
            RemoveLink(findResult);
            return result;
        }

        public void RemoveHead() {
            RemoveLink(Head);
        }

        public void RemoveLink(Link link) {
            if (link == null) {
                return;
            }
            EditVersion++;
            Link previous = link.Previous;
            Link next = link.Next;
            if (previous == next && next == link && link == Head) {
                //single link ring
                link.Invalidate();
                Head = null;
                Count = 0;
                return;
            }

            if (link == Head) {
                Head = next;
            }
            previous.Next = next;
            next.Previous = previous;
            Count--;
        }

        public void RemoveTail() {
            RemoveLink(Tail);
        }

        public void RemoveWhere(Predicate whereClause) {
            int lastEdit = EditVersion;
            int foundCount = 0;
            Link link = Head;
            Link next = link.Next;
            for (int i = 0; i < Count; i++) {
                if (whereClause(link.Value)) {
                    RemoveLink(link);
                    foundCount++;
                }
                if (Count > 0) {
                    link = next;
                    next = next.Next;
                }
            }
            if (foundCount > 0) {
                EditVersion = lastEdit + 1;
            }
        }

        public void SwapValues(Link left, Link right) {
            EditVersion++;
            object temp = left.Value;
            left.Value = right.Value;
            right.Value = temp;
        }

        public object[] ToArray() {
            var result = new object[Count];
            CopyToArray(result);
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetForwardEnumerator();
        }

        protected internal void CopyToArray(Array array, int index = 0) {
            IList list = array;
            var enumerator = new Enumerator(this);
            while (enumerator.MoveNext()) {
                list[index] = enumerator.Current;
                index++;
            }
        }

        private static IndexedLink FindAndIndex(object value, Enumerator enumerator) {
            return FindAndIndex(obj => obj.Equals(value), enumerator);
        }

        private static IndexedLink FindAndIndex(Predicate whereClause, Enumerator enumerator) {
            while (enumerator.MoveNext()) {
                Link link = enumerator.CurrentLink;
                if (whereClause(link.Value)) {
                    return new IndexedLink(link, enumerator.CurrentIndex);
                }
            }
            return null;
        }
    }
}