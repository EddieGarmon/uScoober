namespace uScoober.Hardware.Memory
{
    public interface IHaveAddressedMemory
    {
        int TotalBytesAvailable { get; }

        byte ReadMemory(ushort address);

        void ReadMemory(ushort startAddress, byte[] buffer, int bufferIndex = 0, int length = -1);

        void WriteMemory(ushort address, byte value);

        void WriteMemory(ushort startAddress, byte[] buffer, int bufferStartIndex = 0, int length = -1);
    }
}