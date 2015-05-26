using System;
using System.Reflection;
using System.Threading;
using Microsoft.SPOT;
using uScoober.Hardware.Light;
using uScoober.TestFramework.Core;
using uScoober.TestFramework.UI;

namespace uScoober.TestFramework
{
    public class LedTestHarness
    {
        public static void RunTests(Assembly assembly, IDigitalLed led, IRunnerUserInput input = null, string logDirectory = null, bool showGcMessages = false) {
            if (assembly == null) {
                throw new ArgumentNullException("assembly");
            }
            if (led == null) {
                throw new ArgumentNullException("led");
            }
            Debug.EnableGCMessages(showGcMessages);

            FeedbackToLed ui = new FeedbackToLed(led);

            var dispatch = new FeedbackDispatcher(ui, new FeedbackToDebug());
            if (BuildAutomation.InBuild) {
                dispatch.Add(new FeedbackToLogFiles());
            } else if (logDirectory != null) {
                dispatch.Add(new FeedbackToLogFiles(logDirectory));
            }

            var runner = new TestRunner(assembly, input, dispatch);
            runner.ExecuteTests()
                  .Wait();

            if (!BuildAutomation.InBuild) {
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}