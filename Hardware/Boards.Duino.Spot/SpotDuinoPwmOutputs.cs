using Microsoft.SPOT.Hardware;
using uScoober.Hardware.Spot;

namespace uScoober.Hardware.Boards.Spot
{
    internal abstract class SpotDuinoPwmOutputs : DisposableBase,
                                                  IDuinoPwmOutputs
    {
        private IPulseWidthModulatedOutput _d10;
        private IPulseWidthModulatedOutput _d11;
        private IPulseWidthModulatedOutput _d3;
        private IPulseWidthModulatedOutput _d5;
        private IPulseWidthModulatedOutput _d6;
        private IPulseWidthModulatedOutput _d9;

        public IPulseWidthModulatedOutput D10 {
            get { return _d10 ?? (_d10 = Create(PinD10)); }
        }

        public IPulseWidthModulatedOutput D11 {
            get { return _d11 ?? (_d11 = Create(PinD11)); }
        }

        public IPulseWidthModulatedOutput D3 {
            get { return _d3 ?? (_d3 = Create(PinD3)); }
        }

        public IPulseWidthModulatedOutput D5 {
            get { return _d5 ?? (_d5 = Create(PinD5)); }
        }

        public IPulseWidthModulatedOutput D6 {
            get { return _d6 ?? (_d6 = Create(PinD6)); }
        }

        public IPulseWidthModulatedOutput D9 {
            get { return _d9 ?? (_d9 = Create(PinD9)); }
        }

        protected abstract Cpu.PWMChannel PinD10 { get; }

        protected abstract Cpu.PWMChannel PinD11 { get; }

        protected abstract Cpu.PWMChannel PinD3 { get; }

        protected abstract Cpu.PWMChannel PinD5 { get; }

        protected abstract Cpu.PWMChannel PinD6 { get; }

        protected abstract Cpu.PWMChannel PinD9 { get; }

        protected IPulseWidthModulatedOutput Create(Cpu.PWMChannel channel) {
            return new SpotPwmOutput(channel);
        }

        protected override void DisposeManagedResources() {
            DisposeOutput(ref _d3);
            DisposeOutput(ref _d5);
            DisposeOutput(ref _d6);
            DisposeOutput(ref _d9);
            DisposeOutput(ref _d10);
            DisposeOutput(ref _d11);
        }

        private static void DisposeOutput(ref IPulseWidthModulatedOutput output) {
            if (output == null) {
                return;
            }
            output.Dispose();
            output = null;
        }
    }
}