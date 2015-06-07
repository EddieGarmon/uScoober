using System.Text;
using System.Threading;

namespace uScoober.Hardware.Display
{
    public class CharacterDisplay : IDisplayText

    {
        private static readonly byte[] RowStartAddress = {
            0x00,
            0x40,
            0x14,
            0x54
        };
        private readonly IDriveTextDisplays _driver;
        private bool _isCursorBlinking = true;
        private bool _isCursorUnderlined = true;
        private bool _isEnabled = true;

        public CharacterDisplay(int columns, int rows, IDriveTextDisplays driver) {
            Columns = columns;
            Rows = rows;

            _driver = driver;
            _driver.Initialize(rows >= 2);
            //NB: we should be initialized and in the correct bit transfer mode

            _driver.SetCommand(Commands.EntryMode.Identifier | Commands.EntryMode.RightToLeft);
            _driver.Send();

            SendDisplaySettings();

            ClearScreen();
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

        public bool IsCursorUnderlined {
            get { return _isCursorUnderlined; }
            set {
                _isCursorUnderlined = value;
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

        public void ClearRow(int row) {
            SetCursorLocation(row, 0);
            _driver.SetData((byte)' ');
            for (int i = 0; i < Columns; i++) {
                _driver.Send();
            }
        }

        public void ClearScreen() {
            _driver.SetCommand(Commands.Clear);
            _driver.Send();
            Thread.Sleep(2); // this command takes a long time!
        }

        public void Home() {
            _driver.SetCommand(Commands.Home);
            _driver.Send();
            Thread.Sleep(2); // this command takes a long time!
        }

        public void SetCursorLocation(int row, int column) {
            int address = RowStartAddress[row] + column;
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

        public void WriteRow(int row, string message) {
            int fill = Columns - message.Length;
            if (fill < 0) {
                message = message.Substring(0, Columns);
            }
            SetCursorLocation(row, 0);
            Write(message);
            if (fill <= 0) {
                return;
            }
            _driver.SetData((byte)' ');
            for (int i = 0; i < fill; i++) {
                _driver.Send();
            }
        }

        private void SendDisplaySettings() {
            byte command = Commands.Display.Identifier;
            if (_isEnabled) {
                command |= Commands.Display.On;
            }
            if (_isCursorUnderlined) {
                command |= Commands.Display.CursorVisible;
            }
            if (_isCursorBlinking) {
                command |= Commands.Display.CursorBlink;
            }
            _driver.SetCommand(command);
            _driver.Send();
        }

        internal static class Commands
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