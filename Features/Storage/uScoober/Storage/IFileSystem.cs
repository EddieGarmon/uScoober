namespace uScoober.Storage
{
    public interface IFileSystem
    {
        IFolder AppLocalStorage { get; }

        IFolder AppRoamingStorage { get; }

        IFolder TempStorage { get; }

        IFolder CreateFolder(AbsoluteFolderPath path);

        IFile GetFile(AbsoluteFilePath path);

        IFolder GetFolder(AbsoluteFolderPath path);
    }
}