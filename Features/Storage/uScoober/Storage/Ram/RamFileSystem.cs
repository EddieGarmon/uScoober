using System;

namespace uScoober.Storage.Ram
{
    internal class RamFileSystem : IFileSystem
    {
        public IFolder AppLocalStorage { get; private set; }

        public IFolder AppRoamingStorage { get; private set; }

        public IFolder TempStorage { get; private set; }

        public IFolder CreateFolder(AbsoluteFolderPath path) {
            throw new NotImplementedException("RamFileSystem.CreateFolder");
        }

        public IFile GetFile(AbsoluteFilePath path) {
            throw new NotImplementedException("RamFileSystem.GetFile");
        }

        public IFolder GetFolder(AbsoluteFolderPath path) {
            throw new NotImplementedException("RamFileSystem.GetFolder");
        }
    }
}