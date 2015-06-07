using System;
using uScoober.DataStructures.Typed;

namespace uScoober.Storage
{
    /// <summary>
    ///     Class AbsoluteFilePath
    /// </summary>
    public sealed class AbsoluteFilePath : AbsolutePath,
                                           IFilePath
    {
        private string _extension;
        private string _userGivenName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AbsoluteFilePath" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public AbsoluteFilePath(string path)
            : base(false, path) { }

        internal AbsoluteFilePath(StringList parts)
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

        /// <summary>
        ///     Gets the folder.
        /// </summary>
        /// <value>The folder.</value>
        public AbsoluteFolderPath Folder {
            get { return Parent; }
        }

        IFilePath IFilePath.GetSiblingFilePath(string fileNameWithExtension) {
            return GetSiblingFilePath(fileNameWithExtension);
        }

        IFolderPath IFilePath.GetSiblingFolderPath(string folderName) {
            return GetSiblingFolderPath(folderName);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.String" /> to <see cref="AbsoluteFilePath" />.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator AbsoluteFilePath(string path) {
            return path == null ? null : new AbsoluteFilePath(path);
        }

        public static RelativeFilePath operator -(AbsoluteFilePath toFile, AbsoluteFilePath fromPath) {
            throw new NotImplementedException("AbsoluteFilePath.-");
        }

        /// <summary>
        ///     Implements the -.
        /// </summary>
        /// <param name="toFile">To file.</param>
        /// <param name="fromDir">From dir.</param>
        /// <returns>The result of the operator.</returns>
        /// <exception cref="System.Exception">No common path exists:  + fromDir.Value +  ->  + toFile.Value</exception>
        public static RelativeFilePath operator -(AbsoluteFilePath toFile, AbsoluteFolderPath fromDir) {
            throw new NotImplementedException("AbsoluteFilePath.-");
            //var relativePath = MakeRelative(fromDir, toFile.Parent);
            //return new RelativeFilePath(relativePath, relativePath.Count, toFile.ItemName);
        }
    }
}