using System;
using System.Collections;
using Microsoft.SPOT.Hardware;
using uScoober.Hardware.Spot;

namespace uScoober.Hardware.Boards.Spot
{
    internal abstract class SpotDuinoDigitalOutputs : DisposableBase,
                                                      IDuinoDigitalOutputs
    {
        private readonly Hashtable _store = new Hashtable();

        public IDigitalOutput Bind(Cpu.Pin pin, bool initialState = false, string id = null) {
            var output = new SpotDigitalOutput(pin, initialState, id);
            _store.Add(pin, output);
            return output;
        }

        public IDigitalOutput Get(Cpu.Pin pin) {
            return (IDigitalOutput)_store[pin];
        }

        protected override void DisposeManagedResources() {
            foreach (IDisposable disposable in _store.Values) {
                disposable.Dispose();
            }
            _store.Clear();
        }
    }
}