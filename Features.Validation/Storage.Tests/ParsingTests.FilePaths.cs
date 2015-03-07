using System.Collections;
using uScoober.TestFramework;
using uScoober.TestFramework.Assert;

namespace uScoober.Storage
{
    public partial class ParsingTests
    {
        public IEnumerable AbsoluteFile_Data() {
            return new[] {
                new FileArgs(@"c:\filename", @"c:\filename", @""),
                new FileArgs(@"c:\filename.ext", @"c:\filename.ext", @"ext"),
                new FileArgs(@"c:\filename.ext.gz", @"c:\filename.ext.gz", @"gz"),
                new FileArgs(@"filename", null, null),
                new FileArgs(@"filename.ext", null, null),
                new FileArgs(@"", null, null),
                new FileArgs(@".\", null, null),
                new FileArgs(@".\filename.ext", null, null),
                new FileArgs(@"c:", null, null),
                new FileArgs(@"c:\", null, null),
                new FileArgs(@"c:\filename\", null, null),
                new FileArgs(@"c:\filename.ext\", null, null)
            };
        }

        public void AbsoluteFile_Theory(FileArgs args) {
            if (args.Expected == null) {
                Trap.Exception(() => new AbsoluteFilePath(args.Source))
                    .ShouldNotBeNull();
            }
            else {
                var file = new AbsoluteFilePath(args.Source);
                file.ToString()
                    .ShouldEqual(args.Expected);
                file.FileExtension.ShouldEqual(args.Extension);
            }
        }

        public IEnumerable RelativeFile_Data() {
            return new[] {
                new FileArgs(@".\filename", @".\filename", @""),
                new FileArgs(@".\filename.ext", @".\filename.ext", @"ext"),
                new FileArgs(@".\filename.ext.gz", @".\filename.ext.gz", @"gz"),
                new FileArgs(@"..\filename", @"..\filename", @""),
                new FileArgs(@"..\filename.ext", @"..\filename.ext", @"ext"),
                new FileArgs(@"..\filename.ext.gz", @"..\filename.ext.gz", @"gz"),
                new FileArgs(@"", null, null),
                new FileArgs(@".\", null, null),
                new FileArgs(@"c:\filename", null, null)
            };
        }

        public void RelativeFile_Theory(FileArgs args) {
            if (args.Expected == null) {
                Trap.Exception(() => new RelativeFilePath(args.Source))
                    .ShouldNotBeNull();
            }
            else {
                var file = new RelativeFilePath(args.Source);
                file.ToString()
                    .ShouldEqual(args.Expected);
                file.FileExtension.ShouldEqual(args.Extension);
            }
        }

        public class FileArgs
        {
            public FileArgs(string source, string expected, string extension) {
                Source = source;
                Expected = expected;
                Extension = extension;
            }

            public string Expected { get; private set; }

            public string Extension { get; private set; }

            public string Source { get; private set; }

            public override string ToString() {
                return new PrettyArgs().Add("Source", Source)
                                       .Add("Expected", Expected)
                                       .ToString();
            }
        }
    }
}