using uScoober.IO;

namespace uScoober.TestFramework.Mocks
{
    public class MockAnalogInput : DisposableBase,
                                   IAnalogInput
    {
        public MockAnalogInput(string id) {
            Id = id;
        }

        public string Id { get; private set; }

        public ProvideDouble ValueProvider { get; set; }

        public double Read() {
            ThrowIfDisposed();
            return ValueProvider();
        }
    }
}