using System.Reflection;
using uScoober.Hardware;
using uScoober.Hardware.Display;
using uScoober.TestFramework;
using uScoober.TestFramework.Input;

internal static class EntryPoint
{
    public static void Main() {
        /*  Pick a driver that matches your hardware setup */
        IDriveTextDisplays driver = null;

        CharacterDisplay lcd = new CharacterDisplay(20, 4, driver) {
            IsCursorBlinking = false,
            IsCursorUnderlined = false
        };

        /* Hook user input buttons */
        IDigitalInterrupt startTests = Signals.DigitalInterrupt.Bind(Pin.Pin1, "Restart Testing", ResistorMode.PullUp, InterruptMode.InterruptEdgeLow, 50);
        var input = new GpioInput(startTests);

        /* Run the tests */
        lcd.ClearScreen();
        TextTestHarness.RunTests(Assembly.GetExecutingAssembly(), lcd, input);
    }
}