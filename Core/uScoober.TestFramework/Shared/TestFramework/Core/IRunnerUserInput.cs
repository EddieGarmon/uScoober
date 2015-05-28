using uScoober.Hardware;

namespace uScoober.TestFramework.Core
{
    public interface IRunnerUserInput
    {
        IDigitalInterrupt ScrollDown { get; }

        IDigitalInterrupt ScrollUp { get; }

        IDigitalInterrupt StartTests { get; }
    }
}