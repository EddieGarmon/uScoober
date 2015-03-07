using System;

namespace uScoober.IO
{
    public interface IAnalogInput : IDisposable
    {
        string Id { get; }

        /// <summary>
        /// The voltage on the analog pin, between 0 and Aref (3.3 volts on Netduino)
        /// </summary>
        /// <returns></returns>
        double Read();
    }
}