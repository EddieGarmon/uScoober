using System.Reflection;
using uScoober.Hardware;
using uScoober.Hardware.Boards;
using uScoober.TestFramework;

internal static class EntryPoint
{
    public static void Main() {
        //use multiple tasks to do setup, then launch into the tests
        //Task.WaitAll(Task.Run(() => {
        //                          while (IPAddress.GetDefaultLocalAddress() == IPAddress.Any) {
        //                              Thread.Sleep(100);
        //                          }
        //                          Debug.Print(IPAddress.GetDefaultLocalAddress()
        //                                               .ToString());
        //                      }),
        //             Task.Run(() => { /* setup storage */
        //                      }),
        //             Task.Run(() => { /* setup something else */
        //                      }),
        //);

        var board = new Netduino();
        board.OnboardLed.Blink(10, 200);

        // how to bind signals
        Signals.DigitalOutput.Bind(board.Pins.D0);
        Signals.DigitalInput.Bind(board.Pins.D1,"sample this line",ResistorMode.PullUp);
        Signals.DigitalInterrupt.Bind(board.Pins.D2,"interruptable line",ResistorMode.PullUp,InterruptMode.InterruptEdgeLow);

        Signals.AnalogInput.Bind(board.Analog.PinA0);

        Signals.PwmOutput.Bind(board.Pwm.PinD5);

        //attach LCD - 

        // have every port print name to LCD on interupt

        GuiTestHarness.RunTests(Assembly.GetExecutingAssembly());
    }
}