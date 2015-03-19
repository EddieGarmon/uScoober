using Microsoft.SPOT.Hardware;
using uScoober.Hardware.I2C;

namespace uScoober.Hardware.Spot
{
    public class SpotI2CBus : DisposableBase,
                              II2CBus
    {
        private readonly I2CDevice _nativeBus;

        public SpotI2CBus() {
            _nativeBus = new I2CDevice(new I2CDevice.Configuration(0, 100));
        }

        public I2CDevice.I2CReadTransaction CreateReadTransaction(byte[] buffer) {
            ThrowIfDisposed();
            return I2CDevice.CreateReadTransaction(buffer);
        }

        public I2CDevice.I2CWriteTransaction CreateWriteTransaction(byte[] buffer) {
            ThrowIfDisposed();
            return I2CDevice.CreateWriteTransaction(buffer);
        }

        public int Execute(I2CDevice.Configuration config, I2CDevice.I2CTransaction[] actions, int timeoutMilliseconds) {
            ThrowIfDisposed();
            lock (_nativeBus) {
                _nativeBus.Config = config;
                return _nativeBus.Execute(actions, timeoutMilliseconds);
            }
        }

        public bool Read(I2CDevice.Configuration config, byte[] readBuffer, int timeoutMilliseconds) {
            ThrowIfDisposed();
            lock (_nativeBus) {
                _nativeBus.Config = config;
                I2CDevice.I2CTransaction[] actions = {
                    I2CDevice.CreateReadTransaction(readBuffer)
                };
                int bytesTransfered = _nativeBus.Execute(actions, timeoutMilliseconds);
                return (bytesTransfered >= readBuffer.Length);
            }
        }

        public bool Write(I2CDevice.Configuration config, byte[] writeBuffer, int timeoutMilliseconds) {
            ThrowIfDisposed();
            lock (_nativeBus) {
                _nativeBus.Config = config;
                I2CDevice.I2CTransaction[] actions = {
                    I2CDevice.CreateWriteTransaction(writeBuffer)
                };
                int bytesTransfered = _nativeBus.Execute(actions, timeoutMilliseconds);
                return (bytesTransfered >= writeBuffer.Length);
            }
        }

        public bool WriteRead(I2CDevice.Configuration config, byte[] writeBuffer, byte[] readBuffer, int timeoutMilliseconds) {
            ThrowIfDisposed();
            lock (_nativeBus) {
                _nativeBus.Config = config;
                I2CDevice.I2CTransaction[] actions = {
                    I2CDevice.CreateWriteTransaction(writeBuffer),
                    I2CDevice.CreateReadTransaction(readBuffer)
                };
                int bytesTransfered = _nativeBus.Execute(actions, timeoutMilliseconds);
                return (bytesTransfered >= (writeBuffer.Length + readBuffer.Length));
            }
        }

        protected override void DisposeManagedResources() {
            _nativeBus.Dispose();
        }

        /// <summary>Finalizer for this class</summary>
        /// <remarks>
        ///     Use C# destructor syntax for finalization code.
        ///     This destructor will run only if the Dispose method
        ///     does not get called to suppress finalization.
        /// </remarks>
        ~SpotI2CBus() {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(DisposeReason.Finalizer);
        }
    }
}