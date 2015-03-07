using System.Collections;

namespace uScoober.DataStructures
{
    public partial class Enumerable : IEnumerable
    {
        private readonly IEnumerator _enumerator;

        public Enumerable(IEnumerator enumerator) {
            _enumerator = enumerator;
        }

        public IEnumerator GetEnumerator() {
            return _enumerator;
        }
    }
}