using System;
using System.Text;

namespace uScoober.Storage
{
    public static class IFileExtensions
    {
        public static IFile Move(this IFile file, RelativeFilePath relativePath, CollisionStrategy collisionStrategy = CollisionStrategy.FailIfExists) {
            if (relativePath == null) {
                throw new ArgumentNullException("relativePath");
            }
            AbsoluteFilePath newPath = file.Path + relativePath;
            return file.Move(newPath, collisionStrategy);
        }

        public static string ReadAllText(this IFile file, Encoding encoding = null) {
            encoding = encoding ?? Encoding.UTF8;
            using (var stream = file.OpenToRead()) {
                var buffer = new byte[512];
                var builder = new StringBuilder();
                int count;
                while ((count = stream.Read(buffer, 0, buffer.Length)) >= 0) {
                    if (count > 0) {
                        builder.Append(encoding.GetChars(buffer, 0, count));
                    }
                }
                return builder.ToString();
            }
        }

        public static void WriteAllText(this IFile file, string content, Encoding encoding = null) {
            encoding = encoding ?? Encoding.UTF8;
            byte[] bytes = encoding.GetBytes(content);

            using (var stream = file.OpenToWrite()) {
                stream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}