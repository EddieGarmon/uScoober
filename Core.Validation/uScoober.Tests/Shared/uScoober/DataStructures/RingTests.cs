using uScoober.TestFramework.Assert;

namespace uScoober.DataStructures
{
    public class RingTests
    {
        public void CreateEmpty_Fact() {
            var ring = new Ring();
            ring.Count.ShouldEqual(0);
            ring.EditVersion.ShouldEqual(0);
        }

        public void CreateFromEnumerable_Fact() {
            string[] values = {
                "a",
                "b",
                "c"
            };
            var ring = new Ring(values);
            ring.Count.ShouldEqual(3);
            ring.EditVersion.ShouldEqual(0);
        }
    }
}