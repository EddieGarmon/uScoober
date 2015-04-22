using System;
using System.Runtime.CompilerServices;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware.Spot
{
    internal class SpotDigitalInterupt : DisposableBase,
                                         IDigitalInterupt
    {
        private static bool __autoEnableInteruptHandler = true;
        private readonly int _debounceMilliseconds;
        private readonly InterruptPort _interrupt;
        private readonly string _name;
        private bool _interuptEnabled;
        private bool _invertReading;
        private DateTime _lastTriggered = DateTime.MinValue;
        private InteruptHandler _onInterupt;

        public SpotDigitalInterupt(Cpu.Pin pin,
                                   string name = null,
                                   Port.ResistorMode internalResistorMode = Port.ResistorMode.Disabled,
                                   Port.InterruptMode interruptMode = Port.InterruptMode.InterruptNone,
                                   int debounceMilliseconds = 0) {
            _interrupt = new InterruptPort(pin, false, internalResistorMode, interruptMode);
            _interrupt.OnInterrupt += ProxyToUserHandler;
            _name = name ?? "DigitalInterupt-" + pin;
            _debounceMilliseconds = debounceMilliseconds;
        }

        public int DebounceMilliseconds {
            get {
                ThrowIfDisposed();
                return _debounceMilliseconds;
            }
        }

        public bool InteruptEnabled {
            get {
                ThrowIfDisposed();
                return _interuptEnabled;
            }
            set {
                ThrowIfDisposed();
                _interuptEnabled = value;
            }
        }

        public bool InvertReading {
            get {
                ThrowIfDisposed();
                return _invertReading;
            }
            set {
                ThrowIfDisposed();
                _invertReading = value;
            }
        }

        public DateTime LastTriggered {
            get {
                ThrowIfDisposed();
                return _lastTriggered;
            }
        }

        public string Name {
            get {
                ThrowIfDisposed();
                return _name;
            }
        }

        public int Pin {
            get {
                ThrowIfDisposed();
                return (int)_interrupt.Id;
            }
        }

        public static bool AutoEnableInteruptHandler {
            get { return __autoEnableInteruptHandler; }
            set { __autoEnableInteruptHandler = value; }
        }

        public event InteruptHandler OnInterupt {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add {
                ThrowIfDisposed();
                _onInterupt = (InteruptHandler)WeakDelegate.Combine(_onInterupt, value);
                InteruptEnabled |= AutoEnableInteruptHandler;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove {
                ThrowIfDisposed();
                _onInterupt = (InteruptHandler)WeakDelegate.Remove(_onInterupt, value);
                if (_onInterupt == null) {
                    InteruptEnabled &= !AutoEnableInteruptHandler;
                }
            }
        }

        public bool Read() {
            ThrowIfDisposed();
            bool value = _interrupt.Read();
            return InvertReading ? !value : value;
        }

        protected override void DisposeManagedResources() {
            _interrupt.Dispose();
            _onInterupt = null;
        }

        private void ProxyToUserHandler(uint pinNumber, uint value, DateTime time) {
            ThrowIfDisposed();
            if (!InteruptEnabled) {
                return;
            }
            DateTime debounceEnds = LastTriggered.AddMilliseconds(DebounceMilliseconds);
            if (DebounceMilliseconds > 0 && debounceEnds > time) {
                return;
            }
            _lastTriggered = time;
            InteruptHandler handler = _onInterupt;
            if (handler != null) {
                bool newValue = InvertReading ? value == 0 : value == 1;
                handler(this, newValue, time);
            }
            //todo - need callback to possibly fire after debounce time if were toggled
        }

        ~SpotDigitalInterupt() {
            Dispose(DisposeReason.Finalizer);
        }
    }
}