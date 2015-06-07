using System.Reflection;
using uScoober.Hardware;
using uScoober.Hardware.Display;
using uScoober.Hardware.I2C;
using uScoober.Hardware.IO;
using uScoober.Hardware.Spot;
using uScoober.TestFramework;
using uScoober.TestFramework.Core;
using SecretLabsPinAssignment = SecretLabs.NETMF.Hardware.Netduino.Pins;

internal static class EntryPoint
{
    public static void Main() {
        //todo: include a schematic

        /*  Pick a driver that matches your hardware setup */
        IDriveTextDisplays driver = null;
        // GPIO or MCP23017 driver 
        driver = GetGpio8Driver();
        //driver = GetGpio4Driver();
        //driver = GetMcp8Driver();
        //driver = GetMcp4Driver();

        CharacterDisplay lcd = new CharacterDisplay(20, 4, driver) {
            IsCursorBlinking = false,
            IsCursorUnderlined = false
        };

        /* Hook user input buttons */
        IRunnerUserInput input = null;
        // GPIO or MCP23017 indirected
        input = GetGpioInput();

        /* Run the tests */
        lcd.ClearScreen();
        TextTestHarness.RunTests(Assembly.GetExecutingAssembly(), lcd, input);
    }

    private static IDriveTextDisplays GetGpio4Driver() {
        // bind these pins low if using 8bit data bus connection
        IDigitalOutput data0 = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D6, "LCD data0");
        IDigitalOutput data1 = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D7, "LCD data1");
        IDigitalOutput data2 = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D8, "LCD data2");
        IDigitalOutput data3 = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D9, "LCD data3");

        IDigitalOutput data4 = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D10, "LCD data4");
        IDigitalOutput data5 = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D11, "LCD data5");
        IDigitalOutput data6 = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D12, "LCD data6");
        IDigitalOutput data7 = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D13, "LCD data7");

        IDigitalOutput enable = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D5, "LCD enable");
        IDigitalOutput registerSelect = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D4, "LCD registerSelect");

        return new GpioTextDriver(data4, data5, data6, data7, enable, registerSelect);
    }

    private static IDriveTextDisplays GetGpio8Driver() {
        IDigitalOutput data0 = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D6, "LCD data0");
        IDigitalOutput data1 = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D7, "LCD data1");
        IDigitalOutput data2 = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D8, "LCD data2");
        IDigitalOutput data3 = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D9, "LCD data3");

        IDigitalOutput data4 = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D10, "LCD data4");
        IDigitalOutput data5 = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D11, "LCD data5");
        IDigitalOutput data6 = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D12, "LCD data6");
        IDigitalOutput data7 = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D13, "LCD data7");

        IDigitalOutput enable = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D5, "LCD enable");
        IDigitalOutput registerSelect = Signals.DigitalOutput.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D4, "LCD registerSelect");

        return new GpioTextDriver(data0, data1, data2, data3, data4, data5, data6, data7, enable, registerSelect);
    }

    private static IRunnerUserInput GetGpioInput() {
        IDigitalInterrupt start = Signals.DigitalInterrupt.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D0,
                                                                "Start Tests",
                                                                ResistorMode.PullUp,
                                                                InterruptMode.InterruptEdgeLow,
                                                                50);
        IDigitalInterrupt up = Signals.DigitalInterrupt.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D1,
                                                             "Start Tests",
                                                             ResistorMode.PullUp,
                                                             InterruptMode.InterruptEdgeLow,
                                                             50);
        IDigitalInterrupt down = Signals.DigitalInterrupt.Bind((Pin)SecretLabsPinAssignment.GPIO_PIN_D2,
                                                               "Start Tests",
                                                               ResistorMode.PullUp,
                                                               InterruptMode.InterruptEdgeLow,
                                                               50);
        return new UserInput(start, up, down);
    }

    private static IDriveTextDisplays GetMcp4Driver() {
        II2CBus bus = new SpotI2CBus();
        MCP23017 mcp = new MCP23017(bus, 0x021);
        DisplayPins displayPins = new DisplayPins((Pin)25, (Pin)26, (Pin)27, (Pin)28, (Pin)1, (Pin)3, Pin.None, (Pin)2);

        return new Mcp23017TextDriver(mcp, displayPins);
    }

    private static IDriveTextDisplays GetMcp8Driver() {
        II2CBus bus = new SpotI2CBus();
        MCP23017 mcp = new MCP23017(bus, 0x021);
        DisplayPins displayPins = new DisplayPins((Pin)21, (Pin)22, (Pin)23, (Pin)24, (Pin)25, (Pin)26, (Pin)27, (Pin)28, (Pin)1, (Pin)3, Pin.None, (Pin)2);

        return new Mcp23017TextDriver(mcp, displayPins);
    }

    private class UserInput : IRunnerUserInput
    {
        public UserInput(IDigitalInterrupt startTests)
            : this(startTests, null, null) { }

        public UserInput(IDigitalInterrupt startTests, IDigitalInterrupt scrollUp, IDigitalInterrupt scrollDown) {
            StartTests = startTests;
            ScrollUp = scrollUp;
            ScrollDown = scrollDown;
        }

        public IDigitalInterrupt ScrollDown { get; private set; }

        public IDigitalInterrupt ScrollUp { get; private set; }

        public IDigitalInterrupt StartTests { get; private set; }
    }
}