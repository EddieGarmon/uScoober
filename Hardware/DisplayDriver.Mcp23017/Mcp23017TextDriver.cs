using System;
using System.Collections.Generic;
using System.Text;

namespace uScoober.Hardware.Display
{
    public class Mcp23017TextDriver : HD44780CompatibleTextDriver.EightBitData.IndependentControls
    {
        public Mcp23017TextDriver(DisplayPins displayPins, IDigitalOutput enable, IDigitalOutput isDataMode, IDigitalOutput isBackLightOn)
            : base(displayPins, enable, isDataMode, isBackLightOn) { }

        public override void Commit() {
            //align data to pins

            //Set enable to high

            //write data

            //Set enabled low to send to LCD

            throw new NotImplementedException();
        }
    }
}
