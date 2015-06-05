using System;
using System.Text;
using System.Threading;

namespace uScoober.Hardware.Display
{
    public class CharacterDisplay : IDisplayText

    {
        private readonly IDriveTextDisplays _driver;
        private bool _isCursorBlinking = true;
        private bool _isCursorVisible = true;
        private bool _isEnabled = true;

        public CharacterDisplay(int columns, int rows, IDriveTextDisplays driver) {
            Columns = columns;
            Rows = rows;

            _driver = driver;

            SendInitializationCommands();
            //NB: we should be initialized and in 8 bit mode

            //FunctionSet 8bit or 4bit?, 2 or 1 line, letter grid size
            //todo: extract SendFunctionSetup() as deployed/configured

            byte functionSetupCommand = Commands.FunctionSetup.Identifier;
            if (_driver.BitMode == BitMode.Eight) {
                functionSetupCommand |= Commands.FunctionSetup.EightBitMode;
            }
            if (rows >= 2) {
                functionSetupCommand |= Commands.FunctionSetup.TwoLines;
            }
            _driver.SetCommand(functionSetupCommand);
            _driver.Send();

            _driver.SetCommand(Commands.EntryMode.Identifier | Commands.EntryMode.RightToLeft);
            _driver.Send();

            SendDisplaySettings();

            ClearAll();
        }

        public int Columns { get; private set; }

        public Encoding Encoding { get; set; }

        public bool IsBacklightEnabled { get; set; }

        public bool IsCursorBlinking {
            get { return _isCursorBlinking; }
            set {
                _isCursorBlinking = value;
                SendDisplaySettings();
            }
        }

        public bool IsCursorVisible {
            get { return _isCursorVisible; }
            set {
                _isCursorVisible = value;
                SendDisplaySettings();
            }
        }

        public bool IsEnabled {
            get { return _isEnabled; }
            set {
                _isEnabled = value;
                SendDisplaySettings();
            }
        }

        public int Rows { get; private set; }

        public void ClearAll() {
            _driver.SetCommand(Commands.Clear);
            _driver.Send();
            Thread.Sleep(2); // this command takes a long time!
        }

        public void ClearRow(int row) {
            throw new NotImplementedException("CharacterDisplay.ClearRow");
        }

        public void Home() {
            _driver.SetCommand(Commands.Home);
            _driver.Send();
            Thread.Sleep(2); // this command takes a long time!
        }

        public void SetCursorLocation(int row, int column) {
            int address = CalculateAddress(row, column);
            _driver.SetCommand((byte)(Commands.SetDisplayRamAddress | address));
            _driver.Send();
        }

        public void Write(string text) {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            Write(bytes, 0, bytes.Length);
        }

        public void Write(char value) {
            Write((byte)value);
        }

        public void Write(byte[] buffer, int startOffset, int count) {
            int end = startOffset + count;
            for (int i = startOffset; i < end; i++) {
                Write(buffer[i]);
            }
        }

        public void Write(byte value) {
            _driver.SetData(value);
            _driver.Send();
        }

        private int CalculateAddress(int row, int column) {
            // change column value to zero-based
            column -= 1;

            int address = (row == 2) ? column + 0x40 : column;
            throw new NotImplementedException("CharacterDisplay.CalculateAddress");
            return address;
        }

        private void SendDisplaySettings() {
            byte command = Commands.Display.Identifier;
            if (_isEnabled) {
                command |= Commands.Display.On;
            }
            if (_isCursorVisible) {
                command |= Commands.Display.CursorVisible;
            }
            if (_isCursorBlinking) {
                command |= Commands.Display.CursorBlink;
            }
            _driver.SetCommand(command);
            _driver.Send();
        }

        private void SendInitializationCommands() {
            //Initializer sequence is 3 @ 0x30;
            _driver.SetCommand(Commands.FunctionSetup.Identifier | Commands.FunctionSetup.EightBitMode);
            _driver.Send();
            _driver.Send();
            _driver.Send();
        }

        private static class Commands
        {
            public const byte Clear = 0x01;
            public const byte Home = 0x02;
            public const byte SetCharacterRamAddress = 0x40;
            public const byte SetDisplayRamAddress = 0x80;

            public static class Display
            {
                public const byte CursorBlink = 0x01;
                public const byte CursorVisible = 0x02;
                public const byte Identifier = 0x08;
                public const byte On = 0x04;
            }

            public static class EntryMode
            {
                public const byte Identifier = 0x04;
                public const byte RightToLeft = 0x02;
                public const byte ShiftDisplay = 0x01;
            }

            public static class FunctionSetup
            {
                public const byte EightBitMode = 0x10;
                public const byte Font5X10 = 0x04;
                public const byte Identifier = 0x20;
                public const byte TwoLines = 0x08;
            }

            public static class ShiftCursor
            {
                public const byte AndDisplay = 0x08;
                public const byte Identifier = 0x10;
                public const byte ToRight = 0x04;
            }
        }
    }
}