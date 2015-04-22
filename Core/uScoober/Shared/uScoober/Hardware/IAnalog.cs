namespace uScoober.Hardware
{
    public interface IAnalogInput : ISignal
    {
        /// <summary>
        ///     The voltage on the analog pin, between 0 and Aref (3.3 volts on Netduino)
        /// </summary>
        /// <returns></returns>
        double Read();
    }
}