using System;
using System.Collections;

namespace uScoober.DataStructures
{
    public interface IStack : IEnumerable
    {
        int Count { get; }

        void Clear();

        bool Contains(object item);

        void CopyTo(Array array, int arrayIndex);

        object Peek(int index = 0);

        object Pop();

        void Push(object item);
    }
}