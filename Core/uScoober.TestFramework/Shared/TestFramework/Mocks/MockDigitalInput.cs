using System;
using uScoober.Hardware;

namespace uScoober.TestFramework.Mocks
{
    public class MockDigitalInput : DisposableBase,
                                    IDigitalInput
    {
        public MockDigitalInput(string id) {
            Id = id;
        }

        public string Id { get; private set; }

        public bool InteruptEnabled { get; set; }

        public bool InvertReading { get; set; }

        public ProvideBool ValueProvider { get; set; }

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

        public bool Read() {
            ThrowIfDisposed();
            bool value = ValueProvider();
            return InvertReading ? !value : value;
        }
    }
}