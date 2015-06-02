namespace uScoober.Hardware.Display
{
    public class DisplayPins
    {
        public DisplayPins(Pin dataOrCommand, Pin d4, Pin d5, Pin d6, Pin d7, Pin enable, Pin backLight = Pin.None, Pin readOrWrite = Pin.None) {
            D0 = Pin.None;
            D1 = Pin.None;
            D2 = Pin.None;
            D3 = Pin.None;

            D4 = d4;
            D5 = d5;
            D6 = d6;
            D7 = d7;

            Enable = enable;
            DataOrCommand = dataOrCommand;
            BackLight = backLight;
            ReadOrWrite = readOrWrite;

            TransferMode = BitMode.Four;
        }

        public DisplayPins(Pin dataOrCommand,
                           Pin d0,
                           Pin d1,
                           Pin d2,
                           Pin d3,
                           Pin d4,
                           Pin d5,
                           Pin d6,
                           Pin d7,
                           Pin enable,
                           Pin backLight = Pin.None,
                           Pin readOrWrite = Pin.None) {
            D0 = d0;
            D1 = d1;
            D2 = d2;
            D3 = d3;

            D4 = d4;
            D5 = d5;
            D6 = d6;
            D7 = d7;

            Enable = enable;
            DataOrCommand = dataOrCommand;
            BackLight = backLight;
            ReadOrWrite = readOrWrite;

            TransferMode = BitMode.Eight;
        }

        public Pin BackLight { get; private set; }

        public Pin D0 { get; private set; }

        public Pin D1 { get; private set; }

        public Pin D2 { get; private set; }

        public Pin D3 { get; private set; }

        public Pin D4 { get; private set; }

        public Pin D5 { get; private set; }

        public Pin D6 { get; private set; }

        public Pin D7 { get; private set; }

        public Pin DataOrCommand { get; private set; }

        public Pin Enable { get; private set; }

        public Pin ReadOrWrite { get; private set; }

        public BitMode TransferMode { get; private set; }
    }
}