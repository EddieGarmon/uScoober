using System;

namespace uScoober.Storage
{
    internal static class PathExceptions
    {
        public static Exception NotARelativePath(string path) {
            return new Exception("The specified path is not a relative path: " + path);
        }

        public static Exception NotAnAbsolutePath(string path) {
            return new Exception("The specified path is not an absolute path: " + path);
        }

        public static Exception UndefinedSiblingFor(string path) {
            return new Exception("A sibling of '" + path + "' is undefined");
        }
    }
}