using uScoober.Hardware;
using uScoober.Hardware.Display;
using SL = SecretLabs.NETMF.Hardware.Netduino;

namespace TestFramework.TextValidator
{
    public class TestLcdUsingGpioProvider
    {
        public static void WriteToScreen(BitMode bitmode) {
            //IDigitalOutput data0 = Signals.DigitalOutput.Bind((Pin)SL.Pins.GPIO_PIN_D6, "LCD data0");
            //IDigitalOutput data1 = Signals.DigitalOutput.Bind((Pin)SL.Pins.GPIO_PIN_D7, "LCD data1");
            //IDigitalOutput data2 = Signals.DigitalOutput.Bind((Pin)SL.Pins.GPIO_PIN_D8, "LCD data2");
            //IDigitalOutput data3 = Signals.DigitalOutput.Bind((Pin)SL.Pins.GPIO_PIN_D9, "LCD data3");
            IDigitalOutput data4 = Signals.DigitalOutput.Bind((Pin)SL.Pins.GPIO_PIN_D10, "LCD data4");
            IDigitalOutput data5 = Signals.DigitalOutput.Bind((Pin)SL.Pins.GPIO_PIN_D11, "LCD data5");
            IDigitalOutput data6 = Signals.DigitalOutput.Bind((Pin)SL.Pins.GPIO_PIN_D12, "LCD data6");
            IDigitalOutput data7 = Signals.DigitalOutput.Bind((Pin)SL.Pins.GPIO_PIN_D13, "LCD data7");

            IDigitalOutput enable = Signals.DigitalOutput.Bind((Pin)SL.Pins.GPIO_PIN_D4, "LCD enable");
            IDigitalOutput readWrite = Signals.DigitalOutput.Bind((Pin)SL.Pins.GPIO_PIN_D3, "LCD readWrite");
            IDigitalOutput registerSelect = Signals.DigitalOutput.Bind((Pin)SL.Pins.GPIO_PIN_D2, "LCD registerSelect");
            IDigitalOutput backlightEnable = null;

            IDriveTextDisplays driver = new GpioTextDriver(
            //data0,
            //                                               data1,
            //                                               data2,
            //                                               data3,
                                                           data4,
                                                           data5,
                                                           data6,
                                                           data7,
                                                           enable,
                                                           registerSelect,
                                                           backlightEnable,
                                                           readWrite);

            CharacterDisplay lcd = new CharacterDisplay(20, 4, driver);

            lcd.Write("Hello Eddie");
            //lcd.SetCursorLocation(2,1);
            lcd.Write("abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ987654321");
        }
    }
}