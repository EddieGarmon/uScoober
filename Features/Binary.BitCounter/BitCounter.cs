namespace uScoober.Binary
{
    internal static class BitCounter
    {
        internal static readonly uint[] Masks32 = {
            0x55555555,
            0x33333333,
            0x0F0F0F0F,
            0x00FF00FF,
            0x0000FFFF
        };

        internal static readonly ulong[] Masks64 = {
            0x5555555555555555,
            0x3333333333333333,
            0x0F0F0F0F0F0F0F0F,
            0x00FF00FF00FF00FF,
            0x0000FFFF0000FFFF,
            0x00000000FFFFFFFF
        };

        public static int CountBits(sbyte data) {
            return CountBits((byte)data);
        }

        public static int CountBits(byte data) {
            for (int i = 0; i < 3; i++) {
                data = (byte)((data & Masks32[i]) + ((data >> (1 << i)) & Masks32[i]));
            }
            return data;
        }

        public static int CountBits(char data) {
            return CountBits((ushort)data);
        }

        public static int CountBits(short data) {
            return CountBits((ushort)data);
        }

        public static int CountBits(ushort data) {
            for (int i = 0; i < 4; i++) {
                data = (ushort)((data & Masks32[i]) + ((data >> (1 << i)) & Masks32[i]));
            }
            return data;
        }

        public static int CountBits(int data) {
            return CountBits((uint)data);
        }

        public static int CountBits(uint data) {
            for (int i = 0; i < 5; i++) {
                data = (data & Masks32[i]) + ((data >> (1 << i)) & Masks32[i]);
            }
            return (int)data;
        }

        public static int CountBits(long data) {
            return CountBits((ulong)data);
        }

        public static int CountBits(ulong data) {
            for (int i = 0; i < 6; i++) {
                data = (data & Masks64[i]) + ((data >> (1 << i)) & Masks64[i]);
            }
            return (int)data;
        }
    }
}