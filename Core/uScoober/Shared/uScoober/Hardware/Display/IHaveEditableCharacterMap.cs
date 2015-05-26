namespace uScoober.Hardware.Display
{
    public interface IHaveEditableCharacterMap
    {
        void CreateChar(int location, byte[] charmap);
    }
}