using System.Text;

namespace uScoober.Hardware.Display
{
    public interface IDisplayText
    {
        int Columns { get; }

        Encoding Encoding { get; set; }

        bool IsBacklightEnabled { get; set; }

        bool IsCursorBlinking { get; set; }

        bool IsCursorUnderlined { get; set; }

        bool IsEnabled { get; set; }

        int Rows { get; }

        void ClearRow(int row);

        void ClearScreen();

        void Home();

        void SetCursorLocation(int row, int column);

        void Write(string text);

        void Write(char value);

        void Write(byte[] buffer, int startOffset, int count);

        void Write(byte value);

        void WriteRow(int row, string message);
    }
}