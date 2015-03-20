using System;

namespace uScoober.Binary
{
    internal static class BinaryCodedDecimal
    {
        public static byte Pack(int value) {
            if (value < 0 || value > 99) {
                throw new ArgumentOutOfRangeException("value");
            }
            return (byte)(((value / 10) << 4) | (value % 10));
        }

        public static int Unpack(byte value) {
            return (value >> 4) * 10 + (value & 0x0F);
        }
    }
}