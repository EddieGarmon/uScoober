using System.Reflection;
using uScoober.TestFramework;

internal static class EntryPoint
{
    public static void Main() {
        GuiTestHarness.RunTests(Assembly.GetExecutingAssembly());
    }
}