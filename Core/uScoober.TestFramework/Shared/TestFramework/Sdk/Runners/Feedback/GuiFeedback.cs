using Microsoft.SPOT.Presentation;
using uScoober.TestFramework.UI.Views;

namespace uScoober.TestFramework.Sdk.Runners.Feedback
{
    internal class GuiFeedback : ITestRunFeedback
    {
        private TestRunView _view;

        public bool IsGui {
            get { return true; }
        }

        public UIElement InitializeGui() {
            _view = new TestRunView();
            return _view;
        }

        public void ScrollDown() {
            _view.ScrollDown();
        }

        public void ScrollUp() {
            _view.ScrollUp();
        }

        public virtual void UseTestRun(TestRun testRun) {
            _view.UseTestRun(testRun);
        }
    }
}