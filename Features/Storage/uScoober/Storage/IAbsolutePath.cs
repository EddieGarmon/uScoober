namespace uScoober.Storage
{
    public interface IAbsolutePath : IPath
    {
        new AbsoluteFolderPath Parent { get; }

        PathType RootType { get; }

        string RootValue { get; }
    }
}