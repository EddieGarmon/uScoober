using System;
using System.IO;

namespace uScoober.Storage.Spot
{
    internal class SpotFolder : IFolder
    {
        private readonly AbsoluteFolderPath _path;

        public SpotFolder(AbsoluteFolderPath path) {
            _path = path;
        }

        public string Name {
            get { return _path.ItemName; }
        }

        public AbsoluteFolderPath Path {
            get { return _path; }
        }

        public IFile CreateFile(string name, CollisionStrategy collisionStrategy = CollisionStrategy.FailIfExists) {
            throw new NotImplementedException("SpotFolder.CreateFile");
        }

        public IFolder CreateFolder(string name) {
            var childFolderPath = _path.GetChildFolderPath(name);
            Directory.CreateDirectory(childFolderPath);
            return new SpotFolder(childFolderPath);
        }

        public void Delete() {
            throw new NotImplementedException("SpotFolder.Delete");
        }

        public bool Exists() {
            return Directory.Exists(_path);
        }

        public IFile GetFile(string name) {
            throw new NotImplementedException("SpotFolder.GetFile");
        }

        public FileCollection GetFiles() {
            Directory.EnumerateFiles(_path);
            throw new NotImplementedException("SpotFolder.GetFiles");
        }

        public IFolder GetFolder(string name) {
            throw new NotImplementedException("SpotFolder.GetFolder");
        }

        public FolderCollection GetFolders() {
            throw new NotImplementedException("SpotFolder.GetFolders");
        }
    }
}