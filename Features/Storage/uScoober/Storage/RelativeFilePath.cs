using uScoober.DataStructures.Typed;

namespace uScoober.Storage
{
    /// <summary>
    ///     Class RelativeFilePath
    /// </summary>
    public sealed class RelativeFilePath : RelativePath,
                                           IFilePath
    {
        private string _extension;
        private string _userGivenName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelativeFilePath" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public RelativeFilePath(string path)
            : base(false, path) { }

        internal RelativeFilePath(StringList parts)
            : base(false, parts) { }

        public string FileExtension {
            get {
                if (_extension == null) {
                    NameHelper.ExtractExtension(ItemName, out _userGivenName, out _extension);
                }
                return _extension;
            }
        }

        public string FileName {
            get { return ItemName; }
        }

        public string FileNameWithoutExtension {
            get {
                if (_userGivenName == null) {
                    NameHelper.ExtractExtension(ItemName, out _userGivenName, out _extension);
                }
                return _userGivenName;
            }
        }

        IFilePath IFilePath.GetSiblingFilePath(string fileNameWithExtension) {
            return GetSiblingFilePath(fileNameWithExtension);
        }

        IFolderPath IFilePath.GetSiblingFolderPath(string folderName) {
            return GetSiblingFolderPath(folderName);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.String" /> to <see cref="RelativeFilePath" />.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator RelativeFilePath(string path) {
            return path == null ? null : new RelativeFilePath(path);
        }
    }
}