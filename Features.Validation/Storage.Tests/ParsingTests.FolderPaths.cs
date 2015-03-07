using System.Collections;
using Microsoft.SPOT;
using uScoober.TestFramework;
using uScoober.TestFramework.Assert;

namespace uScoober.Storage
{
    public partial class ParsingTests
    {
        public IEnumerable AbsoluteFolder_Data() {
            return new[] {
                new FolderArgs(@"C:", @"C:\"),
                new FolderArgs(@"C:\", @"C:\"),
                new FolderArgs(@"C:\.", @"C:\"),
                new FolderArgs(@"C:\.\", @"C:\"),
                new FolderArgs(@"C:\Hello", @"C:\Hello\"),
                new FolderArgs(@"C:\Hello\", @"C:\Hello\"),
                new FolderArgs(@"C:\Hello\.", @"C:\Hello\"),
                new FolderArgs(@"C:\Hello\.\", @"C:\Hello\"),
                new FolderArgs(@"C:\Hello\World", @"C:\Hello\World\"),
                new FolderArgs(@"C:\Hello\World\", @"C:\Hello\World\"),
                new FolderArgs(@"C:\Hello\\World\", @"C:\Hello\World\"),
                new FolderArgs(@"C:\Hello\.\World\", @"C:\Hello\World\"),
                new FolderArgs(@"C:\Hello\..\World\", @"C:\World\"),
                new FolderArgs(@"C:\Hello\..\World\..", @"C:\"),
                new FolderArgs(@"C:\Hello\World\..\..", @"C:\"),
                new FolderArgs(@"C:\Windows\oops\..\System\", @"C:\Windows\System\"),
                new FolderArgs(@"", null),
                new FolderArgs(@".\", null),
                new FolderArgs(@"..\", null),
                new FolderArgs(@"c:\..\", null),
                new FolderArgs(@"c:\foo\..\..\", null),
                new FolderArgs(@"c:\foo\..\.\..\", null),
                new FolderArgs(@"c:\foo\|\", null),
                new FolderArgs(@"c:\foo\:\", null),
                new FolderArgs(@"osi:\foo\", null)
            };
        }

        public void AbsoluteFolder_Theory(FolderArgs args) {
            if (args.Expected == null) {
                Trap.Exception(() => new AbsoluteFolderPath(args.Source))
                    .ShouldNotBeNull();
            }
            else {
                var folder = new AbsoluteFolderPath(args.Source);
                Debug.Print(folder.ToString());
                folder.ToString()
                      .ShouldEqual(args.Expected);
            }
        }

        public IEnumerable RelativeFolder_Data() {
            return new[] {
                new FolderArgs(@".", @".\"),
                new FolderArgs(@".\", @".\"),
                new FolderArgs(@".\.", @".\"),
                new FolderArgs(@".\..", @"..\"),
                new FolderArgs(@".\Hello\..\..\", @"..\"),
                new FolderArgs(@".\..\..\", @"..\..\"),
                new FolderArgs(@".\Hello\World", @".\Hello\World\"),
                new FolderArgs(@".\Hello\World\", @".\Hello\World\"),
                new FolderArgs(@"..", @"..\"),
                new FolderArgs(@"..\", @"..\"),
                new FolderArgs(@"..\.", @"..\"),
                new FolderArgs(@"..\Hello\World", @"..\Hello\World\"),
                new FolderArgs(@"..\Hello\World\", @"..\Hello\World\"),
                new FolderArgs(@"", null),
                new FolderArgs(@"c:\", null),
                new FolderArgs(@"\\unc\path", null),
                new FolderArgs(@"http://path/", null),
                new FolderArgs(@"hello\world\", null)
            };
        }

        public void RelativeFolder_Theory(FolderArgs args) {
            if (args.Expected == null) {
                Trap.Exception(() => new RelativeFolderPath(args.Source))
                    .ShouldNotBeNull();
            }
            else {
                var folder = new RelativeFolderPath(args.Source);
                folder.ToString()
                      .ShouldEqual(args.Expected);
            }
        }

        public class FolderArgs
        {
            public FolderArgs(string source, string expected) {
                Source = source;
                Expected = expected;
            }

            public string Expected { get; private set; }

            public string Source { get; private set; }

            public override string ToString() {
                return new PrettyArgs().Add("Source", Source)
                                       .Add("Expected", Expected)
                                       .ToString();
            }
        }
    }
}