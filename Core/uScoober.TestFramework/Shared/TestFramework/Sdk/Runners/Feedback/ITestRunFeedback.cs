using Microsoft.SPOT.Presentation;

namespace uScoober.TestFramework.Sdk.Runners.Feedback
{
    internal interface ITestRunFeedback
    {
        bool IsGui { get; }

        UIElement InitializeGui();

        void ScrollDown();

        void ScrollUp();

        void UseTestRun(TestRun testRun);
    }
}