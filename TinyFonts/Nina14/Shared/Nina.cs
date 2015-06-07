using Microsoft.SPOT;

namespace TinyFonts
{
    public static class Nina
    {
        private static Font __nina14;

        public static Font Nina14 {
            get { return __nina14 ?? (__nina14 = ViewResources.GetFont(ViewResources.FontResources.Nina14)); }
        }
    }
}