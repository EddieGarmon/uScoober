using System.Reflection;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using uScoober.Hardware;
using uScoober.Hardware.Spot;
using uScoober.TestFramework.Sdk.Runners.Feedback;

namespace uScoober.TestFramework.Sdk.Runners
{
    internal class EmulatorRunner : TestRunner
    {
        private IDigitalInput _restartTests;
        private IDigitalInput _scrollDown;
        private IDigitalInput _scrollUp;

        public EmulatorRunner(Assembly assemblyUnderTest)
            : base(assemblyUnderTest, new GuiFeedback(), true) {
            _restartTests = new SpotDigitalInput(EmulatorButtons.Select, "Restart Tests", ResistorMode.PullUp, InterruptMode.InterruptEdgeLow);
            _restartTests.OnInterupt += (source, state, time) => {
                                            Debug.Print("Attempt Restart");
                                            ExecuteTests();
                                        };
            _restartTests.InteruptEnabled = true;

            _scrollUp = new SpotDigitalInput(EmulatorButtons.Up, "Scroll Up", ResistorMode.PullUp, InterruptMode.InterruptEdgeLow);
            _scrollUp.OnInterupt += (source, state, time) => {
                                        Debug.Print("Scroll Up Requested");
                                        Feedback.ScrollUp();
                                    };
            _scrollUp.InteruptEnabled = true;

            _scrollDown = new SpotDigitalInput(EmulatorButtons.Down, "Scroll Down", ResistorMode.PullUp, InterruptMode.InterruptEdgeLow);
            _scrollDown.OnInterupt += (source, state, time) => {
                                          Debug.Print("Scroll Down Requested");
                                          Feedback.ScrollDown();
                                      };
            _scrollDown.InteruptEnabled = true;
        }

        private class EmulatorButtons
        {
            public const Cpu.Pin Down = Cpu.Pin.GPIO_Pin4;
            public const Cpu.Pin Left = Cpu.Pin.GPIO_Pin0;
            public const Cpu.Pin Right = Cpu.Pin.GPIO_Pin1;
            public const Cpu.Pin Select = Cpu.Pin.GPIO_Pin3;
            public const Cpu.Pin Up = Cpu.Pin.GPIO_Pin2;
        }
    }
}