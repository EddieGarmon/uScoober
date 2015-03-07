namespace uScoober.Storage
{
    public static class PathTypeExtensions
    {
        public static bool IsRelative(this PathType pathType) {
            return (pathType & PathType.Relative) == PathType.Relative;
        }
    }
}