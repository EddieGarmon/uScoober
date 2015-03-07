using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using uScoober.TestFramework.Sdk;
using uScoober.TestFramework.UI.Resources;

namespace uScoober.TestFramework.UI.Views
{
    internal class TestRunView : ContentControl
    {
        private readonly Microsoft.SPOT.Presentation.Controls.Text _currentTest;
        private readonly Microsoft.SPOT.Presentation.Controls.Text _duration;
        private readonly Microsoft.SPOT.Presentation.Controls.Text _fail;
        private readonly StackPanel _failurePanel;
        private readonly Microsoft.SPOT.Presentation.Controls.Text _pass;
        private int _failureCount;
        private int _failureScrollIndex;
        private TestRun _testRun;

        public TestRunView() {
            Child = new StackPanel(Orientation.Vertical) {
                Width = SystemMetrics.ScreenWidth,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Children = {
                    new Microsoft.SPOT.Presentation.Controls.Text(Fonts.Nina14, " ** uScoober Runner ** ") {
                        ForeColor = Colors.Purple,
                        HorizontalAlignment = HorizontalAlignment.Center
                    },
                    new StackPanel(Orientation.Horizontal) {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Children = {
                            new Microsoft.SPOT.Presentation.Controls.Text(Fonts.Nina14, "Passed:") {
                                ForeColor = Colors.Green,
                                HorizontalAlignment = HorizontalAlignment.Right
                            },
                            (_pass = new Microsoft.SPOT.Presentation.Controls.Text(Fonts.Nina14, "0") {
                                ForeColor = Colors.Green,
                                HorizontalAlignment = HorizontalAlignment.Left
                            }),
                            new Microsoft.SPOT.Presentation.Controls.Text(Fonts.Nina14, "  Failed:") {
                                ForeColor = Colors.Red
                            },
                            (_fail = new Microsoft.SPOT.Presentation.Controls.Text(Fonts.Nina14, "0") {
                                ForeColor = Colors.Red
                            })
                        }
                    },
                    new StackPanel(Orientation.Horizontal) {
                        Children = {
                            new Microsoft.SPOT.Presentation.Controls.Text(Fonts.Nina14, " Testing: "),
                            (_currentTest = new Microsoft.SPOT.Presentation.Controls.Text(Fonts.Nina14, " [Queued]") {
                                TextWrap = true
                            }),
                        }
                    },
                    (_duration = new Microsoft.SPOT.Presentation.Controls.Text(Fonts.Nina14, string.Empty)),
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

        public void UseTestRun(TestRun testRun) {
            _testRun = testRun;
            _failureScrollIndex = _failureCount = 0;
            Dispatcher.BeginInvoke(_ => {
                                       _currentTest.TextContent = " [Queued]";
                                       _pass.TextContent = "0";
                                       _fail.TextContent = "0";
                                       _duration.TextContent = " Started [" + _testRun.RunStarted.ToString("hh:mm:ss") + "]";
                                       _failurePanel.Children.Clear();
                                       return null;
                                   },
                                   null);
            testRun.Updated = UpdateDisplay;
        }

        private void UpdateDisplay() {
            Dispatcher.BeginInvoke(_ => {
                                       //update UI summary elements
                                       _currentTest.TextContent = _testRun.IsComplete ? " [Complete]" : _testRun.CurrentTestName;
                                       _pass.TextContent = _testRun.PassedCount.ToString();
                                       _fail.TextContent = _testRun.FailedCount.ToString();

                                       if (_testRun.IsComplete) {
                                           _duration.TextContent = " Run [" + _testRun.RunStarted.ToString("hh:mm:ss") + " - "
                                                                   + _testRun.RunFinished.ToString("hh:mm:ss") + "] - CUT [" + _testRun.DurationSummary + "]";
                                       }
                                       //capture any new exceptions
                                       if (_failurePanel.Children.Count < _testRun.FailedCount) {
                                           for (int i = _failurePanel.Children.Count; i < _testRun.FailedCount; i++) {
                                               _failurePanel.Children.Add(new TestFailureDetails((TestCase)_testRun.Failures[i], i + 1));
                                           }
                                           _failureCount = _testRun.FailedCount;
                                       }

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