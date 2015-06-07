using System.Reflection;
using uScoober.Hardware;
using uScoober.Hardware.Light;
using uScoober.TestFramework;
using uScoober.TestFramework.Core;
using SecretLabsPinAssignment = SecretLabs.NETMF.Hardware.Netduino.Pins;

internal static class EntryPoint
{
    public static void Main() {
        DigitalLed onboardLed = new DigitalLed((Pin)SecretLabsPinAssignment.ONBOARD_LED);
        UserInput input = new UserInput(Signals.DigitalInterrupt.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D0, "Restart Testing", ResistorMode.PullUp, InterruptMode.InterruptEdgeLow, 50));
        LedTestHarness.RunTests(Assembly.GetExecutingAssembly(), onboardLed);
    }

    private class UserInput : IRunnerUserInput
    {
        public UserInput(IDigitalInterrupt startTests) {
            StartTests = startTests;
        }

        public IDigitalInterrupt StartTests { get; private set; }

        public IDigitalInterrupt ScrollUp { get { return null; } }

        public IDigitalInterrupt ScrollDown { get { return null; } }
    }

}