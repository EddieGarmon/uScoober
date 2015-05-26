using System.Reflection;
using SecretLabs.NETMF.Hardware.Netduino;
using uScoober.Hardware.Text;
using uScoober.TestFramework;

internal static class EntryPoint
{
    public static void Main() {
        //input buttons on mcp
        //lcd char display output using mcp transfer

        ICharacterDisplay charDisplay = null;
        //TestHarness.RunTests(Assembly.GetExecutingAssembly(), null, new FeedbackToCharDisplay(charDisplay));
    }
}