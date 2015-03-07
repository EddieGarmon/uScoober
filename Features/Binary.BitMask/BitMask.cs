namespace uScoober.Binary
{
    public static class BitMask
    {
        public static byte GetValue(byte value, byte mask) {
            return (byte)(value & mask);
        }

        public static bool IsAllOff(byte value, byte mask) {
            return (~value & mask) == mask;
        }

        public static bool IsAllOn(byte value, byte mask) {
            return (value & mask) == mask;
        }

        public static byte TurnOffBits(byte value, byte mask) {
            return (byte)(value & ~mask);
        }

        public static byte TurnOnBits(byte value, byte mask) {
            return (byte)(value | mask);
        }
    }

    // todo: which syntax do we like here: static call or extensions?
    public static class BitMaskExtensions
    {
        public static byte WithMask(this byte value, byte mask) {
            return (byte)(value & mask);
        }
    }
}