using System.Collections;
using uScoober.TestFramework;
using uScoober.TestFramework.Assert;

namespace uScoober.Storage
{
    public partial class Operators
    {
        public class AbsoluteFileTests { }

        public class AbsoluteFolderTests
        {
            public IEnumerable AddRelativeFilePath_Data() {
                return new[] {
                    new TheoryArgs(@"c:\", @".\relative", @"c:\relative"),
                    new TheoryArgs(@"c:\Hello\World", @".\..\Moto", @"c:\Hello\Moto"),
                    new TheoryArgs(@"c:\", @"..\relative", null),
                    new TheoryArgs(@"c:\Hello\World", @".\..\..\..\Moto", null)
                };
            }

            public void AddRelativeFilePath_Theory(TheoryArgs args) {
                AbsoluteFolderPath absoulte = args.LeftPath;
                RelativeFilePath relative = args.RightPath;
                if (args.ExpectedPath == null) {
                    Trap.Exception(() => { var _ = absoulte + relative; })
                        .ShouldNotBeNull();
                }
                else {
                    var combined = absoulte + relative;
                    combined.ToString()
                            .ShouldEqual(args.ExpectedPath);
                }
            }

            public IEnumerable AddRelativeFolderPath_Data() {
                return new[] {
                    new TheoryArgs(@"c:\", @".\relative", @"c:\relative\"),
                    new TheoryArgs(@"c:\Hello\World", @".\..\Moto", @"c:\Hello\Moto\"),
                    new TheoryArgs(@"c:\", @"..\relative", null),
                    new TheoryArgs(@"c:\Hello\World", @".\..\..\..\Moto", null)
                };
            }

            public void AddRelativeFolderPath_Theory(TheoryArgs args) {
                AbsoluteFolderPath absoulte = args.LeftPath;
                RelativeFolderPath relative = args.RightPath;
                if (args.ExpectedPath == null) {
                    Trap.Exception(() => { var _ = absoulte + relative; })
                        .ShouldNotBeNull();
                }
                else {
                    var combined = absoulte + relative;
                    combined.ToString()
                            .ShouldEqual(args.ExpectedPath);
                }
            }

            public IEnumerable SubtractAbsoluteFile_Data() {
                return new[] {
                    new TheoryArgs(@"c:\hello", @"d:\world\filename.ext", null),
                    new TheoryArgs(@"c:\hello", @"c:\world\filename.ext", @"..\hello\")
                };
            }

            public void SubtractAbsoluteFile_Theory(TheoryArgs args) {
                AbsoluteFolderPath finish = args.LeftPath;
                AbsoluteFilePath start = args.RightPath;
                if (args.ExpectedPath == null) {
                    Trap.Exception(() => { var _ = finish - start; })
                        .ShouldNotBeNull();
                }
                else {
                    RelativeFolderPath relative = finish - start;
                    relative.ToString()
                            .ShouldEqual(args.ExpectedPath);
                }
            }

            public IEnumerable SubtractAbsoluteFolder_Data() {
                return new[] {
                    new TheoryArgs(@"c:\hello", @"d:\world", null),
                    new TheoryArgs(@"c:\hello\world", @"c:\hello\moto", @"..\world\"),
                    new TheoryArgs(@"c:\hello\world", @"c:\hello", @".\world\"),
                    new TheoryArgs(@"c:\hello\world", @"c:\hello\world", @".\"),
                    new TheoryArgs(@"c:\hello", @"c:\hello\world", @"..\")
                };
            }

            public void SubtractAbsoluteFolder_Theory(TheoryArgs args) {
                AbsoluteFolderPath finish = args.LeftPath;
                AbsoluteFolderPath start = args.RightPath;
                if (args.ExpectedPath == null) {
                    Trap.Exception(() => { var _ = finish - start; })
                        .ShouldNotBeNull();
                }
                else {
                    RelativeFolderPath relative = finish - start;
                    relative.ToString()
                            .ShouldEqual(args.ExpectedPath);
                }
            }
        }
    }
}