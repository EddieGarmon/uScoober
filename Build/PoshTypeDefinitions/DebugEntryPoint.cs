namespace PoshTypeDefinitions
{
    public class DebugEntryPoint
    {
        public static void Main() {
            // This project exists to validate the c# that psake uses for version management
            SemVer semanticVersion = null;
            semanticVersion = SemVer.Parse("1.2.3");
            semanticVersion = SemVer.Parse("1.2.3-pre");
            semanticVersion = SemVer.Parse("1.2.3+meta");
            semanticVersion = SemVer.Parse("1.2.3-pre+meta");
        }
    }
}