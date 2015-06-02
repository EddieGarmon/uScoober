namespace uScoober.Hardware.Display
{
    public interface IDriveTextDisplays
    {
        /// <summary>
        /// Set the Enable pin low so the LCD consumes the data
        /// </summary>
        void Commit();

        void SetCommand(byte value);

        void SetData(byte value);
    }
}