using System;
using System.Collections;
using Microsoft.SPOT.Hardware;
using uScoober.Hardware.Spot;

namespace uScoober.Hardware.Boards.Spot
{
    internal abstract class SpotDuinoDigitalInputs : DisposableBase,
                                                 IDuinoDigitalInputs
    {
        private readonly Hashtable _store = new Hashtable();

        public IDigitalInterupt Bind(Cpu.Pin pin, ResistorMode internalResistorMode = ResistorMode.Disabled, string id = null, int debounceMilliseconds = 0) {
            var input = new SpotDigitalInterupt(pin,
                                                id,
                                                (Port.ResistorMode)internalResistorMode,
                                                (Port.InterruptMode)InterruptMode.InterruptNone,
                                                debounceMilliseconds);
            _store.Add(pin, input);
            return input;
        }

        public IDigitalInterupt Bind(Cpu.Pin pin,
                                     ResistorMode internalResistorMode,
                                     InterruptMode interruptMode,
                                     InteruptHandler handler,
                                     string id = null,
                                     int debounceMilliseconds = 0,
                                     bool interuptEnabled = true) {
            var input = new SpotDigitalInterupt(pin, id, (Port.ResistorMode)internalResistorMode, (Port.InterruptMode)interruptMode, debounceMilliseconds);
            input.OnInterupt += handler; //todo: verify
            _store.Add(pin, input);
            return input;
        }

        public IDigitalInterupt Get(Cpu.Pin pin) {
            return (SpotDigitalInterupt)_store[pin];
        }

        public IDigitalInterupt Invert(Cpu.Pin pin, ResistorMode internalResistorMode = ResistorMode.Disabled, string id = null, int debounceMilliseconds = 0) {
            var input = Bind(pin, internalResistorMode, id, debounceMilliseconds);
            input.InvertReading = true;
            return input;
        }

        public IDigitalInterupt Invert(Cpu.Pin pin,
                                       ResistorMode internalResistorMode,
                                       InterruptMode interruptMode,
                                       InteruptHandler handler,
                                       string id = null,
                                       int debounceMilliseconds = 0,
                                       bool interuptEnabled = true) {
            var input = new SpotDigitalInterupt(pin, id, (Port.ResistorMode)internalResistorMode, (Port.InterruptMode)interruptMode, debounceMilliseconds);
            throw new NotImplementedException("register handler");
            _store.Add(pin, input);
            return input;
        }

        protected override void DisposeManagedResources() {
            foreach (IDisposable disposable in _store.Values) {
                disposable.Dispose();
            }
            _store.Clear();
        }
    }
}