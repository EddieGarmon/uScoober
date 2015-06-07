namespace uScoober.Hardware.Display
{
    public interface IDriveTextDisplays
    {
        BitMode BitMode { get; }

        void Initialize(bool multiline);

        /// <summary>
        /// Set the Enable pin low so the LCD consumes the data
        /// </summary>
        void Send();

        void SetCommand(byte value);

        void SetData(byte value);
    }
}