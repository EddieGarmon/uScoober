using System;
using System.Diagnostics;
using System.Text;
using uScoober.DataStructures;
using uScoober.DataStructures.Typed;

namespace uScoober.Storage
{
    /// <summary>
    ///     Class StoragePath:
    ///     directories will always end with a Platform.Environment.PathSeperator and files will not.
    /// </summary>
    [DebuggerDisplay("{GetDebugInfo()}")]
    public abstract class BasePath : IPath
    {
        private readonly bool _isFolder;
        private readonly StringList _parts;
        private readonly PathType _pathType;
        private string _toString;

        protected BasePath(PathType pathType, bool isFolder, string path) {
            if (path == null) {
                throw new ArgumentNullException("path");
            }
            if (!isFolder) {
                char lastChar = path[path.Length - 1];
                if (lastChar == '\\' || lastChar == '/') {
                    throw new Exception("No filename specified: " + path);
                }
            }
            _isFolder = isFolder;

            NameHelper.EnsureValidPathCharacters(path);

            RootMatch rootMatch = RootMatch.FindRoot(path);
            if (rootMatch == null) {
                throw new Exception("Unsupported path: " + path);
            }
            if (pathType.IsRelative()) {
                if (!rootMatch.PathType.IsRelative()) {
                    throw PathExceptions.NotARelativePath(path);
                }
            }
            else {
                if (rootMatch.PathType.IsRelative()) {
                    throw PathExceptions.NotAnAbsolutePath(path);
                }
            }
            _pathType = rootMatch.PathType;
            _parts = rootMatch.Parts;
            Segment(rootMatch.ItemPath);
            CleanUpRoute();
        }

