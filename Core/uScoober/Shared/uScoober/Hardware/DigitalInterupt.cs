namespace uScoober.Hardware
{
    public static class DigitalInterupt
    {
        public const int DebounceDefault = -1;

        static DigitalInterupt() {
            AutoEnableInterruptHandler = true;
            DebounceDefaultMilliseconds = 50;
        }

        public static bool AutoEnableInterruptHandler { get; set; }

        public static int DebounceDefaultMilliseconds { get; set; }
    }
}