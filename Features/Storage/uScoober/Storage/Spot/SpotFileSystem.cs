using System.IO;

namespace uScoober.Storage.Spot
{
    public class SpotFileSystem
    {
        //NB: extend this partial class with well know folders such as:
        // public IFolder TempStorage { get; private set; }

        public IFolder CreateFolder(AbsoluteFolderPath path) {
            Directory.CreateDirectory(path);
            return new SpotFolder(path);
        }

        public IFile GetFile(AbsoluteFilePath path) {
            return File.Exists(path) ? new SpotFile(path) : null;
        }

        public IFolder GetFolder(AbsoluteFolderPath path) {
            return Directory.Exists(path) ? new SpotFolder(path) : null;
        }
    }
}