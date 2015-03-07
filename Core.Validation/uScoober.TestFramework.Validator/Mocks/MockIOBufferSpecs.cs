using System;
using uScoober.TestFramework.Assert;

namespace uScoober.TestFramework.Mocks
{
    public class MockIOBufferSpec
    {
        public void AppendWithBufferGrowth_Fact() {
            // storage is    | _A_ | _B_ | _C_ | _D_ |
            // readIndex is  0
            // append        | _1_ | _2_ |
            // storage should become => | _A_ | _B_ | _C_ | _D_ | _1_ | _2_ |
            var mock = new MockIOBuffer(0x0a, 0x0b, 0x0c, 0x0d);
            mock.Append(1, 2);
            mock.Storage.ShouldEnumerateEqual(0x0a, 0x0b, 0x0c, 0x0d, 1, 2);
        }

        public void AppendWithShiftingIndex_Fact() {
            // storage is    | _A_ | _B_ | _C_ | _D_ |
            // readIndex is  3
            // append        | _1_ | _2_ |
            // storage should become => | _A_ | _D_ | _1_ | _2_ |
            // readIndex should become 1
            var mock = new MockIOBuffer(0x0a, 0x0b, 0x0c, 0x0d);
            mock.ReadIndex = 3;
            mock.Append(1, 2);
            mock.Storage.ShouldEnumerateEqual(0x0A, 0x0D, 1, 2);
            mock.ReadIndex.ShouldEqual(1);
        }

        public void CreateByteArrayData_Fact() {
            var x = new byte[0];
            var mock = new MockIOBuffer(x);
            mock.Storage.ShouldReference(x);
        }

        public void CreateMultiByteData_Fact() {
            var mock = new MockIOBuffer(1, 2, 3);
            mock.Storage.ShouldEnumerateEqual(1, 2, 3);
        }

        public void CreateNoData_Fact() {
            var mock = new MockIOBuffer();
            mock.Storage.Length.ShouldEqual(0);
        }

        public void CreateNullData_Fact() {
            var exception = Trap.Exception(() => { var buffer = new MockIOBuffer(null); });
            exception.ShouldBeOfType(typeof(ArgumentNullException));
        }

        public void CreateSingleByteData_Fact() {
            var mock = new MockIOBuffer(0xAB);
            mock.Storage.ShouldEnumerateEqual(0xab);
        }

        public void ReadPastEndThrows_Fact() {
            var mock = new MockIOBuffer(0x0a);
            var output = new byte[8];
            var e = Trap.Exception(() => mock.ReadInto(output, 0, 4));
            e.ShouldBeOfType(typeof(IndexOutOfRangeException));
        }

        public void Read_Fact() {
            var mock = new MockIOBuffer(0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f);
            var output = new byte[8];
            mock.ReadInto(output, 3, 2);
            output.ShouldEnumerateEqual(0, 0, 0, 0x0a, 0x0b, 0, 0, 0);
            mock.ReadIndex.ShouldEqual(2);
            mock.ReadInto(output, 3, 3);
            output.ShouldEnumerateEqual(0, 0, 0, 0x0c, 0x0d, 0x0e, 0, 0);
            mock.ReadIndex.ShouldEqual(5);
            mock.ReadInto(output, 0, 1);
            output.ShouldEnumerateEqual(0x0f, 0, 0, 0x0c, 0x0d, 0x0e, 0, 0);
            mock.ReadIndex.ShouldEqual(6);
        }
    }
}