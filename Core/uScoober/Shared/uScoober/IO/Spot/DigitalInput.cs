using System;
using Microsoft.SPOT.Hardware;
using uScoober.Hardware;
using SpotInputPort = Microsoft.SPOT.Hardware.InputPort;
using SpotInterruptPort = Microsoft.SPOT.Hardware.InterruptPort;

namespace uScoober.IO.Spot
{
    public class DigitalInput : DisposableBase,
                                IDigitalInput
    {
        private readonly SpotInterruptPort _port;
        private InteruptHandler _onInterupt;

        public DigitalInput(Cpu.Pin pin,
                            string id = null,
                            ResistorMode internalResistorMode = ResistorMode.Disabled,
                            InterruptMode interruptMode = InterruptMode.InterruptNone,
                            int debounceMilliseconds = 0) {
            _port = new SpotInterruptPort(pin, false, (Port.ResistorMode)internalResistorMode, (Port.InterruptMode)interruptMode);
            _port.OnInterrupt += ProxyToUserHandler;
            Id = id ?? "DigitalIn-" + pin;
            DebounceMilliseconds = debounceMilliseconds;
        }

        public int DebounceMilliseconds { get; private set; }

        public string Id { get; private set; }

        public bool InteruptEnabled { get; set; }

        public bool InvertReading { get; set; }

        public DateTime LastTriggered { get; private set; }

        public event InteruptHandler OnInterupt {
            add {
                _onInterupt += value;
                InteruptEnabled |= AutoEnableInteruptHandler;
            }
            remove {
                _onInterupt -= value;
                if (_onInterupt == null) {
                    InteruptEnabled &= !AutoEnableInteruptHandler;
                }
            }
        }

        public bool Read() {
            ThrowIfDisposed();
            bool value = _port.Read();
            return InvertReading ? !value : value;
        }

        protected override void DisposeManagedResources() {
            _port.Dispose();
            _onInterupt = null;
        }

        private void ProxyToUserHandler(uint pinNumber, uint value, DateTime time) {
            if (!InteruptEnabled) {
                return;
            }
            if (DebounceMilliseconds > 0 && LastTriggered.AddMilliseconds(DebounceMilliseconds) > time) {
                return;
            }
            LastTriggered = time;
            var handler = _onInterupt;
            if (handler != null) {
                bool newValue = InvertReading ? value == 0 : value == 1;
                handler(this, newValue, time);
            }
        }

        ~DigitalInput() {
            Dispose(DisposeReason.Finalizer);
        }

        public static bool AutoEnableInteruptHandler { get; set; }
    }
}