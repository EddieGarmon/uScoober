using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware
{
    public enum InterruptMode
    {
        InterruptNone = Port.InterruptMode.InterruptNone,
        InterruptEdgeLow = Port.InterruptMode.InterruptEdgeLow,
        InterruptEdgeHigh = Port.InterruptMode.InterruptEdgeHigh,
        InterruptEdgeBoth = Port.InterruptMode.InterruptEdgeBoth,
        InterruptEdgeLevelHigh = Port.InterruptMode.InterruptEdgeLevelHigh,
        InterruptEdgeLevelLow = Port.InterruptMode.InterruptEdgeLevelLow,
    }
}