using uScoober.Hardware;

namespace uScoober.TestFramework.Mocks
{
    public class MockSignal : DisposableBase,
                              ISignal
    {
        protected MockSignal(string name = null) {
            Name = name;
        }

        public string Name { get; private set; }

        public int Pin {
            get { return 0; }
        }
    }
}