using System;
using uScoober.DataStructures.Typed;

namespace uScoober.Storage
{
    /// <summary>
    ///     Class AbsoluteFolderPath
    /// </summary>
    public sealed class AbsoluteFolderPath : AbsolutePath,
                                             IFolderPath
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AbsoluteFolderPath" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public AbsoluteFolderPath(string path)
            : base(true, path) { }

        internal AbsoluteFolderPath(StringList parts)
            : base(true, parts) { }

        public string FolderName {
            get { return ItemName; }
        }

        internal bool IsRoot {
            get { return Parts.Count == 1; }
        }

        /// <summary>
        ///     Gets the file path.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="extension">The extension.</param>
        /// <returns>AbsoluteFilePath.</returns>
        public AbsoluteFilePath GetChildFilePath(string name, string extension) {
            return GetChildFilePath(name + "." + extension);
        }

        /// <summary>
        ///     Gets the file path.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>AbsoluteFilePath.</returns>
        /// <exception cref="System.Exception">Invalid file name:  + fileName</exception>
        public AbsoluteFilePath GetChildFilePath(string fileName) {
            NameHelper.EnsureValidFilenameCharacters(fileName);
            var parts = Parts.Clone();
            parts.Add(fileName);
            return new AbsoluteFilePath(parts);
        }

        /// <summary>
        ///     Gets the child path.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns>AbsoluteFolderPath.</returns>
        /// <exception cref="System.Exception">Invalid folder name:  + folderName</exception>
        public AbsoluteFolderPath GetChildFolderPath(string folderName) {
            NameHelper.EnsureValidFilenameCharacters(folderName);
            var parts = Parts.Clone();
            parts.Add(folderName);
            return new AbsoluteFolderPath(parts);
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

        private static StringList MakeAbsolute(AbsoluteFolderPath fromHere, RelativePath adjustment) {
            var result = fromHere.Parts.CloneSublist(fromHere.Parts.Count);
            var enumerator = adjustment.Parts.GetEnumerator();
            // skip the root
            enumerator.MoveNext();
            while (enumerator.MoveNext()) {
                string part = enumerator.Current;
                switch (part) {
                    case ".":
                        break;
                    case "..":
                        // bug? can we go above the root here?
                        result.RemoveLast();
                        break;
                    default:
                        result.Add(part);
                        break;
                }
            }
            return result;
        }

        private static StringList MakeRelative(AbsoluteFolderPath fromHere, AbsoluteFolderPath toHere) {
            if (fromHere.RootValue != toHere.RootValue) {
                throw new Exception("No shared root between: " + fromHere + " -> " + toHere);
            }

            bool hasFrom;
            var @from = fromHere.Parts.GetEnumerator();
            bool hasTo;
            var @to = toHere.Parts.GetEnumerator();

            while (true) {
                hasFrom = from.MoveNext();
                hasTo = to.MoveNext();
                if (!hasFrom || !hasTo || from.Current != to.Current) {
                    break;
                }
                // eat all matches
            }

            //NB: dont forget the null absolute root
            var relative = new StringList(new string[] {
                null
            });

            while (hasFrom) {
                relative.Add("..");
                hasFrom = from.MoveNext();
            }
            //NB: ensure we are relativly rooted
            if (relative.Count == 1) {
                relative.Add(".");
            }
            while (hasTo) {
                relative.Add(to.Current);
                hasTo = to.MoveNext();
            }
            return relative;
        }

        /// <summary>
        ///     Implements the +.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="relative">The relative.</param>
        /// <returns>The result of the operator.</returns>
        public static AbsoluteFolderPath operator +(AbsoluteFolderPath root, RelativeFolderPath relative) {
            var parts = MakeAbsolute(root, relative);
            return new AbsoluteFolderPath(parts);
        }

        /// <summary>
        ///     Implements the +.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="relative">The relative.</param>
        /// <returns>The result of the operator.</returns>
        public static AbsoluteFilePath operator +(AbsoluteFolderPath root, RelativeFilePath relative) {
            var parts = MakeAbsolute(root, relative);
            return new AbsoluteFilePath(parts);
        }

        /// <summary>
        ///     Implements the -.
        /// </summary>
        /// <param name="toDir">To dir.</param>
        /// <param name="fromDir">From dir.</param>
        /// <returns>The result of the operator.</returns>
        /// <exception cref="System.Exception">No common path exists:  + fromDir.Value +  ->  + toDir.Value</exception>
        public static RelativeFolderPath operator -(AbsoluteFolderPath toDir, AbsoluteFolderPath fromDir) {
            StringList relative = MakeRelative(fromDir, toDir);
            return new RelativeFolderPath(relative);
        }

        public static RelativeFolderPath operator -(AbsoluteFolderPath toDir, AbsoluteFilePath fromFile) {
            StringList relative = MakeRelative(fromFile.Parent, toDir);
            return new RelativeFolderPath(relative);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.String" /> to <see cref="AbsoluteFolderPath" />.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator AbsoluteFolderPath(string path) {
            return path == null ? null : new AbsoluteFolderPath(path);
        }
    }
}