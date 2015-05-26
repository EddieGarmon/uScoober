namespace uScoober
{
    public static class Empty
    {
        private static readonly object[] __objects = new object[0];

        public static object[] Objects {
            get { return __objects; }
        }
    }
}