using System.Text;

namespace uScoober.Extensions
{
    internal static class StringBuilderExtensions
    {
        public static void RemoveEnd(this StringBuilder builder, int length) {
            builder.Remove(builder.Length - length, length);
        }
    }
}