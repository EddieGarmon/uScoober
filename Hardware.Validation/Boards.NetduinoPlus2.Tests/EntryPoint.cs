using System.Reflection;
using uScoober.TestFramework;

internal static class EntryPoint
{
    public static void Main() {
        var netduino = new uScoober.Hardware.Boards.NetduinoPlus2();
        new TestHarness(Assembly.GetExecutingAssembly()).ExecuteTests();
    }
}