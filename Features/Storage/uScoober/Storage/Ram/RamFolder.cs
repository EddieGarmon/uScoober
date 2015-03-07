using System;

namespace uScoober.Storage.Ram
{
    internal class RamFolder : IFolder
    {
        public string Name { get; private set; }

        public AbsoluteFolderPath Path { get; private set; }

        public IFile CreateFile(string name, CollisionStrategy collisionStrategy = CollisionStrategy.FailIfExists) {
            throw new NotImplementedException("RamFolder.CreateFile");
        }

        public IFolder CreateFolder(string name) {
            throw new NotImplementedException("RamFolder.CreateFolder");
        }

        public void Delete() {
            throw new NotImplementedException("RamFolder.Delete");
        }

        public bool Exists() {
            throw new NotImplementedException("RamFolder.Exists");
        }

        public IFile GetFile(string name) {
            throw new NotImplementedException("RamFolder.GetFile");
        }

        public FileCollection GetFiles() {
            throw new NotImplementedException("RamFolder.GetFiles");
        }

        public IFolder GetFolder(string name) {
            throw new NotImplementedException("RamFolder.GetFolder");
        }

        public FolderCollection GetFolders() {
            throw new NotImplementedException("RamFolder.GetFolders");
        }
    }
}