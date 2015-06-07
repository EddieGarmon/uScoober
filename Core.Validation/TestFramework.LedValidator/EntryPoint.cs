using System.Reflection;
using uScoober.Hardware;
using uScoober.Hardware.Light;
using uScoober.TestFramework;
using uScoober.TestFramework.Input;
using SecretLabsPinAssignment = SecretLabs.NETMF.Hardware.Netduino.Pins;

internal static class EntryPoint
{
    public static void Main() {
        DigitalLed led = new DigitalLed((Pin)SecretLabsPinAssignment.ONBOARD_LED);
        IDigitalInterrupt startTests = Signals.DigitalInterrupt.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D0,
                                                                     "Restart Testing",
                                                                     ResistorMode.PullUp,
                                                                     InterruptMode.InterruptEdgeLow,
                                                                     50);
        var input = new GpioInput(startTests);
        LedTestHarness.RunTests(Assembly.GetExecutingAssembly(), led, input);
    }
}