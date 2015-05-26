using Microsoft.SPOT.Hardware;
using uScoober.Hardware;
using uScoober.Hardware.Spot;
using uScoober.TestFramework.Core;

namespace uScoober.TestFramework
{
    internal class EmulatorButtons : IRunnerUserInput
    {
        private readonly IDigitalInterupt _scrollDown;
        private readonly IDigitalInterupt _scrollUp;
        private readonly IDigitalInterupt _startTests;

        public EmulatorButtons() {
            _startTests = new SpotDigitalInterupt(EmulatorPins.Select,
                                                  "Restart Tests",
                                                  (Port.ResistorMode)ResistorMode.PullUp,
                                                  (Port.InterruptMode)InterruptMode.InterruptEdgeLow);

            _scrollUp = new SpotDigitalInterupt(EmulatorPins.Up,
                                                "Scroll Up",
                                                (Port.ResistorMode)ResistorMode.PullUp,
                                                (Port.InterruptMode)InterruptMode.InterruptEdgeLow);

            _scrollDown = new SpotDigitalInterupt(EmulatorPins.Down,
                                                  "Scroll Down",
                                                  (Port.ResistorMode)ResistorMode.PullUp,
                                                  (Port.InterruptMode)InterruptMode.InterruptEdgeLow);
        }

        public IDigitalInterupt ScrollDown {
            get { return _scrollDown; }
        }

        public IDigitalInterupt ScrollUp {
            get { return _scrollUp; }
        }

        public IDigitalInterupt StartTests {
            get { return _startTests; }
        }

        private class EmulatorPins
        {
            public const Cpu.Pin Down = Cpu.Pin.GPIO_Pin4;
            public const Cpu.Pin Left = Cpu.Pin.GPIO_Pin0;
            public const Cpu.Pin Right = Cpu.Pin.GPIO_Pin1;
            public const Cpu.Pin Select = Cpu.Pin.GPIO_Pin3;
            public const Cpu.Pin Up = Cpu.Pin.GPIO_Pin2;
        }
    }
}