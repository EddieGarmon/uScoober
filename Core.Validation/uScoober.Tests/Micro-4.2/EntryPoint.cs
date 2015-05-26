using System.Reflection;
using uScoober.TestFramework;

internal static class EntryPoint
{
    public static void Main() {
        TestHarness.RunTests(Assembly.GetExecutingAssembly());
    }
}