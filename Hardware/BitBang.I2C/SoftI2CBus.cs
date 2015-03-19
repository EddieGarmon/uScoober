using System;
using Microsoft.SPOT.Hardware;
using uScoober.Hardware.I2C;

namespace uScoober.IO.I2CBus
{
    public class SoftwareI2CBus : DisposableBase,
                                  II2CBus
    {
        public SoftwareI2CBus(Cpu.Pin clock, Cpu.Pin data) { }

        public I2CDevice.I2CReadTransaction CreateReadTransaction(byte[] buffer) {
            throw new NotImplementedException("SoftwareI2CBus.CreateReadTransaction");
        }

        public I2CDevice.I2CWriteTransaction CreateWriteTransaction(byte[] buffer) {
            throw new NotImplementedException("SoftwareI2CBus.CreateWriteTransaction");
        }

        public int Execute(I2CDevice.Configuration config, I2CDevice.I2CTransaction[] actions, int timeoutMilliseconds) {
            throw new NotImplementedException("SoftwareI2CBus.Execute");
        }

        public bool Read(I2CDevice.Configuration config, byte[] readBuffer, int timeoutMilliseconds) {
            throw new NotImplementedException("SoftwareI2CBus.Read");
        }

        public bool Write(I2CDevice.Configuration config, byte[] writeBuffer, int timeoutMilliseconds) {
            throw new NotImplementedException("SoftwareI2CBus.Write");
        }

        public bool WriteRead(I2CDevice.Configuration config, byte[] writeBuffer, byte[] readBuffer, int timeoutMilliseconds) {
            throw new NotImplementedException("SoftwareI2CBus.WriteRead");
        }

        protected override void DisposeManagedResources() {
            base.DisposeManagedResources();
        }
    }
}