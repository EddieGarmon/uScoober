using Microsoft.SPOT;

namespace TinyFonts
{
    public static class Fonts
    {
        private static Font __small;

        public static Font Small {
            get { return __small ?? (__small = ViewResources.GetFont(ViewResources.FontResources.Small)); }
        }
    }
}