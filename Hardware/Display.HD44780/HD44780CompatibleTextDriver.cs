using System;
using System.Collections.Generic;
using System.Text;

namespace uScoober.Hardware.Display
{
    public abstract class HD44780CompatibleTextDriver : IDriveTextDisplays
    {
        private readonly DisplayPins _displayPins;
        protected bool _nextByteIsData; // named after the 'true' value of the RegiaterSelect bit
        protected byte _nextByteValue;

        protected HD44780CompatibleTextDriver(DisplayPins displayPins) {
            _displayPins = displayPins;
        }

        public bool BackLightEnabled { get; set; }

        public DisplayPins DisplayPins {
            get { return _displayPins; }
        }

        public abstract void Commit();

        public void SetCommand(byte value) {
            _nextByteIsData = false;
            _nextByteValue = value;
        }

        public void SetData(byte value) {
            _nextByteIsData = true;
            _nextByteValue = value;
        }

        public abstract class EightBitData : HD44780CompatibleTextDriver
        {
            private EightBitData(DisplayPins displayPins)
                : base(displayPins) { }

            /// <summary>
            /// Control pins are independent of the data
            /// </summary>
            public abstract class IndependentControls : EightBitData
            {
                private IDigitalOutput _enable;
                private IDigitalOutput _isBackLightOn;
                private IDigitalOutput _isDataMode;

                protected IndependentControls(DisplayPins displayPins, IDigitalOutput enable, IDigitalOutput isDataMode, IDigitalOutput isBackLightOn)
                    : base(displayPins) {
                    _enable = enable;
                    _isDataMode = isDataMode;
                    _isBackLightOn = isBackLightOn;
                }
            }
        }
    }
}
