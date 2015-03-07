using System;
using System.IO;

namespace uScoober.Storage
{
    public interface IFile
    {
        string Name { get; }

        AbsoluteFilePath Path { get; }

        void Delete();

        bool Exists();

        DateTime GetLastWriteTime();

        IFile Move(AbsoluteFilePath absolutePath, CollisionStrategy collisionStrategy = CollisionStrategy.FailIfExists);

        Stream OpenToAppend();

        Stream OpenToRead();

        Stream OpenToWrite();

        IFile Rename(string newFilenameWithExtension, CollisionStrategy collisionStrategy = CollisionStrategy.FailIfExists);

        void Touch();
    }
}