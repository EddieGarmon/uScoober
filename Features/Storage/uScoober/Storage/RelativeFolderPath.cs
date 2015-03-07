using uScoober.DataStructures.Typed;

namespace uScoober.Storage
{
    /// <summary>
    ///     Class RelativeFolderPath
    /// </summary>
    public sealed class RelativeFolderPath : RelativePath,
                                             IFolderPath
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RelativeFolderPath" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public RelativeFolderPath(string path)
            : base(true, path) { }

        internal RelativeFolderPath(StringList parts)
            : base(true, parts) { }

        public string FolderName {
            get { return ItemName; }
        }

        public RelativeFilePath GetChildFilePath(string name, string extension) {
            return GetChildFilePath(name + "." + extension);
        }

        public RelativeFilePath GetChildFilePath(string fileName) {
            NameHelper.EnsureValidFilenameCharacters(fileName);
            var parts = Parts.Clone();
            parts.Add(fileName);
            return new RelativeFilePath(parts);
        }

        public RelativeFolderPath GetChildFolderPath(string folderName) {
            NameHelper.EnsureValidFilenameCharacters(folderName);
            var parts = Parts.Clone();
            parts.Add(folderName);
            return new RelativeFolderPath(parts);
        }

        IFilePath IFolderPath.GetChildFilePath(string fileNameWithExtension) {
            return GetChildFilePath(fileNameWithExtension);
        }

        IFolderPath IFolderPath.GetChildFolderPath(string folderName) {
            return GetChildFolderPath(folderName);
        }

        IFilePath IFolderPath.GetSiblingFilePath(string fileNameWithExtension) {
            return GetSiblingFilePath(fileNameWithExtension);
        }

        IFolderPath IFolderPath.GetSiblingFolderPath(string folderName) {
            return GetSiblingFolderPath(folderName);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.String" /> to <see cref="RelativeFolderPath" />.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator RelativeFolderPath(string path) {
            return path == null ? null : new RelativeFolderPath(path);
        }
    }
}