        protected internal BasePath(PathType pathType, bool isFolder, StringList parts) {
            _pathType = pathType;
            _isFolder = isFolder;
            _parts = parts;
            CleanUpRoute();
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is absolute path.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is absolute path; otherwise, <c>false</c>.
        /// </value>
        public bool IsAbsolutePath {
            get { return (_pathType & PathType.Absolute) == PathType.Absolute; }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is file path.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is file path; otherwise, <c>false</c>.
        /// </value>
        public bool IsFilePath {
            get { return !_isFolder; }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is folder path.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is folder path; otherwise, <c>false</c>.
        /// </value>
        public bool IsFolderPath {
            get { return _isFolder; }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is relative path.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is relative path; otherwise, <c>false</c>.
        /// </value>
        public bool IsRelativePath {
            get { return (_pathType | PathType.Relative) == PathType.Relative; }
        }

        protected PathType PathType {
            get { return _pathType; }
        }

        protected string RootValue {
            get { return _parts.First; }
        }

        bool IPath.HasParent {
            get { throw new Exception("Must be overridden in inherited class."); }
        }

        /// <summary>
        ///     Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        IFolderPath IPath.Parent {
            get { throw new Exception("Must be overridden in inherited class."); }
        }

        protected internal string ItemName {
            get { return _parts.Last; }
        }

        protected internal StringList Parts {
            get { return _parts; }
        }

        /// <summary>
        ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            return Equals(obj as BasePath);
        }

        /// <summary>
        ///     Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>
        ///     <c>true</c> if XXXX, <c>false</c> otherwise
        /// </returns>
        public bool Equals(BasePath other) {
            if (other == null || _pathType != other._pathType || _isFolder != other._isFolder) {
                return false;
            }
            var myItems = _parts.GetEnumerator();
            var otherItems = other._parts.GetEnumerator();
            while (true) {
                if (!myItems.MoveNext()) {
                    return !otherItems.MoveNext();
                }
                if (otherItems.MoveNext()) {
                    if (myItems.Current == otherItems.Current) {
                        continue;
                    }
                }
                return false;
            }
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode() {
            return ToString()
                .GetHashCode();
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() {
            return _toString ?? (_toString = BuildString());
        }

        private string BuildString() {
            if (_parts.Count == 1) {
                return _parts.First;
            }

            var builder = new StringBuilder();
            var enumerator = _parts.GetEnumerator();
            // add root
            enumerator.MoveNext();
            builder.Append(enumerator.Current);
            // add first path component
            if (enumerator.MoveNext()) {
                builder.Append(enumerator.Current);
            }
            // prepend seperator for all remaining segments
            while (enumerator.MoveNext()) {
                builder.Append(NameHelper.PathSeperator);
                builder.Append(enumerator.Current);
            }
            // add directory marker if required
            if (IsFolderPath) {
                builder.Append(NameHelper.PathSeperator);
            }
            return builder.ToString();
        }

        private void CleanUpRoute() {
            NameHelper.EnsureValidFilenameCharacters(_parts);

            // NB: process all inner special directories of the route segments
            // ""	-> remove empty segments
            // "."	-> reference to current folder, remove unless it is the first segment of a relative path
            // ".."	-> reference parent folder, remove this and parent folder, as long as the parent is not first segment or special too

            var ring = _parts.ToRing();

            var link = ring.Head.Next; // first item path segment
            while (link != ring.Head) {
                Ring.Link previous = link.Previous;
                Ring.Link next = link.Next;
                switch ((string)link.Value) {
                    case "":
                        ring.RemoveLink(link);
                        link = next;
                        break;

                    case ".":
                        if (IsRelativePath && previous == ring.Head) {
                            link = next;
                            break;
                        }
                        ring.RemoveLink(link);
                        link = next;
                        break;

                    case "..":
                        if (previous == ring.Head) {
                            if (IsRelativePath) {
                                link = next;
                                break;
                            }
                            throw new Exception("Attempt to reference parent above absolute root.");
                        }
                        if ((string)previous.Value == ".") {
                            ring.RemoveLink(previous);
                            link = next;
                            break;
                        }
                        if ((string)previous.Value == "..") {
                            link = next;
                            break;
                        }
                        ring.RemoveLink(previous);
                        ring.RemoveLink(link);
                        link = next;
                        break;

                    default:
                        link = link.Next;
                        break;
                }
            }

            if (_parts.Count < 1) {
                throw new Exception("Empty path is not valid.");
            }
            if (_parts.Count == 1) {
                if (IsFolderPath && IsAbsolutePath) {
                    //a single root folder is allowed.
                    return;
                }
                throw new Exception("Empty path is not valid.");
            }

            // .\ and ..\ required for relative path part[1]
            // .\ and ..\ illegal for absolute path[1]
            var firstPart = (string)ring.Head.Next.Value;
            bool startsWithRelativeSpecialPart = RelativePath.IsRelativeSpecialPart(firstPart);
            if (IsAbsolutePath) {
                if (startsWithRelativeSpecialPart) {
                    throw new Exception("Not a valid absolute path.");
                }
            }
            else {
                if (!startsWithRelativeSpecialPart) {
                    throw new Exception("Not a valid relative path.");
                }
            }

            if ((IsFilePath) && ((IsAbsolutePath && _parts.Count == 1) || (IsRelativePath && _parts.Count == 2))) {
                throw new Exception("No file name specified.");
            }
        }

        private string GetDebugInfo() {
            return (IsAbsolutePath ? "Abs-" : "Rel-") + (IsFolderPath ? "Dir" : "File") + ": " + BuildString();
        }

        private void Segment(string path) {
            //parse parts after root has been stripped
            int nextSegmentStart = 0;
            for (int i = 0; i < path.Length; i++) {
                char @char = path[i];
                if (@char != '\\' && @char != '/') {
                    continue;
                }
                // add a segment if length is > 0;
                int length = i - nextSegmentStart;
                if (length > 0) {
                    _parts.Add(path.Substring(nextSegmentStart, length));
                }
                nextSegmentStart = i + 1;
            }
            if (nextSegmentStart < path.Length) {
                _parts.Add(path.Substring(nextSegmentStart));
            }
        }

        /// <summary>
        ///     Implements the ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(BasePath left, BasePath right) {
            if (ReferenceEquals(left, right)) {
                return true;
            }
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) {
                return false;
            }
            return left.Equals(right);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="BasePath" /> to <see cref="System.String" />.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(BasePath path) {
            return path.ToString();
        }

        /// <summary>
        ///     Implements the !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(BasePath left, BasePath right) {
            return !(left == right);
        }

        private class RootMatch
        {
            private RootMatch(PathType pathType, StringList parts, string itemPath) {
                PathType = pathType;
                Parts = parts;
                ItemPath = itemPath;
            }

            public string ItemPath { get; private set; }

            public StringList Parts { get; private set; }

            public PathType PathType { get; private set; }

            public static RootMatch FindRoot(string path) {
                // ordered list of match makers, start with relative path, then order of use
                //return null if not found
                return MatchRelativePath(path) ?? MatchSdCard(path) ?? MatchDriveLetter(path) ?? MatchRamDrive(path) ?? MatchNetworkShare(path);
            }

            private static RootMatch MatchDriveLetter(string path) {
                if (path.Length < 2) {
                    return null;
                }
                char first = path[0];
                if (('a' <= first && first <= 'z') || ('A' <= first && first <= 'Z')) {
                    if (path[1] == ':') {
                        if (path.Length == 2) {
                            return new RootMatch(PathType.DriveLetter, new StringList(path + "\\"), string.Empty);
                        }
                        if (path[2] == '\\') {
                            return path.Length == 3
                                       ? new RootMatch(PathType.DriveLetter, new StringList(path), string.Empty)
                                       : new RootMatch(PathType.DriveLetter, new StringList(path.Substring(0, 3)), path.Substring(3));
                        }
                    }
                }
                return null;
            }

            private static RootMatch MatchNetworkShare(string path) {
                // todo support: // \\server\share\<item-path>
                return null;
            }

            private static RootMatch MatchRamDrive(string path) {
                // todo support: // \ram\<item-path>
                return null;
            }

            private static RootMatch MatchRelativePath(string path) {
                switch (path.Length) {
                    case 0:
                        return null;
                    case 1:
                        if (path == ".") {
                            return new RootMatch(PathType.Relative,
                                                 new StringList {
                                                     null
                                                 },
                                                 path);
                        }
                        return null;
                    case 2:
                        if (path == @".\" || path == "..") {
                            return new RootMatch(PathType.Relative,
                                                 new StringList {
                                                     null
                                                 },
                                                 path);
                        }
                        return null;
                    default:
                        //sibling relative=>  .\
                        if (path[0] == '.' && path[1] == '\\') {
                            return new RootMatch(PathType.Relative,
                                                 new StringList {
                                                     null
                                                 },
                                                 path);
                        }
                        //parent relative=>  ..\
                        if (path[0] == '.' && path[1] == '.' && path[2] == '\\') {
                            return new RootMatch(PathType.Relative,
                                                 new StringList {
                                                     null
                                                 },
                                                 path);
                        }
                        return null;
                }
            }

            private static RootMatch MatchSdCard(string path) {
                // support: // \SD\<item-path>
                if (path.Length < 4) {
                    return null;
                }
                if (path[0] == '\\' && (path[1] == 's' || path[1] == 'S') && (path[2] == 'd' || path[2] == 'D') && path[3] == '\\') {
                    return new RootMatch(PathType.SdDrive, new StringList(@"\SD\"), path.Length == 4 ? string.Empty : path.Substring(4));
                }
                return null;
            }
        }
    }
}