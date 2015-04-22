using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware.Boards
{
    internal interface IDuinoDigitalInputs
    {
        /// <summary>
        /// Generally used with pull-down resistors
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="internalResistorMode"></param>
        /// <param name="id"></param>
        /// <param name="debounceMilliseconds"></param>
        /// <returns></returns>
        IDigitalInterupt Bind(Cpu.Pin pin, ResistorMode internalResistorMode = ResistorMode.Disabled, string id = null, int debounceMilliseconds = 0);

        /// <summary>
        /// Generally used with pull-down resistors
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="internalResistorMode"></param>
        /// <param name="interruptMode"></param>
        /// <param name="handler"></param>
        /// <param name="id"></param>
        /// <param name="debounceMilliseconds"></param>
        /// <param name="interuptEnabled"></param>
        /// <returns></returns>
        IDigitalInterupt Bind(Cpu.Pin pin,
                              ResistorMode internalResistorMode,
                              InterruptMode interruptMode,
                              InteruptHandler handler,
                              string id = null,
                              int debounceMilliseconds = 0,
                              bool interuptEnabled = true);

        IDigitalInterupt Get(Cpu.Pin pin);

        /// <summary>
        /// Generally used with pull-up resistors
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="internalResistorMode"></param>
        /// <param name="id"></param>
        /// <param name="debounceMilliseconds"></param>
        /// <returns></returns>
        IDigitalInterupt Invert(Cpu.Pin pin, ResistorMode internalResistorMode = ResistorMode.Disabled, string id = null, int debounceMilliseconds = 0);

        /// <summary>
        /// Generally used with pull-up resistors
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="internalResistorMode"></param>
        /// <param name="interruptMode"></param>
        /// <param name="handler"></param>
        /// <param name="id"></param>
        /// <param name="debounceMilliseconds"></param>
        /// <param name="interuptEnabled"></param>
        /// <returns></returns>
        IDigitalInterupt Invert(Cpu.Pin pin,
                                ResistorMode internalResistorMode,
                                InterruptMode interruptMode,
                                InteruptHandler handler,
                                string id = null,
                                int debounceMilliseconds = 0,
                                bool interuptEnabled = true);
    }
}