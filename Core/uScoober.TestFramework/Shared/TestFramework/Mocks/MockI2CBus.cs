using System;
using Microsoft.SPOT.Hardware;
using uScoober.DataStructures;
using uScoober.IO.I2CBus;
using uScoober.TestFramework.Assert;

namespace uScoober.TestFramework.Mocks
{
    public class MockI2CBus : II2CBus
    {
        private readonly Ring _devices = new Ring();

        public void BufferInputFor(ushort address, params byte[] input) {
            var deviceBuffers = GetDevice(address);
            deviceBuffers.Input.Append(input);
        }

        public I2CDevice.I2CReadTransaction CreateReadTransaction(byte[] buffer) {
            return I2CDevice.CreateReadTransaction(buffer);
        }

        public I2CDevice.I2CWriteTransaction CreateWriteTransaction(byte[] buffer) {
            return I2CDevice.CreateWriteTransaction(buffer);
        }

        public void Dispose() {
            _devices.Clear();
        }

        public int Execute(I2CDevice.Configuration config, I2CDevice.I2CTransaction[] actions, int timeoutMilliseconds) {
            foreach (var action in actions) {
                if (action is I2CDevice.I2CReadTransaction) {
                    Read(config, action.Buffer, timeoutMilliseconds);
                }
                else if (action is I2CDevice.I2CWriteTransaction) {
                    Write(config, action.Buffer, timeoutMilliseconds);
                }
                else {
                    throw new InvalidOperationException("Unknown Transaction type");
                }
            }
            return 0; //bug? is this correct?
        }

        public bool Read(I2CDevice.Configuration config, byte[] readBuffer, int timeoutMilliseconds) {
            var device = GetDevice(config.Address);
            device.Input.ReadInto(readBuffer, 0, readBuffer.Length);
            return true;
        }

        public void ShouldObserveOutput(ushort address, params byte[] expectedOutput) {
            var output = GetDevice(address)
                .Output;
            output.Storage.ShouldEnumerateEqual(expectedOutput);
            output.Clear();
        }

        public bool Write(I2CDevice.Configuration config, byte[] writeBuffer, int timeoutMilliseconds) {
            var device = GetDevice(config.Address);
            device.Output.Append(writeBuffer);
            return true;
        }

        public bool WriteRead(I2CDevice.Configuration config, byte[] writeBuffer, byte[] readBuffer, int timeoutMilliseconds) {
            var device = GetDevice(config.Address);
            device.Output.Append(writeBuffer);
            device.Input.ReadInto(readBuffer, 0, readBuffer.Length);
            return true;
        }

        private DeviceBuffers GetDevice(ushort address) {
            var indexedLink = _devices.Find(obj => ((DeviceBuffers)obj).Address == address);
            if (indexedLink != null) {
                return (DeviceBuffers)indexedLink.Value;
            }
            var temp = new DeviceBuffers(address);
            _devices.InsertTail(temp);
            return temp;
        }

        private class DeviceBuffers
        {
            public DeviceBuffers(ushort address) {
                Address = address;
                Input = new MockIOBuffer();
                Output = new MockIOBuffer();
            }

            public ushort Address { get; private set; }

            public MockIOBuffer Input { get; private set; }

            public MockIOBuffer Output { get; private set; }
        }
    }
}