using System;
using System.Collections;

namespace uScoober.DataStructures
{
    public interface IQueue : IEnumerable
    {
        int Count { get; }

        void Clear();

        bool Contains(object item);

        void CopyTo(Array array, int startingIndex);

        object Dequeue();

        void Enqueue(object item);

        object Peek(int index = 0);
    }
}