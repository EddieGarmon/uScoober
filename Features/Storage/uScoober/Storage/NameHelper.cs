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
            (Char)1,
            (Char)2,
            (Char)3,
            (Char)4,
            (Char)5,
            (Char)6,
            (Char)7,
            (Char)8,
            (Char)9,
            (Char)10,
            (Char)11,
            (Char)12,
            (Char)13,
            (Char)14,
            (Char)15,
            (Char)16,
            (Char)17,
            (Char)18,
            (Char)19,
            (Char)20,
            (Char)21,
            (Char)22,
            (Char)23,
            (Char)24,
            (Char)25,
            (Char)26,
            (Char)27,
            (Char)28,
            (Char)29,
            (Char)30,
            (Char)31,
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
            (Char)1,
            (Char)2,
            (Char)3,
            (Char)4,
            (Char)5,
            (Char)6,
            (Char)7,
            (Char)8,
            (Char)9,
            (Char)10,
            (Char)11,
            (Char)12,
            (Char)13,
            (Char)14,
            (Char)15,
            (Char)16,
            (Char)17,
            (Char)18,
            (Char)19,
            (Char)20,
            (Char)21,
            (Char)22,
            (Char)23,
            (Char)24,
            (Char)25,
            (Char)26,
            (Char)27,
            (Char)28,
            (Char)29,
            (Char)30,
            (Char)31
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