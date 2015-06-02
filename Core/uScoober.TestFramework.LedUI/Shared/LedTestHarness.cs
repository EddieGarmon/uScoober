using System;
using System.Reflection;
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

            FeedbackToLed ui = new FeedbackToLed(led);

            TestHarness.RunTests(assembly, input, ui, logDirectory, showGcMessages);
        }
    }
}