using uScoober.Hardware;

namespace uScoober.TestFramework.Core
{
    public interface IRunnerUserInput
    {
        IDigitalInterupt ScrollDown { get; }

        IDigitalInterupt ScrollUp { get; }

        IDigitalInterupt StartTests { get; }
    }
}