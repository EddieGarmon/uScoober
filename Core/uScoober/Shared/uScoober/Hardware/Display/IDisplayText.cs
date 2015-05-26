using System.Text;

namespace uScoober.Hardware.Display
{
    public interface IDisplayText
    {
        bool BlinkCursor { get; set; }

        int Columns { get; }

        Encoding Encoding { get; set; }

        bool IsBacklightEnabled { get; set; }

        bool IsEnabled { get; set; }

        int Rows { get; }

        bool ShowCursor { get; set; }

        void Clear();

        void SendCommand(byte data);

        void SetCursorLocation(int row, int column);

        void Write(string text);

        void Write(char value);

        void Write(byte[] buffer, int startOffset, int count);

        void Write(byte value);
    }
}