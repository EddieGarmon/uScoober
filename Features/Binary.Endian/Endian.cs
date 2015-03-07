// Inspired by Jon Skeet

using System;

namespace uScoober.Binary
{
    // This a managed replacement for BitConverter that does both big and little endian conversions
    public static class Endian
    {
        public const bool MachineIsLittleEndian = true;

        /// <summary>
        /// The native types are in machine endian, the byte[]s are in big endian
        /// </summary>
        public static readonly IEndianByter Big = new BigEndianByter();

        /// <summary>
        /// The native types are in machine endian, the byte[]s are in little endian
        /// </summary>
        public static readonly IEndianByter Little = new LittleEndianByter();

        public static short Swap(short value) {
            return (short)Swap((ushort)value);
        }

        public static ushort Swap(ushort value) {
            uint temp = (value & 0x00ffu) << 8;
            temp |= (value & 0xff00u) >> 8;
            return (ushort)temp;
        }

        public static int Swap(int value) {
            return (int)Swap((uint)value);
        }

        public static uint Swap(uint value) {
            uint temp = (value & 0x000000ffu) << 24;
            temp |= (value & 0x0000ff00u) << 8;
            temp |= (value & 0x00ff0000u) >> 8;
            temp |= (value & 0xff000000u) >> 24;
            return temp;
        }

        public static long Swap(long value) {
            return (long)Swap((ulong)value);
        }

        public static ulong Swap(ulong value) {
            var temp = (value & 0x00000000000000FFul) << 56;
            temp |= (value & 0x000000000000FF00ul) << 40;
            temp |= (value & 0x0000000000FF0000ul) << 24;
            temp |= (value & 0x00000000FF000000ul) << 8;
            temp |= (value & 0x000000FF00000000ul) >> 8;
            temp |= (value & 0x0000FF0000000000ul) >> 24;
            temp |= (value & 0x00FF000000000000ul) >> 40;
            temp |= (value & 0xFF00000000000000ul) >> 56;
            return temp;
        }

        public interface IEndianByter
        {
            void CopyBytes(short value, byte[] buffer, int startIndex);

            void CopyBytes(ushort value, byte[] buffer, int startIndex);

            void CopyBytes(int value, byte[] buffer, int startIndex);

            void CopyBytes(uint value, byte[] buffer, int startIndex);

            void CopyBytes(long value, byte[] buffer, int startIndex);

            void CopyBytes(ulong value, byte[] buffer, int startIndex);

            void CopyBytes(float value, byte[] buffer, int startIndex);

            void CopyBytes(double value, byte[] buffer, int startIndex);

            byte[] GetBytes(short value);

            byte[] GetBytes(ushort value);

            byte[] GetBytes(int value);

            byte[] GetBytes(uint value);

            byte[] GetBytes(long value);

            byte[] GetBytes(ulong value);

            byte[] GetBytes(float value);

            byte[] GetBytes(double value);

            double GetDouble(byte[] buffer, int startIndex = 0);

            float GetFloat(byte[] buffer, int startIndex = 0);

            int GetInt(byte[] buffer, int startIndex = 0);

            long GetLong(byte[] buffer, int startIndex = 0);

            short GetShort(byte[] buffer, int startIndex = 0);

            uint GetUInt(byte[] buffer, int startIndex = 0);

            ulong GetULong(byte[] buffer, int startIndex = 0);

            ushort GetUShort(byte[] buffer, int startIndex = 0);
        }

        /// <summary>
        /// The native types are in machine endian, the byte[]s are in big endian
        /// </summary>
        private class BigEndianByter : EndianByter
        {
            protected override long Extract(byte[] buffer, int startIndex, int length) {
                long result = 0;
                unchecked {
                    for (int i = 0; i < length; i++) {
                        result = (result << 8) | buffer[startIndex + i];
                    }
                }
                return result;
            }

            protected override void Fill(byte[] buffer, long value, int startIndex, int length) {
                unchecked {
                    for (int i = length - 1; i >= 0; i--) {
                        buffer[startIndex + i] = (byte)(value & 0xff);
                        value = value >> 8;
                    }
                }
            }
        }

        private abstract class EndianByter : IEndianByter
        {
            public void CopyBytes(short value, byte[] buffer, int startIndex) {
                ValidateBuffer(buffer, startIndex, 2);
                Fill(buffer, value, startIndex, 2);
            }

            public void CopyBytes(ushort value, byte[] buffer, int startIndex) {
                ValidateBuffer(buffer, startIndex, 2);
                Fill(buffer, value, startIndex, 2);
            }

            public void CopyBytes(int value, byte[] buffer, int startIndex) {
                ValidateBuffer(buffer, startIndex, 4);
                Fill(buffer, value, startIndex, 4);
            }

            public void CopyBytes(uint value, byte[] buffer, int startIndex) {
                ValidateBuffer(buffer, startIndex, 4);
                Fill(buffer, value, startIndex, 4);
            }

