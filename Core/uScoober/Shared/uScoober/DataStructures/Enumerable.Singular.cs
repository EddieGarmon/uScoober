using System.Collections;

namespace uScoober.DataStructures
{
    public partial class Enumerable
    {
        public class Singular : IEnumerable,
                                IEnumerator
        {
            private readonly object _value;
            private int _index = -1;

            public Singular(object value) {
                _value = value;
            }

            object IEnumerator.Current {
                get { return _index == 0 ? _value : null; }
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return this;
            }

            bool IEnumerator.MoveNext() {
                if (_index < 1) {
                    _index++;
                }
                return _index == 0;
            }

            void IEnumerator.Reset() {
                _index = -1;
            }
        }
    }
}