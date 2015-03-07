using System;
using System.IO;

namespace uScoober.Storage.Ram
{
    internal class RamFile : IFile
    {
        public string Name { get; private set; }

        public AbsoluteFilePath Path { get; private set; }

        public void Delete() {
            throw new NotImplementedException("RamFile.Delete");
        }

        public bool Exists() {
            throw new NotImplementedException("RamFile.Exists");
        }

        public DateTime GetLastWriteTime() {
            throw new NotImplementedException("RamFile.GetLastWriteTime");
        }

        public IFile Move(AbsoluteFilePath absolutePath, CollisionStrategy collisionStrategy = CollisionStrategy.FailIfExists) {
            throw new NotImplementedException("RamFile.Move");
        }

        public Stream OpenToAppend() {
            throw new NotImplementedException("RamFile.OpenToAppend");
        }

        public Stream OpenToRead() {
            throw new NotImplementedException("RamFile.OpenToRead");
        }

        public Stream OpenToWrite() {
            throw new NotImplementedException("RamFile.OpenToWrite");
        }

        public IFile Rename(string newFilenameWithExtension, CollisionStrategy collisionStrategy = CollisionStrategy.FailIfExists) {
            throw new NotImplementedException("RamFile.Rename");
        }

        public void Touch() {
            throw new NotImplementedException("RamFile.Touch");
        }
    }
}