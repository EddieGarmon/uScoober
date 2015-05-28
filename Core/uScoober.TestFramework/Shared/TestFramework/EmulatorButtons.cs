using uScoober.Hardware;
using uScoober.TestFramework.Core;

namespace uScoober.TestFramework
{
    public class EmulatorButtons : IRunnerUserInput
    {
        private readonly IDigitalInterrupt _scrollDown;
        private readonly IDigitalInterrupt _scrollUp;
        private readonly IDigitalInterrupt _startTests;

        public EmulatorButtons() {
            _startTests = Signals.DigitalInterrupt.Bind(EmulatorPins.Select, "Restart Tests", ResistorMode.PullUp, InterruptMode.InterruptEdgeLow);

            _scrollUp = Signals.DigitalInterrupt.Bind(EmulatorPins.Up, "Scroll Up", ResistorMode.PullUp, InterruptMode.InterruptEdgeLow);

            _scrollDown = Signals.DigitalInterrupt.Bind(EmulatorPins.Down, "Scroll Down", ResistorMode.PullUp, InterruptMode.InterruptEdgeLow);
        }

        public IDigitalInterrupt ScrollDown {
            get { return _scrollDown; }
        }

        public IDigitalInterrupt ScrollUp {
            get { return _scrollUp; }
        }

        public IDigitalInterrupt StartTests {
            get { return _startTests; }
        }

        private class EmulatorPins
        {
            public const Pin Down = Pin.Pin4;
            public const Pin Left = Pin.Pin0;
            public const Pin Right = Pin.Pin1;
            public const Pin Select = Pin.Pin3;
            public const Pin Up = Pin.Pin2;
        }
    }
}