using System;

namespace uScoober.Hardware.Input
{
    internal class PushButton : DisposableBase,
                              IButton
    {
        private readonly IDigitalInput _input;
        private Action _onButtonDown = delegate { };
        private Action _onButtonUp = delegate { };

        // todo: add a digital debounce via a minimum time between state changes?

        public PushButton(IDigitalInput input) {
            _input = input;
            _input.OnInterupt += HandleInterupt;
        }

        public event Action ButtonDown {
            add { _onButtonDown += value; }
            remove { _onButtonDown -= value; }
        }

        public event Action ButtonUp {
            add { _onButtonUp += value; }
            remove { _onButtonUp -= value; }
        }

        protected override void DisposeManagedResources() {
            _input.OnInterupt -= HandleInterupt;
            _input.Dispose();
            _onButtonDown = null;
            _onButtonUp = null;
        }

        private void HandleInterupt(IDigitalInput source, bool newPinState, DateTime time) {
            if (newPinState) {
                _onButtonDown();
            }
            else {
                _onButtonUp();
            }
        }
    }
}