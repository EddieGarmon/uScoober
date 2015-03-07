using Microsoft.SPOT;
using uScoober.UI.Resources;

namespace uScoober.TestFramework.UI.Resources
{
    public static class Fonts
    {
        private static Font __nina14;
        private static Font __small;

        public static Font Nina14 {
            get { return __nina14 ?? (__nina14 = ViewResources.GetFont(ViewResources.FontResources.Nina14)); }
        }

        public static Font Small {
            get { return __small ?? (__small = ViewResources.GetFont(ViewResources.FontResources.Small)); }
        }
    }
}