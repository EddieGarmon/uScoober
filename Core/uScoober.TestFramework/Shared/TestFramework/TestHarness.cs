using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation;
using uScoober.Hardware.Light;
using uScoober.TestFramework.Sdk.Runners;
using uScoober.TestFramework.Sdk.Runners.Feedback;
using uScoober.Threading;

namespace uScoober.TestFramework
{
    public class TestHarness : Application
    {
        private readonly bool _exitAfterFirstRun;
        private readonly TestRunner _testRunner;

        public TestHarness(Assembly assemblyUnderTest, bool showGcMessages = true)
            : this(assemblyUnderTest, (TestRunner)null, showGcMessages) { }

        public TestHarness(Assembly assemblyUnderTest, IAnalogLed analogLed, bool showGcMessages = true)
            : this(assemblyUnderTest, new TestRunner(assemblyUnderTest, new AnalogLedFeedback(analogLed), false), showGcMessages) { }

        //todo: support character display test feedback
        //public TestHarness(Assembly assemblyUnderTest, ICharacterDisplay characterDisplay, bool showGcMessages = true) {
        //    _assemblyUnderTest = assemblyUnderTest;
        //    if (!SystemInfo.IsEmulator) {
        //        throw new NotImplementedException();
        //        _testRunPresenter = null; // new CharacterFeedback(characterDisplay);
        //    }
        //    Debug.EnableGCMessages(showGcMessages);
        //}

        private TestHarness(Assembly assemblyUnderTest, TestRunner runner, bool showGcMessages = true) {
            //todo? use a file in emulated file system?
            if (File.Exists(BuildRunnerFeedback.BuildMarkerFilename)) {
                _testRunner = new BuildRunner(assemblyUnderTest);
                _exitAfterFirstRun = true;
            }
            else if (runner != null) {
                _testRunner = runner;
            }
            else {
                if (SystemInfo.IsEmulator) {
                    _testRunner = new EmulatorRunner(assemblyUnderTest);
                }
                else if (SystemMetrics.ScreenHeight > 0) {
                    _testRunner = new TestRunner(assemblyUnderTest, new GuiFeedback(), false);
                }
                else {
                    _testRunner = new TestRunner(assemblyUnderTest, new NullFeedback(), false);
                }
            }
            Debug.EnableGCMessages(showGcMessages);
        }

        public void ExecuteTests() {
            Run(_testRunner.Feedback.IsGui
                    ? new Window {
                        Height = SystemMetrics.ScreenHeight,
                        Width = SystemMetrics.ScreenWidth,
                        Visibility = Visibility.Visible
                    }
                    : null);
        }

        protected override void OnStartup(EventArgs e) {
            base.OnStartup(e);

            //NB: we must wait and create all UI objects in 'OnStartup' so they are on the dispatcher thread
            var gui = _testRunner.Feedback.InitializeGui();
            if (gui != null) {
                if (MainWindow == null) {
                    Debugger.Break();
                    throw new Exception("Did you call TestHarness.Run() instead of TestHarness.ExecuteTests()?");
                }
                MainWindow.Child = gui;
            }

            _testRunner.ExecuteTests();

            if (_exitAfterFirstRun) {
                Task.Run(() => {
                             while (_testRunner.State != TestRunnerState.Stopped) {
                                 Thread.Sleep(10);
                             }
                         })
                    .ContinueWith(previous => Dispatcher.BeginInvoke(StartShutdown, null));
            }
        }

        private object StartShutdown(object unused) {
            //shutdown task threads
            TaskScheduler.UnusedThreadTimeoutMilliseconds = 10;
            // empty task to pump all task threads
            Task.Run(() => { });

            // shutdown the application
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            Shutdown();
            return null;
        }
    }
}