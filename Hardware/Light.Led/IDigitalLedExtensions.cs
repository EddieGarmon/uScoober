using System.Threading;

namespace uScoober.Hardware.Light
{
    internal static class IDigitalLedExtensions
    {
        /// <summary>
        /// Beware: THIS BLOCKS!
        /// </summary>
        /// <param name="led"></param>
        /// <param name="cycleCount"></param>
        /// <param name="onTimeMilliseconds"></param>
        public static void Blink(this IDigitalLed led, int cycleCount, int onTimeMilliseconds = 500) {
            for (int i = 0; i < cycleCount; i++) {
                led.TurnOn();
                Thread.Sleep(onTimeMilliseconds);
                led.TurnOff();
                Thread.Sleep(onTimeMilliseconds);
            }
        }
    }
}