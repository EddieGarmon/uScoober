namespace uScoober.Storage
{
    public interface IFolderPath : IPath
    {
        string FolderName { get; }

        IFilePath GetChildFilePath(string fileNameWithExtension);

        IFolderPath GetChildFolderPath(string folderName);

        IFilePath GetSiblingFilePath(string fileNameWithExtension);

        IFolderPath GetSiblingFolderPath(string folderName);
    }
}