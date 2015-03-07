using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware
{
    public enum ResistorMode : ushort
    {
        Disabled = Port.ResistorMode.Disabled,
        External = Port.ResistorMode.Disabled,
        PullUp = Port.ResistorMode.PullUp,
        PullDown = Port.ResistorMode.PullDown,
    }
}