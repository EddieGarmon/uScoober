namespace uScoober.Extensions
{
    public static class FloatExtensions
    {
        public const float DefaultCloseMargin = 0.0001f;

        public static unsafe int GetHashFast(this float value) {
            return *(int*)&value;
        }

        public static bool IsCloseTo(this float value, float other, float absoluteTolerance = DefaultCloseMargin) {
            float delta = value - other;
            if (delta < 0) {
                delta = -delta;
            }
            return delta < absoluteTolerance;
        }
    }
}