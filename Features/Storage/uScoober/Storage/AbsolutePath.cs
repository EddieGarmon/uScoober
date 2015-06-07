using uScoober.DataStructures.Typed;

namespace uScoober.Storage
{
    public abstract class AbsolutePath : BasePath,
                                         IAbsolutePath
    {
        private AbsoluteFolderPath _parent;

        protected AbsolutePath(bool isFolder, string path)
            : base(PathType.Absolute, isFolder, path) { }

        protected internal AbsolutePath(bool isFolder, StringList parts)
            : base(PathType.Absolute, isFolder, parts) { }

        public bool HasParent {
            get { return Parts.Count > 1; }
        }

        public AbsoluteFolderPath Parent {
            get { return HasParent ? _parent ?? (_parent = new AbsoluteFolderPath(Parts.CloneSublist(Parts.Count - 1))) : null; }
        }

        public PathType RootType {
            get { return PathType; }
        }

        public new string RootValue {
            get { return base.RootValue; }
        }

        IFolderPath IPath.Parent {
            get { return Parent; }
        }

        public AbsoluteFilePath GetSiblingFilePath(string name, string extension) {
            return GetSiblingFilePath(name + "." + extension);
        }

        public AbsoluteFilePath GetSiblingFilePath(string fileNameWithExtension) {
            NameHelper.EnsureValidFilenameCharacters(fileNameWithExtension);
            // cant from root, can elsewhere
            if (Parts.Count == 1) {
                throw PathExceptions.UndefinedSiblingFor(RootValue);
            }
            var parts = Parts.CloneSublist(Parts.Count - 1);
            parts.Add(fileNameWithExtension);
            return new AbsoluteFilePath(parts);
        }

        public AbsoluteFolderPath GetSiblingFolderPath(string folderName) {
            NameHelper.EnsureValidFilenameCharacters(folderName);
            // cant from root, can elsewhere
            if (Parts.Count == 1) {
                throw PathExceptions.UndefinedSiblingFor(RootValue);
            }
            var parts = Parts.CloneSublist(Parts.Count - 1);
            parts.Add(folderName);
            return new AbsoluteFolderPath(parts);
        }
    }
}