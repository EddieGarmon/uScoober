using System;
using uScoober.Hardware;

namespace uScoober.TestFramework.Mocks
{
    public class MockDigitalInput : MockSignal,
                                    IDigitalInput
    {
        protected MockDigitalInput(string name = null)
            : base(name) { }

        public bool InvertReading { get; set; }

        public ProvideBool ValueProvider { get; set; }

        public bool Read() {
            ThrowIfDisposed();
            bool value = ValueProvider();
            return InvertReading ? !value : value;
        }
    }

    public class MockDigitalInterupt : MockDigitalInput,
                                       IDigitalInterupt
    {
        public MockDigitalInterupt(string name)
            : base(name) { }

        public bool InteruptEnabled { get; set; }

        public event InteruptHandler OnInterupt;

        public void RaiseInterupt() {
            RaiseInterupt(Read(), DateTime.Now);
        }

        public void RaiseInterupt(DateTime time) {
            RaiseInterupt(Read(), time);
        }

        public void RaiseInterupt(bool state, DateTime time) {
            if (!InteruptEnabled) {
                return;
            }
            InteruptHandler handler = OnInterupt;
            if (handler != null) {
                bool newValue = InvertReading ? !state : state;
                handler(this, newValue, time);
            }
        }
    }

    public class MockDigitalOutput : MockSignal,
                                     IDigitalOutput
    {
        public MockDigitalOutput(string name, bool initialState = false)
            : base(name) {
            State = initialState;
        }

        public bool State { get; private set; }

        public void Write(bool state) {
            ThrowIfDisposed();
            State = state;
        }
    }
}