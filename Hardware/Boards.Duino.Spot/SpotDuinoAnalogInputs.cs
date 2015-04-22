using Microsoft.SPOT.Hardware;
using uScoober.Hardware.Spot;

namespace uScoober.Hardware.Boards.Spot
{
    internal abstract class SpotDuinoAnalogInputs : DisposableBase,
                                                    IDuinoAnalogInputs
    {
        private IAnalogInput _a0;
        private IAnalogInput _a1;
        private IAnalogInput _a2;
        private IAnalogInput _a3;
        private IAnalogInput _a4;
        private IAnalogInput _a5;

        public IAnalogInput A0 {
            get { return _a0 ?? (_a0 = Create(PinA0)); }
        }

        public IAnalogInput A1 {
            get { return _a1 ?? (_a1 = Create(PinA1)); }
        }

        public IAnalogInput A2 {
            get { return _a2 ?? (_a2 = Create(PinA2)); }
        }

        public IAnalogInput A3 {
            get { return _a3 ?? (_a3 = Create(PinA3)); }
        }

        public IAnalogInput A4 {
            get { return _a4 ?? (_a4 = Create(PinA4)); }
        }

        public IAnalogInput A5 {
            get { return _a5 ?? (_a5 = Create(PinA5)); }
        }

        protected abstract Cpu.AnalogChannel PinA0 { get; }

        protected abstract Cpu.AnalogChannel PinA1 { get; }

        protected abstract Cpu.AnalogChannel PinA2 { get; }

        protected abstract Cpu.AnalogChannel PinA3 { get; }

        protected abstract Cpu.AnalogChannel PinA4 { get; }

        protected abstract Cpu.AnalogChannel PinA5 { get; }

        protected IAnalogInput Create(Cpu.AnalogChannel channel) {
            return new SpotAnalogInput(channel);
        }

        protected override void DisposeManagedResources() {
            DisposeInput(ref _a0);
            DisposeInput(ref _a1);
            DisposeInput(ref _a2);
            DisposeInput(ref _a3);
            DisposeInput(ref _a4);
            DisposeInput(ref _a5);
        }

        private static void DisposeInput(ref IAnalogInput input) {
            if (input == null) {
                return;
            }
            input.Dispose();
            input = null;
        }
    }
}