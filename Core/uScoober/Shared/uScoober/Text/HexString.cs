using System;
using System.Text;

namespace uScoober.Text
{
    /// <summary>
    /// Basic Hex conversion class
    /// </summary>
    public static class HexString
    {
        /// <summary>
        /// Gets the byte represented by the specified string value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte GetByte(string value) {
            var int32 = Convert.ToInt32(value, 16);
            if (int32 < Byte.MinValue || int32 > Byte.MaxValue) {
                throw new Exception("Byte Overflow");
            }
            return (byte)int32;
        }

        /// <summary>
        /// Gets the int represented by the specified string value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int GetInt32(string value) {
            return Convert.ToInt32(value, 16);
        }

        /// <summary>
        /// Gets the hex string representation of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetString(byte value) {
            return value.ToString("X2");
        }

        public static string GetString(byte[] value, int startIndex = 0, int length = -1) {
            if (length < 0) {
                length = value.Length;
            }
            var builder = new StringBuilder(length * 2);
            for (int i = 0; i < length; i++) {
                builder.Append(value[startIndex + i].ToString("X2"));
            }
            return builder.ToString();
        }

        /// <summary>
        /// Gets the hex string representation of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetString(int value) {
            return value.ToString("X2");
        }

        /// <summary>
        /// Gets the hex string representation of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetString(ushort value) {
            return value.ToString("X2");
        }
    }
}