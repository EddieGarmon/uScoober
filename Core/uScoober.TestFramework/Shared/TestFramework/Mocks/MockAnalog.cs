using uScoober.Hardware;

namespace uScoober.TestFramework.Mocks
{
    public class MockAnalogInput : MockSignal,
                                   IAnalogInput
    {
        public MockAnalogInput(string name)
            : base(name) { }

        public ProvideDouble ValueProvider { get; set; }

        public double Read() {
            ThrowIfDisposed();
            return ValueProvider();
        }
    }
}