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
        private readonly IDigitalInterupt _restartTests;
        private readonly IDigitalInterupt _scrollDown;
        private readonly IDigitalInterupt _scrollUp;

        public EmulatorRunner(Assembly assemblyUnderTest)
            : base(assemblyUnderTest, new GuiFeedback(), true) {
            _restartTests = new SpotDigitalInterupt(EmulatorButtons.Select,
                                                    "Restart Tests",
                                                    (Port.ResistorMode)ResistorMode.PullUp,
                                                    (Port.InterruptMode)InterruptMode.InterruptEdgeLow);
            _restartTests.OnInterupt += (source, state, time) => {
                                            Debug.Print("Attempt Restart");
                                            ExecuteTests();
                                        };
            _restartTests.InteruptEnabled = true;

            _scrollUp = new SpotDigitalInterupt(EmulatorButtons.Up,
                                                "Scroll Up",
                                                (Port.ResistorMode)ResistorMode.PullUp,
                                                (Port.InterruptMode)InterruptMode.InterruptEdgeLow);
            _scrollUp.OnInterupt += (source, state, time) => {
                                        Debug.Print("Scroll Up Requested");
                                        Feedback.ScrollUp();
                                    };
            _scrollUp.InteruptEnabled = true;

            _scrollDown = new SpotDigitalInterupt(EmulatorButtons.Down,
                                                  "Scroll Down",
                                                  (Port.ResistorMode)ResistorMode.PullUp,
                                                  (Port.InterruptMode)InterruptMode.InterruptEdgeLow);
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