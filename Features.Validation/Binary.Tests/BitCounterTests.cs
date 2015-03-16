using uScoober.TestFramework.Assert;

namespace uScoober.Binary
{
    public class BitCounterTests
    {
        public void EightHasOneBit_Fact() {
            int value = 8;
            BitCounter.CountBits(value)
                      .ShouldEqual(1);
        }
    }
}