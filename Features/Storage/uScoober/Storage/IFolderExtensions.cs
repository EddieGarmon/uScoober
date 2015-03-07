using System.Text;

namespace uScoober.Storage
{
    public static class IFolderExtensions
    {
        public static IFile CreateFile(this IFolder folder,
                                       AbsoluteFilePath path,
                                       byte[] contents,
                                       CollisionStrategy collisionStrategy = CollisionStrategy.FailIfExists) {
            var file = folder.CreateFile(path, collisionStrategy);
            using (var writer = file.OpenToWrite()) {
                writer.Write(contents, 0, contents.Length);
            }
            return file;
        }

        public static IFile CreateFile(this IFolder folder,
                                       AbsoluteFilePath path,
                                       string utf8Contents,
                                       CollisionStrategy collisionStrategy = CollisionStrategy.FailIfExists) {
            return CreateFile(folder, path, Encoding.UTF8.GetBytes(utf8Contents), collisionStrategy);
        }

        public static IFile GetOrCreateFile(this IFolder folder, string name) {
            return folder.GetFile(name) ?? folder.CreateFile(name);
        }
    }
}