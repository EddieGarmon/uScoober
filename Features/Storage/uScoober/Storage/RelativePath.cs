using uScoober.DataStructures.Typed;

namespace uScoober.Storage
{
    public abstract class RelativePath : BasePath,
                                         IRelativePath
    {
        private RelativeFolderPath _parent;

        protected RelativePath(bool isFolder, string path)
            : base(PathType.Relative, isFolder, path) { }

        protected internal RelativePath(bool isFolder, StringList parts)
            : base(PathType.Relative, isFolder, parts) { }

        public bool HasParent {
            get { return Parts.Count > 2 && !IsRelativeSpecialPart(Parts[Parts.Count - 2]); }
        }

        public RelativeFolderPath Parent {
            get { return HasParent ? _parent ?? (_parent = new RelativeFolderPath(Parts.CloneSublist(Parts.Count - 1))) : null; }
        }

        public RelativeFilePath GetSiblingFilePath(string name, string extension) {
            return GetSiblingFilePath(name + "." + extension);
        }

        public RelativeFilePath GetSiblingFilePath(string fileNameWithExtension) {
            NameHelper.EnsureValidFilenameCharacters(fileNameWithExtension);
            StringList parts = Parts.Clone();
            parts.Add("..");
            parts.Add(fileNameWithExtension);
            return new RelativeFilePath(parts);
        }

        public RelativeFolderPath GetSiblingFolderPath(string folderName) {
            NameHelper.EnsureValidFilenameCharacters(folderName);
            StringList parts = Parts.Clone();
            parts.Add("..");
            parts.Add(folderName);
            return new RelativeFolderPath(parts);
        }

        IFolderPath IPath.Parent {
            get { return Parent; }
        }

        internal static bool IsRelativeSpecialPart(string part) {
            return part == "." || part == "..";
        }
    }
}