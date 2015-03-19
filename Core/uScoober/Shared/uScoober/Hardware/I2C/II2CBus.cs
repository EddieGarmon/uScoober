using System;
using Microsoft.SPOT.Hardware;

namespace uScoober.Hardware.I2C
{
    // I²C Bus
    public interface II2CBus : IDisposable
    {
        /// <summary>Creates a new <see cref="I2CDevice.I2CReadTransaction" /> for use in complex transactions</summary>
        /// <param name="buffer">Buffer to be read into. The Length property of buffer determines how many bytes are read</param>
        /// <returns>New transaction</returns>
        /// <seealso cref="Execute" />
        I2CDevice.I2CReadTransaction CreateReadTransaction(byte[] buffer);

        /// <summary>Creates a new <see cref="I2CDevice.I2CWriteTransaction" /> for use in complex transactions</summary>
        /// <param name="buffer">Buffer of data to write. The Length property of Buffer determines how many bytes are written</param>
        /// <returns>New transaction</returns>
        /// <seealso cref="Execute" />
        I2CDevice.I2CWriteTransaction CreateWriteTransaction(byte[] buffer);

        /// <summary>Executes a series of bus transactions with repeat start conditions in between each one</summary>
        /// <param name="config">Configuration for the bus during this operation</param>
        /// <param name="actions">Array of transactions to execute</param>
        /// <param name="timeoutMilliseconds">Timeout for the trnasactions</param>
        /// <returns>Total number of bytes transfered in both directions</returns>
        int Execute(I2CDevice.Configuration config, I2CDevice.I2CTransaction[] actions, int timeoutMilliseconds);

        /// <summary>Performs a simple data read operation</summary>
        /// <param name="config">Configuration for the bus during this operation</param>
        /// <param name="readBuffer">
        ///     Buffer to receive data. The Length property of ReadBuffer determines the number of bytes to
        ///     read
        /// </param>
        /// <param name="timeoutMilliseconds">Millisecond time out value</param>
        /// <remarks>
        ///     Many I2C devices have a simple protocol that supports reading a
        ///     value from a register. This method simplifies that use case by
        ///     wrapping up the I2CTransaction creation and timeout detection etc.
        /// </remarks>
        /// <exception cref="T:System.IO.IOException">Operation failed to complete</exception>
        bool Read(I2CDevice.Configuration config, byte[] readBuffer, int timeoutMilliseconds);

        /// <summary>Performs a simple data write operation</summary>
        /// <param name="config">Configuration for the bus during this operation</param>
        /// <param name="writeBuffer">Buffer containing data to write. The Length property determines the number of bytes written.</param>
        /// <param name="timeoutMilliseconds">Millisecond time out value</param>
        /// <remarks>
        ///     Many I2C devices have a simple protocol that supports writing a
        ///     value to a register. This method simplifies that use case by
        ///     wrapping up the I2CTransaction creation and timeout detection etc.
        /// </remarks>
        /// <exception cref="T:System.IO.IOException">Operation failed to complete</exception>
        bool Write(I2CDevice.Configuration config, byte[] writeBuffer, int timeoutMilliseconds);

        /// <summary>Performs a common Write-repeatstart-read command response operation</summary>
        /// <param name="config">Configuration for the bus during this operation</param>
        /// <param name="writeBuffer">Buffer containing data to write</param>
        /// <param name="readBuffer">
        ///     Buffer to receive data. The Length property of ReadBuffer determines the number of bytes to
        ///     read
        /// </param>
        /// <param name="timeoutMilliseconds">Millisecond time out value</param>
        /// <remarks>
        ///     Many I2C devices have a simple command response protocol of some sort
        ///     this method simplifies the implementation of device specific drivers
        ///     by wrapping up the I2CTransaction creation and timeout detection etc.
        ///     to support a simple command/response type of protocol. It creates a write
        ///     transaction and a read transaction with a repeat-start condition in between
        ///     to maintain control of the bus for the entire operation.
        /// </remarks>
        /// <exception cref="T:System.IO.IOException">Operation failed to complete</exception>
        bool WriteRead(I2CDevice.Configuration config, byte[] writeBuffer, byte[] readBuffer, int timeoutMilliseconds);
    }
}