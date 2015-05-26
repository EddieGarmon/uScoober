using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using uScoober.Hardware;
using uScoober.Hardware.Boards;
using uScoober.Hardware.Light;

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
        //                      }));

        var netduino = new Netduino();
        netduino.OnboardLed.Blink(10, 200);

        //inputs
        Cpu.Pin pin =  Pins.GPIO_PIN_D0;
        var resistorMode = ResistorMode.PullUp;
        int debounceMilliseconds = 0;
        //ctor - always start in listen mode
        var portState = PortState.HighImpedance;

        IDigitalInterupt interupt = netduino.DigitalIn.Invert(pin,
                                                              resistorMode,
                                                              InterruptMode.InterruptEdgeBoth,
                                                              (source, state, time) => {
                                                                  switch (portState) {
                                                                      case PortState.WriteLow:
                                                                      case PortState.WriteHigh:
                                                                          //ignore our writes
                                                                          break;
                                                                      case PortState.HighImpedance:
                                                                          //fire here if enabled
                                                                          netduino.OnboardLed.IsOn = state;
                                                                          break;
                                                                      default:
                                                                          throw new ArgumentOutOfRangeException();
                                                                  }
                                                              },
                                                              "PortIn-" + pin,
                                                              debounceMilliseconds);



        Thread.Sleep(Timeout.Infinite);
        //new TestHarness(Assembly.GetExecutingAssembly(), led).ExecuteTests();
    }


    private enum PortState
    {
        WriteLow,
        WriteHigh,
        HighImpedance
    }
}