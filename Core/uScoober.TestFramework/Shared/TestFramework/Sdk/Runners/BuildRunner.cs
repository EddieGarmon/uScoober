using System.Reflection;
using uScoober.TestFramework.Sdk.Runners.Feedback;

namespace uScoober.TestFramework.Sdk.Runners
{
    internal class BuildRunner : TestRunner
    {
        internal BuildRunner(Assembly assemblyUnderTest)
            : base(assemblyUnderTest, new BuildRunnerFeedback(), false) { }
    }
}