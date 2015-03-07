using System.Collections;
using uScoober.TestFramework.Assert;

namespace uScoober.Extensions
{
    public partial class StringExtensionTests
    {
        public IEnumerable Contains_Data() {
            return new[] {
                new ContainsArgs("", false, null),
                new ContainsArgs("", false, ""),
                new ContainsArgs(null, false, ""),
                new ContainsArgs(null, false, null),
                new ContainsArgs("abcd", false, null),
                new ContainsArgs("abcd", false, ""),
                new ContainsArgs("abcd", false, "e"),
                new ContainsArgs("abcd", false, "ef"),
                new ContainsArgs("abcd", false, "efg"),
                new ContainsArgs("abcd", true, "a"),
                new ContainsArgs("abcd", true, "ab"),
                new ContainsArgs("abcd", true, "abc"),
                new ContainsArgs("abcd", true, "abcd"),
                new ContainsArgs("abcd", true, "bc"),
                new ContainsArgs("abcd", true, "bcd"),
                new ContainsArgs("abcd", true, "cd"),
                new ContainsArgs("abcd", true, "d"),
                new ContainsArgs("abcd", false, "abcde")
            };
        }

        public void Contains_Theory(ContainsArgs args) {
            args.Source.Contains(args.Option)
                .ShouldEqual(args.ShouldContain);
        }

        public class ContainsArgs
        {
            public ContainsArgs(string source, bool shouldContain, string option) {
                Source = source;
                Option = option;
                ShouldContain = shouldContain;
            }

            public string Option { get; private set; }

            public bool ShouldContain { get; private set; }

            public string Source { get; private set; }

            public override string ToString() {
                return Source + " has " + (Option ?? "{null}");
            }
        }
    }
}