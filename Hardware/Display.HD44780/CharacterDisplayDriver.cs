using System.Threading;

namespace uScoober.Hardware.Display
{
    internal abstract class CharacterDisplayDriver : IDriveTextDisplays
    {
        private readonly BitMode _bitMode;
        private byte _nextValue;
        private bool _nextValueIsData;

        protected CharacterDisplayDriver(BitMode bitMode) {
            _bitMode = bitMode;
        }

        public BitMode BitMode {
            get { return _bitMode; }
        }

        protected byte NextValue {
            get { return _nextValue; }
        }

        protected bool NextValueIsData {
            get { return _nextValueIsData; }
        }

        public void Initialize(bool multiline) {
            // initial hardware boot time
            Thread.Sleep(50);

            //Initializer sequence is 3 @ 0x30;
            SetCommand(CharacterDisplay.Commands.FunctionSetup.Identifier | CharacterDisplay.Commands.FunctionSetup.EightBitMode);
            SendInEightBitMode();
            Thread.Sleep(5);
            SendInEightBitMode();
            Thread.Sleep(5);
            SendInEightBitMode();
            Thread.Sleep(1);

            if (_bitMode == BitMode.Four) {
                SetCommand(CharacterDisplay.Commands.FunctionSetup.Identifier);
                SendInEightBitMode();
            }

            byte setupCommand = CharacterDisplay.Commands.FunctionSetup.Identifier;
            if (_bitMode == BitMode.Eight) {
                setupCommand |= CharacterDisplay.Commands.FunctionSetup.EightBitMode;
            }
            if (multiline) {
                setupCommand |= CharacterDisplay.Commands.FunctionSetup.TwoLines;
            }
            SetCommand(setupCommand);
            Send();
        }

        public void Send() {
            if (_bitMode == BitMode.Eight) {
                SendInEightBitMode();
            }
            else {
                SendInFourBitMode();
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

        protected abstract void SendInEightBitMode();

        protected abstract void SendInFourBitMode();
    }
}