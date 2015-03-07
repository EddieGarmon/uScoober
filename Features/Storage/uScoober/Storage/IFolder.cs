namespace uScoober.Storage
{
    public interface IFolder
    {
        string Name { get; }

        AbsoluteFolderPath Path { get; }

        IFile CreateFile(string name, CollisionStrategy collisionStrategy = CollisionStrategy.FailIfExists);

        IFolder CreateFolder(string name);

        void Delete();

        bool Exists();

        IFile GetFile(string name);

        FileCollection GetFiles();

        IFolder GetFolder(string name);

        FolderCollection GetFolders();
    }
}