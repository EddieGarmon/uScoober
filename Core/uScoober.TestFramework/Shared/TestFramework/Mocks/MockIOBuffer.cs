using System;
using uScoober.Extensions;

namespace uScoober.TestFramework.Mocks
{
    public class MockIOBuffer
    {
        public MockIOBuffer(params byte[] initialValues) {
            if (initialValues == null) {
                throw new ArgumentNullException("initialValues");
            }
            Storage = initialValues;
        }

        public byte[] Storage { get; private set; }

        public int ReadIndex { get; set; }

        public void Append(params byte[] newValues) {
            int numberOfUnreadValues = Storage.Length - ReadIndex;
            int sizeNeeded = numberOfUnreadValues + newValues.Length;

            if (sizeNeeded <= Storage.Length) {
                int shiftTo = ReadIndex - newValues.Length;
                for (int i = 0; i < numberOfUnreadValues; i++) {
                    Storage[shiftTo + i] = Storage[ReadIndex + i];
                }

                ReadIndex = ReadIndex - newValues.Length;
                int copyTo = Storage.Length - newValues.Length;

                for (int i = 0; i < newValues.Length; i++) {
                    Storage[copyTo + i] = newValues[i];
                }
            }
            else {
                var temp = new byte[sizeNeeded];
                Storage.CopyTo(temp, ReadIndex, 0, numberOfUnreadValues);
                newValues.CopyTo(temp, numberOfUnreadValues);
                Storage = temp;
                ReadIndex = 0;
            }
        }

        public void Clear() {
            Storage = new byte[0];
            ReadIndex = 0;
        }

        public void ReadInto(byte[] outputBuffer, int startIndex, int count) {
            for (int i = 0; i < count; i++) {
                outputBuffer[startIndex + i] = Storage[ReadIndex];
                ReadIndex++;
            }
        }
    }
}