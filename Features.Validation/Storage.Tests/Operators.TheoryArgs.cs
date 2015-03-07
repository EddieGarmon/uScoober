using uScoober.TestFramework;

namespace uScoober.Storage
{
    public partial class Operators
    {
        public class TheoryArgs
        {
            public TheoryArgs(string leftPath, string rightPath, string expectedPath) {
                LeftPath = leftPath;
                RightPath = rightPath;
                ExpectedPath = expectedPath;
            }

            public string ExpectedPath { get; private set; }

            public string LeftPath { get; private set; }

            public string RightPath { get; private set; }

            public override string ToString() {
                return new PrettyArgs().Add("Left", LeftPath)
                                       .Add("Right", RightPath)
                                       .ToString();
            }
        }
    }
}