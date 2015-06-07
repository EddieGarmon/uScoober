using System.Reflection;
using uScoober.Hardware;
using uScoober.Hardware.Light;
using uScoober.TestFramework;
using uScoober.TestFramework.Input;

internal static class EntryPoint
{
    public static void Main() {
        DigitalLed led = new DigitalLed(Pin.Pin0);
        IDigitalInterrupt startTests = Signals.DigitalInterrupt.Bind(Pin.Pin1, "Restart Testing", ResistorMode.PullUp, InterruptMode.InterruptEdgeLow, 50);
        var input = new GpioInput(startTests);
        LedTestHarness.RunTests(Assembly.GetExecutingAssembly(), led, input);
    }
}