            public void CopyBytes(long value, byte[] buffer, int startIndex) {
                ValidateBuffer(buffer, startIndex, 8);
                Fill(buffer, value, startIndex, 8);
            }

            public void CopyBytes(ulong value, byte[] buffer, int startIndex) {
                ValidateBuffer(buffer, startIndex, 8);
                Fill(buffer, unchecked((long)value), startIndex, 8);
            }

            public unsafe void CopyBytes(float value, byte[] buffer, int startIndex) {
                ValidateBuffer(buffer, startIndex, 4);
                int temp = *((int*)&value);
                Fill(buffer, temp, startIndex, 4);
            }

            public unsafe void CopyBytes(double value, byte[] buffer, int startIndex) {
                ValidateBuffer(buffer, startIndex, 8);
                long temp = *((long*)&value);
                Fill(buffer, temp, startIndex, 8);
            }

            public byte[] GetBytes(short value) {
                var result = new byte[2];
                Fill(result, value, 0, 2);
                return result;
            }

            public byte[] GetBytes(ushort value) {
                var result = new byte[2];
                Fill(result, value, 0, 2);
                return result;
            }

            public byte[] GetBytes(int value) {
                var result = new byte[4];
                Fill(result, value, 0, 4);
                return result;
            }

            public byte[] GetBytes(uint value) {
                var result = new byte[4];
                Fill(result, value, 0, 4);
                return result;
            }

            public byte[] GetBytes(long value) {
                var result = new byte[8];
                Fill(result, value, 0, 8);
                return result;
            }

            public byte[] GetBytes(ulong value) {
                var result = new byte[8];
                Fill(result, unchecked((long)value), 0, 8);
                return result;
            }

            public unsafe byte[] GetBytes(float value) {
                int temp = *((int*)&value);
                return GetBytes(temp);
            }

            public unsafe byte[] GetBytes(double value) {
                long temp = *((long*)&value);
                return GetBytes(temp);
            }

            public unsafe double GetDouble(byte[] buffer, int startIndex = 0) {
                ValidateBuffer(buffer, startIndex, 8);
                long temp = Extract(buffer, startIndex, 8);
                return *((double*)&temp);
            }

            public unsafe float GetFloat(byte[] buffer, int startIndex = 0) {
                ValidateBuffer(buffer, startIndex, 4);
                var temp = unchecked((int)Extract(buffer, startIndex, 4));
                return *((float*)&temp);
            }

            public int GetInt(byte[] buffer, int startIndex = 0) {
                ValidateBuffer(buffer, startIndex, 4);
                return unchecked((int)Extract(buffer, startIndex, 4));
            }

            public long GetLong(byte[] buffer, int startIndex = 0) {
                ValidateBuffer(buffer, startIndex, 8);
                return Extract(buffer, startIndex, 8);
            }

            public short GetShort(byte[] buffer, int startIndex = 0) {
                ValidateBuffer(buffer, startIndex, 2);
                return unchecked((short)Extract(buffer, startIndex, 2));
            }

            public uint GetUInt(byte[] buffer, int startIndex = 0) {
                ValidateBuffer(buffer, startIndex, 4);
                return unchecked((uint)Extract(buffer, startIndex, 4));
            }

            public ulong GetULong(byte[] buffer, int startIndex = 0) {
                ValidateBuffer(buffer, startIndex, 8);
                return unchecked((ulong)Extract(buffer, startIndex, 8));
            }

            public ushort GetUShort(byte[] buffer, int startIndex = 0) {
                ValidateBuffer(buffer, startIndex, 2);
                return unchecked((ushort)Extract(buffer, startIndex, 2));
            }

            protected abstract long Extract(byte[] buffer, int startIndex, int length);

            protected abstract void Fill(byte[] buffer, long value, int startIndex, int length);

            private void ValidateBuffer(byte[] buffer, int startIndex, int length) {
                if (buffer == null) {
                    throw new ArgumentNullException("buffer");
                }
                if ((startIndex < 0) || startIndex >= buffer.Length) {
                    throw new ArgumentOutOfRangeException("startIndex");
                }
                if (startIndex + length > buffer.Length) {
                    throw new ArgumentOutOfRangeException("length");
                }
            }
        }

        /// <summary>
        /// The native types are in machine endian, the byte[]s are in little endian
        /// </summary>
        private class LittleEndianByter : EndianByter
        {
            protected override long Extract(byte[] buffer, int startIndex, int length) {
                long result = 0;
                unchecked {
                    for (int i = startIndex + length - 1; i >= startIndex; i--) {
                        result = (result << 8) | buffer[startIndex + i];
                    }
                }
                return result;
            }

            protected override void Fill(byte[] buffer, long value, int startIndex, int length) {
                unchecked {
                    for (int i = 0; i < length; i++) {
                        buffer[startIndex + i] = (byte)(value & 0xff);
                        value = value >> 8;
                    }
                }
            }
        }
    }
}