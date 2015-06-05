namespace uScoober.Hardware.Display
{
    internal class GpioTextDriver : IDriveTextDisplays
    {
        private readonly BitMode _bitMode;
        private readonly IDigitalOutput[] _data;
        private readonly IDigitalOutput _enable;
        private readonly IDigitalOutput _isBackLightOn;
        private readonly IDigitalOutput _isDataMode; //AKA: register select
        private readonly IDigitalOutput _readWrite;
        private byte _nextValue;
        private bool _nextValueIsData;

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
                              IDigitalOutput readWrite = null) {
            //todo: validate non null required outputs
            _bitMode = BitMode.Eight;
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
                              IDigitalOutput readWrite = null) {
            //todo: validate non null required outputs
            _bitMode = BitMode.Four;
            _data = new[] {
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

        public BitMode BitMode {
            get { return _bitMode; }
        }

        public void Send() {
            _isDataMode.Write(_nextValueIsData);
            if (_readWrite != null) {
                _readWrite.Write(false);
            }
            _enable.Write(true);

            if (_bitMode == BitMode.Eight) {
                //set data
                for (int i = 0; i < 8; i++) {
                    // send from lsb to msb
                    _data[i].Write(((_nextValue >> i) & 0x01) == 0x01);
                }
                TransferToLcd();
            }
            else {
                //set high nibble (4 bits)
                for (int i = 0; i < 4; i++) {
                    _data[i].Write(((_nextValue >> (4 + i)) & 0x01) == 0x01);
                }
                TransferToLcd();
                //set low nibble (4 bits)
                _enable.Write(true);
                for (int i = 0; i < 4; i++) {
                    _data[i].Write(((_nextValue >> i) & 0x01) == 0x01);
                }
                TransferToLcd();
            }
        }

        public void SetCommand(byte value) {
            _nextValueIsData = false;
            _nextValue = value;
        }

        public void SetData(byte value) {
            _nextValueIsData = true;
            _nextValue = value;
        }

        private void TransferToLcd() {
            //transfer to lcd occurs on enable going low
            _enable.Write(false);
        }
    }
}