using uScoober.Text;

namespace uScoober.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string value, string option) {
            return new StringContainsResult(value, option).IsMatch;
        }

        public static bool Contains(this string value, out StringContainsResult result, string option) {
            result = new StringContainsResult(value, option);
            return result.IsMatch;
        }

        public static bool ContainsAny(this string value, params string[] options) {
            StringContainsResult result;
            return ContainsAny(value, out result, options);
        }

        public static bool ContainsAny(this string value, out StringContainsResult result, params string[] options) {
            if (options == null) {
                result = null;
                return false;
            }
            for (int i = 0; i < options.Length; i++) {
                result = new StringContainsResult(value, options[i]);
                if (!result.IsMatch) {
                    continue;
                }
                return true;
            }
            result = null;
            return false;
        }

        public static bool EndsWith(this string value, string ending) {
            return new StringEndsWithResult(value, ending).IsMatch;
        }

        public static bool EndsWithAny(this string value, params string[] options) {
            string match;
            return EndsWithAny(value, out match, options);
        }

        public static bool EndsWithAny(this string value, out string match, params string[] options) {
            for (int i = 0; i < options.Length; i++) {
                var result = new StringEndsWithResult(value, options[i]);
                if (!result.IsMatch) {
                    continue;
                }
                match = result.Option;
                return true;
            }

            match = null;
            return false;
        }
    }
}