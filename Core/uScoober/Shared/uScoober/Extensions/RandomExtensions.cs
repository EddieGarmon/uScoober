using System;

namespace uScoober.Extensions
{
    public static class RandomExtensions
    {
        public static bool NextBool(this Random random) {
            return random.Next(100) < 50;
        }
    }
}