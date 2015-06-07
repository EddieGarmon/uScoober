using System;
using uScoober.DataStructures.Typed;

namespace uScoober.Storage
{
    internal static class NameHelper
    {
        public const char PathSeperator = '\\';
        private static readonly char[] InvalidFileNameChars = {
            '\"',
            '<',
            '>',
            '|',
            '\0',
            (char)1,
            (char)2,
            (char)3,
            (char)4,
            (char)5,
            (char)6,
            (char)7,
            (char)8,
            (char)9,
            (char)10,
            (char)11,
            (char)12,
            (char)13,
            (char)14,
            (char)15,
            (char)16,
            (char)17,
            (char)18,
            (char)19,
            (char)20,
            (char)21,
            (char)22,
            (char)23,
            (char)24,
            (char)25,
            (char)26,
            (char)27,
            (char)28,
            (char)29,
            (char)30,
            (char)31,
            ':',
            '*',
            '?',
            '\\',
            '/'
        };
        private static readonly char[] InvalidPathChars = {
            '\"',
            '<',
            '>',
            '|',
            '\0',
            (char)1,
            (char)2,
            (char)3,
            (char)4,
            (char)5,
            (char)6,
            (char)7,
            (char)8,
            (char)9,
            (char)10,
            (char)11,
            (char)12,
            (char)13,
            (char)14,
            (char)15,
            (char)16,
            (char)17,
            (char)18,
            (char)19,
            (char)20,
            (char)21,
            (char)22,
            (char)23,
            (char)24,
            (char)25,
            (char)26,
            (char)27,
            (char)28,
            (char)29,
            (char)30,
            (char)31
        };

        public static void EnsureValidFilenameCharacters(string pathSegment) {
            if (pathSegment.IndexOfAny(InvalidFileNameChars) >= 0) {
                throw new Exception("Invalid filename characters found: " + pathSegment);
            }
        }

        public static void EnsureValidPathCharacters(string path) {
            if (path.IndexOfAny(InvalidPathChars) >= 0) {
                throw new Exception("Invalid path characters found: " + path);
            }
        }

        public static void ExtractExtension(string nameWithExtension, out string userGivenPart, out string extension) {
            int lastDot = nameWithExtension.LastIndexOf('.');
            if (lastDot > 0) {
                userGivenPart = nameWithExtension.Substring(0, lastDot);
                extension = nameWithExtension.Substring(lastDot + 1);
            }
            else {
                userGivenPart = nameWithExtension;
                extension = string.Empty;
            }
        }

        internal static void EnsureValidFilenameCharacters(StringList path) {
            var enumerator = path.GetEnumerator();
            //NB: skip the 'root' part
            enumerator.MoveNext();
            while (enumerator.MoveNext()) {
                if (enumerator.Current.IndexOfAny(InvalidFileNameChars) >= 0) {
                    throw new Exception("Invalid filename characters found: " + enumerator.Current);
                }
            }
        }
    }
}