using uScoober.IO;

namespace uScoober.TestFramework.Mocks
{
    public class MockDigitalOutput : DisposableBase,
                                     IDigitalOutput
    {
        public MockDigitalOutput(string id, bool initialState = false) {
            State = initialState;
            Id = id;
        }

        public string Id { get; private set; }

        public bool State { get; private set; }

        public void Write(bool state) {
            ThrowIfDisposed();
            State = state;
        }
    }
}