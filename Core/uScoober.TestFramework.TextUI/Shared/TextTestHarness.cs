using System;
using System.Reflection;
using uScoober.Hardware.Display;
using uScoober.TestFramework.Core;
using uScoober.TestFramework.UI;

namespace uScoober.TestFramework
{
    internal class TextTestHarness
    {
        public static void RunTests(Assembly assembly, IDisplayText lcd, IRunnerUserInput input = null, string logDirectory = null, bool showGcMessages = false) {
            if (assembly == null) {
                throw new ArgumentNullException("assembly");
            }
            if (lcd == null) {
                throw new ArgumentNullException("lcd");
            }

            //todo: FeedbackToLed ui = new FeedbackToLed(led);
            var ui = new FeedbackToTextDisplay(lcd);
            TestHarness.RunTests(assembly, input, ui, logDirectory, showGcMessages);
        }
    }
}