using System;

namespace uScoober.DataStructures
{
    public class Buffer
    {
        private int _readIndex;
        private byte[] _storage;
        private int _writeIndex;

        public Buffer(byte[] storage) {
            if (storage == null) {
                throw new ArgumentNullException("storage");
            }
            _storage = storage;
        }

        public Buffer(byte[] storage, int startIndex, int length) {
            if (storage == null) {
                throw new ArgumentNullException("storage");
            }
            if (storage.Length < startIndex + length) {
                throw new ArgumentOutOfRangeException("length");
            }
            _storage = storage;
            _readIndex = startIndex;
            _writeIndex = startIndex + length;
        }

        public Buffer(int length) {
            _storage = new byte[length];
        }

        public void Append(byte value) { }

        public void Append(byte value1, byte value2) { }

        public void Append(byte value1, byte value2, byte value3) { }

        public void Append(params byte[] values) { }

        public void Append(byte[] buffer, int startIndex, int count) { }

        public void Clear() {
            _readIndex = 0;
            _writeIndex = 0;
        }

        public byte Read() {
            throw new NotImplementedException("Buffer.Read");
        }

        public void ReadInto(byte[] outputBuffer, int startIndex, int count) { }

        private void CollectGarbage() { }
    }
}