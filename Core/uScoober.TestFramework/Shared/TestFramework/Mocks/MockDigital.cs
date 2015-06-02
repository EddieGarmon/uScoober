using System;
using System.Runtime.CompilerServices;
using Microsoft.SPOT;
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

    public class MockDigitalInterrupt : MockDigitalInput,
                                        IDigitalInterrupt
    {
        private InterruptHandler _onInterrupt;

        public MockDigitalInterrupt(string name)
            : base(name) { }

        public bool InterruptEnabled { get; set; }

        public event InterruptHandler OnInterrupt {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add {
                ThrowIfDisposed();
                _onInterrupt = (InterruptHandler)WeakDelegate.Combine(_onInterrupt, value);
                InterruptEnabled |= DigitalInterupt.AutoEnableInterruptHandler;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove {
                ThrowIfDisposed();
                _onInterrupt = (InterruptHandler)WeakDelegate.Remove(_onInterrupt, value);
                if (_onInterrupt == null) {
                    InterruptEnabled &= !DigitalInterupt.AutoEnableInterruptHandler;
                }
            }
        }

        public void RaiseInterrupt() {
            RaiseInterrupt(Read(), DateTime.Now);
        }

        public void RaiseInterrupt(DateTime time) {
            RaiseInterrupt(Read(), time);
        }

        public void RaiseInterrupt(bool state, DateTime time) {
            if (!InterruptEnabled) {
                return;
            }
            InterruptHandler handler = _onInterrupt;
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