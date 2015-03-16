using System;
using uScoober.TestFramework.Assert;

namespace uScoober.Extensions
{
    public class FloatExtensionTests
    {
        public void FloatToInt32_Fact() {
            float source = 3.14f;
            int expected = 0x4048F5C3;

            byte[] asBytes = BitConverter.GetBytes(source);
            BitConverter.ToInt32(asBytes, 0)
                        .ShouldEqual(expected);

            source.GetHashFast()
                  .ShouldEqual(expected);
        }
    }
}