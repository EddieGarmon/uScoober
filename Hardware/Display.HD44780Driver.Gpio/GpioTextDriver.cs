namespace uScoober.Hardware.Display
{
    internal class GpioTextDriver : CharacterDisplayDriver
    {
        private readonly IDigitalOutput[] _data;
        private readonly IDigitalOutput _enable;
        private readonly IDigitalOutput _isBackLightOn;
        private readonly IDigitalOutput _isDataMode; //AKA: register select
        private readonly IDigitalOutput _readWrite;

        public GpioTextDriver(IDigitalOutput data0,
                              IDigitalOutput data1,
                              IDigitalOutput data2,
                              IDigitalOutput data3,
                              IDigitalOutput data4,
                              IDigitalOutput data5,
                              IDigitalOutput data6,
                              IDigitalOutput data7,
                              IDigitalOutput enable,
                              IDigitalOutput registerSelect,
                              IDigitalOutput isBackLightOn = null,
                              IDigitalOutput readWrite = null)
            : base(BitMode.Eight) {
            //todo: validate non null required outputs
            _data = new[] {
                data0,
                data1,
                data2,
                data3,
                data4,
                data5,
                data6,
                data7
            };
            _enable = enable;
            _isDataMode = registerSelect;
            _isBackLightOn = isBackLightOn;
            _readWrite = readWrite;
        }

        public GpioTextDriver(IDigitalOutput data4,
                              IDigitalOutput data5,
                              IDigitalOutput data6,
                              IDigitalOutput data7,
                              IDigitalOutput enable,
                              IDigitalOutput registerSelect,
                              IDigitalOutput isBackLightOn = null,
                              IDigitalOutput readWrite = null)
            : base(BitMode.Four) {
            //todo: validate non null required outputs
            _data = new[] {
                null, //data0
                null, //data1
                null, //data2
                null, //data3
                data4,
                data5,
                data6,
                data7
            };
            _enable = enable;
            _isDataMode = registerSelect;
            _isBackLightOn = isBackLightOn;
            _readWrite = readWrite;
        }

        protected override void SendInEightBitMode() {
            _isDataMode.Write(NextValueIsData);
            if (_readWrite != null) {
                _readWrite.Write(false);
            }

            _enable.Write(true);
            LatchUpperNibble();
            LatchLowerNibble(false);
            //transfer to lcd occurs on enable going low
            _enable.Write(false);
        }

        protected override void SendInFourBitMode() {
            _isDataMode.Write(NextValueIsData);
            if (_readWrite != null) {
                _readWrite.Write(false);
            }

            _enable.Write(true);
            LatchUpperNibble();
            //transfer to lcd occurs on enable going low
            _enable.Write(false);

            //set low nibble (4 bits)
            _enable.Write(true);
            LatchLowerNibble(true);
            //transfer to lcd occurs on enable going low
            _enable.Write(false);
        }

        /// <summary>
        /// set low nibble (4 bits)
        /// </summary>
        /// <param name="fourBitMode"></param>
        private void LatchLowerNibble(bool fourBitMode) {
            if (!fourBitMode && BitMode == BitMode.Four) {
                // NB: we cannot set pins that are not connected
                return;
            }

            int offset = fourBitMode ? 4 : 0;
            for (int i = 0; i < 4; i++) {
                _data[offset + i].Write(((NextValue >> i) & 0x01) == 0x01);
            }
        }

        /// <summary>
        /// set high nibble (4 bits)
        /// </summary>
        private void LatchUpperNibble() {
            for (int i = 4; i < 8; i++) {
                _data[i].Write(((NextValue >> i) & 0x01) == 0x01);
            }
        }
    }
}