using System;
using uScoober.Hardware.IO;

namespace uScoober.Hardware.Display
{
    internal class Mcp23017TextDriver : RegisterBasedCharacterDisplayDriver
    {
        public Mcp23017TextDriver(MCP23017 mcp, DisplayPins displayPins)
            : base(displayPins) {
            //pull out individual pins to control
            IDigitalOutput enable = mcp.Output.Bind(displayPins.Enable);
            IDigitalOutput backlight = mcp.Output.Bind(displayPins.BackLight);
        }

        protected override void SendInEightBitMode() {
            //align data to pins

            //Set enable to high

            //write data

            //Set enabled low to send to LCD

            throw new NotImplementedException();
        }

        protected override void SendInFourBitMode() {
            throw new NotImplementedException();
        }
    }
}