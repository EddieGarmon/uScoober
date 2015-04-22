using System.Reflection;
using uScoober.Hardware.Boards;
using uScoober.TestFramework;

internal static class EntryPoint
{
    public static void Main() {
        var board = new Cerbuino();
        var output = board.OnboardLed;
        new TestHarness(Assembly.GetExecutingAssembly(), output).ExecuteTests();
    }
}