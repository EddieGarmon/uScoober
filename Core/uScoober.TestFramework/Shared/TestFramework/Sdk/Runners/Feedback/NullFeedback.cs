using Microsoft.SPOT.Presentation;

namespace uScoober.TestFramework.Sdk.Runners.Feedback
{
    internal class NullFeedback : ITestRunFeedback
    {
        public bool IsGui {
            get { return false; }
        }

        public UIElement InitializeGui() {
            return null;
        }

        public void ScrollDown() { }

        public void ScrollUp() { }

        public void UseTestRun(TestRun testRun) { }
    }
}