using System;

namespace uScoober.Extensions
{
    public static class ArrayExtensions
    {
        public static bool Contains(this Type[] array, Type value) {
            for (int i = 0; i < array.Length; i++) {
                if (array[i] == value) {
                    return true;
                }
            }
            return false;
        }

        public static void CopyTo(this byte[] source, byte[] dest, int sourceStart, int destStart, int count) {
            for (int i = 0; i < count; i++) {
                dest[destStart + i] = source[sourceStart + i];
            }
        }
    }
}