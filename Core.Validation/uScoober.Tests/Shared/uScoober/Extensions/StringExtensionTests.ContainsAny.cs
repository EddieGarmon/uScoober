using System.Collections;
using uScoober.TestFramework.Assert;

namespace uScoober.Extensions
{
    public partial class StringExtensionTests
    {
        public IEnumerable ContainsAny_Data() {
            return new[] {
                new ContainsAnyArgs("abcd", false),
                new ContainsAnyArgs("abcd", false, null),
                new ContainsAnyArgs("abcd", false, null, "", "e", "ef", "efg", "abcde"),
                new ContainsAnyArgs("abcd", true, "a"),
                new ContainsAnyArgs("abcd", true, "", "a"),
                new ContainsAnyArgs("abcd", true, "ab", "abc")
            };
        }

        public void ContainsAny_Theory(ContainsAnyArgs args) {
            args.Source.ContainsAny(args.Options)
                .ShouldEqual(args.ShouldContain);
        }

        public class ContainsAnyArgs
        {
            public ContainsAnyArgs(string source, bool shouldContain, params string[] options) {
                Source = source;
                Options = options;
                ShouldContain = shouldContain;
            }

            public string[] Options { get; private set; }

            public bool ShouldContain { get; private set; }

            public string Source { get; private set; }

            public override string ToString() {
                if (Options == null) {
                    return Source + " [null]";
                }
                if (Options.Length == 0) {
                    return Source + " [empty]";
                }
                return Source + " [" + Options.Length + "] " + (Options[0] ?? "{null}");
            }
        }
    }
}