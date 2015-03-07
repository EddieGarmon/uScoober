namespace uScoober.Storage
{
    public interface IFilePath : IPath
    {
        string FileExtension { get; }

        string FileName { get; }

        string FileNameWithoutExtension { get; }

        IFilePath GetSiblingFilePath(string fileNameWithExtension);

        IFolderPath GetSiblingFolderPath(string folderName);
    }
}