using System;
using System.IO;

namespace uScoober.TestFramework.Core
{
    public class BuildAutomation
    {
        public const string MarkerFilename = @"\WINFS\BuildTesting.txt";

        public static bool InBuild {
            get {
                try {
                    return File.Exists(MarkerFilename);
                }
                catch (Exception) {
                    return false;
                }
            }
        }
    }
}