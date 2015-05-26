using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using TinyFonts;
using uScoober.TestFramework.Core;

namespace uScoober.TestFramework.UI.Views
{
    internal class TestRunView : ContentControl,
                                 IRunnerResultProcessor
    {
        private readonly Microsoft.SPOT.Presentation.Controls.Text _currentTest;
        private readonly Microsoft.SPOT.Presentation.Controls.Text _duration;
        private readonly Microsoft.SPOT.Presentation.Controls.Text _fail;
        private readonly StackPanel _failurePanel;
        private readonly Microsoft.SPOT.Presentation.Controls.Text _pass;
        private int _failureCount;
        private int _failureScrollIndex;
        private TestRunResult _runResults;

        public TestRunView() {
            Child = new StackPanel(Orientation.Vertical) {
                Width = SystemMetrics.ScreenWidth,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Children = {
                    new Microsoft.SPOT.Presentation.Controls.Text(Nina.Nina14, " ** uScoober Runner ** ") {
                        ForeColor = Colors.Purple,
                        HorizontalAlignment = HorizontalAlignment.Center
                    },
                    new StackPanel(Orientation.Horizontal) {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Children = {
                            new Microsoft.SPOT.Presentation.Controls.Text(Nina.Nina14, "Passed:") {
                                ForeColor = Colors.Green,
                                HorizontalAlignment = HorizontalAlignment.Right
                            },
                            (_pass = new Microsoft.SPOT.Presentation.Controls.Text(Nina.Nina14, "0") {
                                ForeColor = Colors.Green,
                                HorizontalAlignment = HorizontalAlignment.Left
                            }),
                            new Microsoft.SPOT.Presentation.Controls.Text(Nina.Nina14, "  Failed:") {
                                ForeColor = Colors.Red
                            },
                            (_fail = new Microsoft.SPOT.Presentation.Controls.Text(Nina.Nina14, "0") {
                                ForeColor = Colors.Red
                            })
                        }
                    },
                    new StackPanel(Orientation.Horizontal) {
                        Children = {
                            new Microsoft.SPOT.Presentation.Controls.Text(Nina.Nina14, " Testing: "),
                            (_currentTest = new Microsoft.SPOT.Presentation.Controls.Text(Nina.Nina14, " [Queued]") {
                                TextWrap = true
                            })
                        }
                    },
                    (_duration = new Microsoft.SPOT.Presentation.Controls.Text(Nina.Nina14, string.Empty)),
                    (_failurePanel = new StackPanel(Orientation.Vertical))
                },
                Visibility = Visibility.Visible
            };
        }

        public void ScrollDown() {
            if (_failureScrollIndex >= _failureCount - 1) {
                return;
            }
            _failureScrollIndex++;
            UpdateScroll();
        }

        public void ScrollUp() {
            if (_failureScrollIndex <= 0) {
                return;
            }
            _failureScrollIndex--;
            UpdateScroll();
        }

        public void TestCaseCompleted(TestCaseResult result) {
            Dispatcher.BeginInvoke(_ => {
                                       _pass.TextContent = _runResults.PassedCount.ToString();
                                       _fail.TextContent = _runResults.FailedCount.ToString();
                                       if (!result.Passed) {
                                           _failurePanel.Children.Add(new TestFailureDetails(result, _failurePanel.Children.Count + 1));
                                       }
                                       return null;
                                   },
                                   null);
        }

        public void TestCaseStarting(string testName) {
            Dispatcher.BeginInvoke(_ => {
                                       _currentTest.TextContent = testName;
                                       return null;
                                   },
                                   null);
        }

        public void TestsCompleted(TestRunResult runResults) {
            Dispatcher.BeginInvoke(_ => {
                                       _currentTest.TextContent = " [Complete]";
                                       _pass.TextContent = _runResults.PassedCount.ToString();
                                       _fail.TextContent = _runResults.FailedCount.ToString();
                                       _duration.TextContent = " Run [" + _runResults.RunStarted.ToString("hh:mm:ss") + " - "
                                                               + _runResults.RunFinished.ToString("hh:mm:ss") + "] - CUT [" + _runResults.DurationSummary + "]";

                                       return null;
                                   },
                                   null);
        }

        public void TestsStarting(TestRunResult runResults) {
            _runResults = runResults;
            _failureScrollIndex = _failureCount = 0;
            Dispatcher.BeginInvoke(_ => {
                                       _currentTest.TextContent = " [Queued]";
                                       _pass.TextContent = "0";
                                       _fail.TextContent = "0";
                                       _duration.TextContent = " Started [" + _runResults.RunStarted.ToString("hh:mm:ss") + "]";
                                       _failurePanel.Children.Clear();
                                       return null;
                                   },
                                   null);
        }

        private void UpdateScroll() {
            if (_failureCount == 0) {
                return;
            }
            Dispatcher.BeginInvoke(_ => {
                                       //update failures panel
                                       for (int i = 0; i < _failureScrollIndex; i++) {
                                           _failurePanel.Children[i].Visibility = Visibility.Collapsed;
                                       }
                                       for (int i = _failureScrollIndex; i < _failurePanel.Children.Count; i++) {
                                           _failurePanel.Children[i].Visibility = Visibility.Visible;
                                       }
                                       return null;
                                   },
                                   null);
        }
    }
}