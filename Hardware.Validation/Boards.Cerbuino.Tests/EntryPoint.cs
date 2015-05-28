using System.Reflection;
using uScoober.Hardware.Boards;
using uScoober.TestFramework;

internal static class EntryPoint
{
    public static void Main() {
        var board = new Cerbuino();
        //IRunnerUserInput input = ??
        GuiTestHarness.RunTests(Assembly.GetExecutingAssembly());
    }
}