using System.Reflection;
using uScoober.Hardware;
using uScoober.Hardware.Light;
using uScoober.TestFramework;

internal static class EntryPoint
{
    public static void Main() {
        Pin netduinoOnboardLedPin = (Pin)55;
        
        DigitalLed onboardLed = new DigitalLed(netduinoOnboardLedPin);
        LedTestHarness.RunTests(Assembly.GetExecutingAssembly(), onboardLed);
    }
}