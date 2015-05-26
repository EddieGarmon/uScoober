using System;
using System.Text;
using uScoober.Hardware.Display;

namespace uScoober.Hardware.Text
{
    public class CharacterDisplay : IDisplayText
    {
        public CharacterDisplay(int width, int height) { }

        public bool BlinkCursor { get; set; }

        public int Columns { get; private set; }

        public Encoding Encoding { get; set; }

        public bool IsBacklightEnabled { get; set; }

        public bool IsEnabled { get; set; }

        public int Rows { get; private set; }

        public bool ShowCursor { get; set; }

        public void Clear() {
            throw new NotImplementedException();
        }

        public void SendCommand(byte data) {
            throw new NotImplementedException();
        }

        public void SetCursorLocation(int row, int column) {
            throw new NotImplementedException();
        }

        public void Write(string text) {
            throw new NotImplementedException();
        }

        public void Write(char value) {
            throw new NotImplementedException();
        }

        public void Write(byte[] buffer, int startOffset, int count) {
            throw new NotImplementedException();
        }

        public void Write(byte value) {
            throw new NotImplementedException();
        }
    }
}