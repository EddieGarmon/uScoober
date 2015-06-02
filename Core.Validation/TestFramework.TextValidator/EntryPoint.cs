using System.Reflection;
using uScoober.Hardware;
using uScoober.Hardware.Display;
using uScoober.Hardware.I2C;
using uScoober.Hardware.IO;
using uScoober.Hardware.Spot;
using uScoober.TestFramework;

internal static class EntryPoint
{
    public static void Main() {
        //input buttons on mcp
        //lcd char display output using mcp transfer

        //todo: include a schematic
        //todo: create a class that wraps up the schematic

        II2CBus bus = new SpotI2CBus();

        MCP23017 expander = new MCP23017(bus, 0x021);

        DisplayPins fourBitDisplayPins = new DisplayPins((Pin)1, (Pin)25, (Pin)26, (Pin)27, (Pin)28, (Pin)3, Pin.None, (Pin)2);
        DisplayPins eightBitDisplayPins = new DisplayPins((Pin)1,
                                                          (Pin)21,
                                                          (Pin)22,
                                                          (Pin)23,
                                                          (Pin)24,
                                                          (Pin)25,
                                                          (Pin)26,
                                                          (Pin)27,
                                                          (Pin)28,
                                                          (Pin)3,
                                                          Pin.None,
                                                          (Pin)2);
        DisplayPins expanderPins = eightBitDisplayPins;

        IDriveTextDisplays driver = new Mcp23017TextDriver(expanderPins,
                                                           expander.Output.Bind(expanderPins.Enable),
                                                           expander.Output.Bind(expanderPins.DataOrCommand),
                                                           expander.Output.Bind(expanderPins.BackLight));
        //IDriveTextDisplays driver = new HD44780CompatibleTextDriver.FourBitData.IndependentControls(eightBitDisplayPins, enable, isDataMode, isBackLightOn);

        IDisplayText lcd = new CharacterDisplay(20, 4, driver);

        TextTestHarness.RunTests(Assembly.GetExecutingAssembly(), lcd);
    }
}