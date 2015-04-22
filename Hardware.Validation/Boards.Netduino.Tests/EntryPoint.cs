using System.Reflection;
using uScoober.Hardware.Light;
using uScoober.TestFramework;

internal static class EntryPoint
{
    public static void Main()
    {
        var netduino = new uScoober.Hardware.Boards.Netduino();
        var led = netduino.OnboardLed;
        var led2 = new AnalogLed(netduino.PwmOut.D11);
        new TestHarness(Assembly.GetExecutingAssembly(), led).ExecuteTests();
    }
}