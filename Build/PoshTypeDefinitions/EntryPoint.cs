namespace PoshTypeDefinitions
{
    public class EntryPoint
    {
        public static void Main() {
            SemVer semanticVersion = null;
            semanticVersion = SemVer.Parse("1.2.3");
            semanticVersion = SemVer.Parse("1.2.3-pre");
            semanticVersion = SemVer.Parse("1.2.3+meta");
            semanticVersion = SemVer.Parse("1.2.3-pre+meta");
        }
    }
}