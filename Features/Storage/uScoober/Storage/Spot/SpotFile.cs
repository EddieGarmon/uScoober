using System;
using System.IO;

namespace uScoober.Storage.Spot
{
    internal class SpotFile : IFile
    {
        private readonly AbsoluteFilePath _path;

        public SpotFile(AbsoluteFilePath path) {
            _path = path;
        }

        public string Name {
            get { return _path.ItemName; }
        }

        public AbsoluteFilePath Path {
            get { return _path; }
        }

        public void Delete() {
            File.Delete(_path);
        }

        public bool Exists() {
            return File.Exists(_path);
        }

        public DateTime GetLastWriteTime() {
            throw new NotImplementedException("SpotFile.GetLastWriteTime");
        }

        public IFile Move(AbsoluteFilePath absolutePath, CollisionStrategy collisionStrategy = CollisionStrategy.FailIfExists) {
            //File.Move();
            throw new NotImplementedException("SpotFile.Move");
        }

        public Stream OpenToAppend() {
            return File.Open(_path, FileMode.Append);
        }

        public Stream OpenToRead() {
            return File.Open(_path, FileMode.Open, FileAccess.Read);
        }

        public Stream OpenToWrite() {
            return File.Open(_path, FileMode.Open, FileAccess.ReadWrite);
        }

        public IFile Rename(string newFilenameWithExtension, CollisionStrategy collisionStrategy = CollisionStrategy.FailIfExists) {
            //move?
            throw new NotImplementedException("SpotFile.Rename");
        }

        public void Touch() {
            throw new NotImplementedException("SpotFile.Touch");
        }
    }
}