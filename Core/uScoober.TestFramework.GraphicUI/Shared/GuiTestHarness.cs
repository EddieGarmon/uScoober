using System;
using System.Reflection;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation;
using uScoober.TestFramework.Core;
using uScoober.TestFramework.Output;
using uScoober.TestFramework.UI.Views;
using uScoober.Threading;

namespace uScoober.TestFramework
{
    public class GuiTestHarness : Application
    {
        private readonly Assembly _assembly;
        private readonly IRunnerUserInput _input;
        private readonly string _logDirectory;

        private GuiTestHarness(Assembly assembly, IRunnerUserInput input, string logDirectory) {
            _assembly = assembly;
            _input = SystemInfo.IsEmulator ? new EmulatorButtons() : input;
            _logDirectory = logDirectory;
        }

        protected override void OnStartup(EventArgs e) {
            base.OnStartup(e);

            var dispatch = new FeedbackDispatcher();

            //NB: we must wait and create all UI objects in 'OnStartup' so they are on the dispatcher thread
            TestRunView gui = new TestRunView();
            MainWindow.Child = gui;

            dispatch.Add(gui);
            dispatch.Add(new FeedbackToDebug());
            if (BuildAutomation.InBuild) {
                dispatch.Add(new FeedbackToLogFiles());
            }
            else if (_logDirectory != null) {
                dispatch.Add(new FeedbackToLogFiles(_logDirectory));
            }

            // connect input scroll buttons
            if (_input != null) {
                if (_input.ScrollUp != null) {
                    _input.ScrollUp.OnInterrupt += (source, state, time) => {
                                                      Debug.Print("Scroll Up Requested");
                                                      gui.ScrollUp();
                                                  };
                }
                if (_input.ScrollDown != null) {
                    _input.ScrollDown.OnInterrupt += (source, state, time) => {
                                                        Debug.Print("Scroll Down Requested");
                                                        gui.ScrollDown();
                                                    };
                }
            }

            var runner = new TestRunner(_assembly, _input, dispatch);

            if (BuildAutomation.InBuild) {
                runner.ExecuteTests()
                      .ContinueWith(previous => Dispatcher.BeginInvoke(StartShutdown, null));
            }
            else {
                runner.ExecuteTests();
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

        public static void RunTests(Assembly assembly, IRunnerUserInput input = null, string logDirectory = null, bool showGcMessages = false) {
            if (assembly == null) {
                throw new ArgumentNullException("assembly");
            }
            Debug.EnableGCMessages(showGcMessages);
            GuiTestHarness app = new GuiTestHarness(assembly, input, logDirectory);
            app.Run(new Window {
                Height = SystemMetrics.ScreenHeight,
                Width = SystemMetrics.ScreenWidth,
                Visibility = Visibility.Visible
            });
        }
    }
}