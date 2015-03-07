using System.Diagnostics;
using uScoober.Text;

namespace uScoober.DataStructures
{
    public partial class Ring
    {
        [DebuggerDisplay("{GetDebugInfo()}")]
        public class IndexedLink
        {
            private readonly Link _link;

            internal IndexedLink(Link link, int index) {
                _link = link;
                Index = index;
            }

            public int Index { get; private set; }

            public Link RingLink {
                get { return _link; }
            }

            public object Value {
                get { return _link.Value; }
            }

            private string GetDebugInfo() {
                return "Index=" + Index + " Id=" + HexString.GetString(_link.GetHashCode()) + " Value=" + (_link.Value ?? "{null}");
            }

            public static implicit operator Link(IndexedLink indexed) {
                return indexed._link;
            }
        }
    }
}