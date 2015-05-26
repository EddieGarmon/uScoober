using System.Reflection;
using SecretLabs.NETMF.Hardware.Netduino;
using uScoober.Hardware.Light;
using uScoober.Hardware.Spot;
using uScoober.TestFramework;

internal static class EntryPoint
{
    public static void Main() {
        DigitalLed onboardLed = new DigitalLed(new SpotDigitalOutput(Pins.ONBOARD_LED));
        LedTestHarness.RunTests(Assembly.GetExecutingAssembly(), onboardLed);
    }
}