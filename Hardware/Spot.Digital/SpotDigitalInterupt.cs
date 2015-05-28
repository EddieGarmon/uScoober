using System;
using System.Runtime.CompilerServices;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware.Spot
{
    internal class SpotDigitalInterrupt : DisposableBase,
                                          IDigitalInterrupt
    {
        private static bool __autoEnableInterruptHandler = true;
        private readonly int _debounceMilliseconds;
        private readonly InterruptPort _interrupt;
        private readonly string _name;
        private bool _interruptEnabled;
        private bool _invertReading;
        private DateTime _lastTriggered = DateTime.MinValue;
        private InterruptHandler _onInterrupt;

        static SpotDigitalInterrupt() {
            Signals.DigitalInterrupt.NewInstance =
                (pin, name, mode, interruptMode, milliseconds) =>
                new SpotDigitalInterrupt((Cpu.Pin)pin, name, (Port.ResistorMode)mode, (Port.InterruptMode)interruptMode, milliseconds);
        }

        private SpotDigitalInterrupt(Cpu.Pin pin,
                                     string name = null,
                                     Port.ResistorMode internalResistorMode = Port.ResistorMode.Disabled,
                                     Port.InterruptMode interruptMode = Port.InterruptMode.InterruptNone,
                                     int debounceMilliseconds = 0) {
            _interrupt = new InterruptPort(pin, false, internalResistorMode, interruptMode);
            _interrupt.OnInterrupt += ProxyToUserHandler;
            _name = name ?? "DigitalInterrupt-" + pin;
            _debounceMilliseconds = debounceMilliseconds;
        }

        public int DebounceMilliseconds {
            get {
                ThrowIfDisposed();
                return _debounceMilliseconds;
            }
        }

        public bool InterruptEnabled {
            get {
                ThrowIfDisposed();
                return _interruptEnabled;
            }
            set {
                ThrowIfDisposed();
                _interruptEnabled = value;
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

        public Pin Pin {
            get {
                ThrowIfDisposed();
                return (Pin)_interrupt.Id;
            }
        }

        public static bool AutoEnableInterruptHandler {
            get { return __autoEnableInterruptHandler; }
            set { __autoEnableInterruptHandler = value; }
        }

        public event InterruptHandler OnInterrupt {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add {
                ThrowIfDisposed();
                _onInterrupt = (InterruptHandler)WeakDelegate.Combine(_onInterrupt, value);
                InterruptEnabled |= AutoEnableInterruptHandler;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove {
                ThrowIfDisposed();
                _onInterrupt = (InterruptHandler)WeakDelegate.Remove(_onInterrupt, value);
                if (_onInterrupt == null) {
                    InterruptEnabled &= !AutoEnableInterruptHandler;
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
            _onInterrupt = null;
        }

        private void ProxyToUserHandler(uint pinNumber, uint value, DateTime time) {
            ThrowIfDisposed();
            if (!InterruptEnabled) {
                return;
            }
            DateTime debounceEnds = LastTriggered.AddMilliseconds(DebounceMilliseconds);
            if (DebounceMilliseconds > 0 && debounceEnds > time) {
                return;
            }
            _lastTriggered = time;
            InterruptHandler handler = _onInterrupt;
            if (handler != null) {
                bool newValue = InvertReading ? value == 0 : value == 1;
                handler(this, newValue, time);
            }
            //todo - need callback to possibly fire after debounce time if were toggled
        }

        ~SpotDigitalInterrupt() {
            Dispose(DisposeReason.Finalizer);
        }
    }
}