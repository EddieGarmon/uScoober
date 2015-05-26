namespace uScoober.Hardware.Text
{
    public interface ICharacterDisplay { }

    public class CharacterDisplay : ICharacterDisplay
    {
        public CharacterDisplay(int width, int height) { }

        public bool IsBacklightEnabled { get; set; }

        public bool IsEnabled { get; set; }

        public bool ShowCursor { get; set; }

        public void Clear() { }

        public void SetCursorLocation(int row, int column) { }

        public void Write(string value) { }

        private void SendCommand() { }
    }
}