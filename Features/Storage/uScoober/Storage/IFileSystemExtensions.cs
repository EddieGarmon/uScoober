using System;
using System.IO;

namespace uScoober.Storage
{
    //todo: really make all of these async
    public static class IFileSystemExtensions
    {
        public static bool CopyFile(this IFileSystem fileSystem, AbsoluteFilePath sourcePath, AbsoluteFilePath targetPath, bool overwriteIfExists = false) {
            IFile source = fileSystem.GetFile(sourcePath);
            if (source == null) {
                return false;
            }
            IFolder targetFolder = fileSystem.CreateFolder(targetPath.Folder);
            IFile file = targetFolder.GetFile(targetPath.ItemName);
            if (file != null && !overwriteIfExists) {
                return false;
            }
            if (file == null) {
                file = targetFolder.CreateFile(targetPath.ItemName);
            }
            var buffer = new byte[512];
            int count;
            using (Stream to = file.OpenToWrite()) {
                using (Stream from = file.OpenToRead()) {
                    while ((count = from.Read(buffer, 0, buffer.Length)) >= 0) {
                        if (count > 0) {
                            to.Write(buffer, 0, count);
                        }
                    }
                }
            }
            return true;
        }

        public static bool CopyFile(this IFileSystem fileSystem, AbsoluteFilePath sourcePath, RelativeFilePath targetPath, bool overwriteIfExists = false) {
            AbsoluteFilePath absoluteTargetPath = sourcePath.Folder + targetPath;
            return CopyFile(fileSystem, sourcePath, absoluteTargetPath, overwriteIfExists);
        }

        public static IFile CreateFile(this IFileSystem fileSystem,
                                       AbsoluteFilePath path,
                                       byte[] contents,
                                       CollisionStrategy collisionStrategy = CollisionStrategy.FailIfExists) {
            return fileSystem.CreateFolder(path.Folder)
                             .CreateFile(path, contents, collisionStrategy);
        }

        public static IFile CreateFile(this IFileSystem fileSystem,
                                       AbsoluteFilePath path,
                                       string utf8Contents,
                                       CollisionStrategy collisionStrategy = CollisionStrategy.FailIfExists) {
            return fileSystem.CreateFolder(path.Folder)
                             .CreateFile(path, utf8Contents, collisionStrategy);
        }

        public static void DeleteFile(this IFileSystem fileSystem, AbsoluteFilePath filePath) {
            var file = fileSystem.GetFile(filePath);
            if (file != null) {
                file.Delete();
            }
        }

        public static IFile GetOrCreateFile(this IFileSystem fileSystem, AbsoluteFilePath path) {
            return fileSystem.CreateFolder(path.Folder)
                             .GetOrCreateFile(path.ItemName);
        }

        public static bool MoveFile(this IFileSystem fileSystem, AbsoluteFilePath sourcePath, AbsoluteFilePath targetPath, bool overwriteIfExists = false) {
            throw new NotImplementedException("IFileSystemExtensions.MoveFile");
        }

        public static bool MoveFile(this IFileSystem fileSystem, AbsoluteFilePath sourcePath, RelativeFilePath targetPath, bool overwriteIfExists = false) {
            AbsoluteFilePath absoluteTargetPath = sourcePath.Folder + targetPath;
            return MoveFile(fileSystem, sourcePath, absoluteTargetPath, overwriteIfExists);
        }
    }
}