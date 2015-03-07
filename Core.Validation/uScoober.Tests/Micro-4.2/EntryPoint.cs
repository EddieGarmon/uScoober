using System.Reflection;
using uScoober.TestFramework;

public static class EntryPoint
{
    public static void Main() {
        new TestHarness(Assembly.GetExecutingAssembly()).ExecuteTests();
    }
}