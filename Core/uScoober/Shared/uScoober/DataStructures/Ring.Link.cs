using System.Diagnostics;
using uScoober.Text;

namespace uScoober.DataStructures
{
    public partial class Ring
    {
        [DebuggerDisplay("{GetDebugInfo()}")]
        public class Link
        {
            /// <summary>
            /// Creates a single link ring.
            /// </summary>
            /// <param name="value"></param>
            internal Link(object value) {
                Value = value;
                Previous = this;
                Next = this;
            }

            /// <summary>
            /// Creates and connects a new link after the specified link
            /// </summary>
            /// <param name="value"></param>
            /// <param name="previous"></param>
            internal Link(object value, Link previous) {
                Value = value;
                Previous = previous;
                Next = previous.Next;
                Previous.Next = this;
                Next.Previous = this;
            }

            public Link Next { get; internal set; }

            public Link Previous { get; internal set; }

            public object Value { get; set; }

            public void Invalidate() {
                Value = null;
                Previous = null;
                Next = null;
            }

            private string GetDebugInfo() {
                return "Id=" + HexString.GetString(GetHashCode()) + " Value=" + (Value ?? "{null}");
            }
        }
    }
}