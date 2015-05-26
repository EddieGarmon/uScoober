namespace uScoober.Hardware.Display
{
    public interface IDriveTextDisplays
    {
        BitMode BitTransferMode { get; }

        void SendCommand();

        void Write();
    }
}