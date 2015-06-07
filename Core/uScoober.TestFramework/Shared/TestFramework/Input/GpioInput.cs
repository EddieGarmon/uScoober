using uScoober.Hardware;
using uScoober.TestFramework.Core;

namespace uScoober.TestFramework.Input
{
    public class GpioInput : IRunnerUserInput
    {
        public GpioInput(IDigitalInterrupt startTests, IDigitalInterrupt scrollUp = null, IDigitalInterrupt scrollDown = null) {
            StartTests = startTests;
            ScrollUp = scrollUp;
            ScrollDown = scrollDown;
        }

        public IDigitalInterrupt ScrollDown { get; private set; }

        public IDigitalInterrupt ScrollUp { get; private set; }

        public IDigitalInterrupt StartTests { get; private set; }
    }
}