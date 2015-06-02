using System;
using System.Text;
using System.Threading;

namespace uScoober.Hardware.Display
{
    public class CharacterDisplay : IDisplayText

    {
        private readonly IDriveTextDisplays _driver;

        public CharacterDisplay(int columns, int rows, IDriveTextDisplays driver) {
            Columns = columns;
            Rows = rows;

            _driver = driver;

            //initialize
            _driver.SetCommand(Commands.FunctionSet.Identifier); //Initializer sequence is 3 @ 0x30; sets 8bit mode
            _driver.Commit();
            _driver.Commit();
            _driver.Commit();
            //NB: we should be initialized and in 8 bit mode

            //FunctionSet 8bit or 4bit?, 2 or 1 line, letter grid size
            _driver.SetCommand(Commands.FunctionSet.Identifier | Commands.FunctionSet.TwoLines);
            _driver.Commit();
            _driver.SetCommand(Commands.Display.Identifier | Commands.Display.On | Commands.Display.CursorUnderline); //DisplayAndCursor
            _driver.Commit();
            _driver.SetCommand(Commands.Clear.Identifier); //Clear
            _driver.Commit();
        }

        public int Columns { get; private set; }

        public Encoding Encoding { get; set; }

        public bool IsBacklightEnabled { get; set; }

        public bool IsCursorBlinking { get; set; }

        public bool IsCursorVisible { get; set; }

        public bool IsEnabled { get; set; }

        public int Rows { get; private set; }

        public void ClearAll() {
            _driver.SetCommand(Commands.Clear.Identifier);
            _driver.Commit();
            Thread.Sleep(2); // this command takes a long time!
        }

        public void ClearRow(int row) {
            throw new NotImplementedException("CharacterDisplay.ClearRow");
        }

        public void Home() {
            _driver.SetCommand(Commands.Home.Identifier);
            _driver.Commit();
            Thread.Sleep(2); // this command takes a long time!
        }

        public void SetCursorLocation(int row, int column) {
            throw new NotImplementedException();
            //LCD_SETDDRAMADDR
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
            _driver.Commit();
        }

        public static class Commands
        {
            public static class Clear
            {
                public const byte Identifier = 0x01;
            }

            public static class Display
            {
                public const byte CursorBlink = 0x01;
                public const byte CursorUnderline = 0x02;
                public const byte Identifier = 0x08;
                public const byte On = 0x04;
            }

            public static class FunctionSet
            {
                public const byte EightBitMode = 0x10;
                public const byte FiveByTenDotFormat = 0x04;
                public const byte Identifier = 0x20;
                public const byte TwoLines = 0x08;
            }

            public static class Home
            {
                public const byte Identifier = 0x02;
            }
        }
    }
}