using Microsoft.SPOT.Hardware;

namespace uScoober.IO.I2C
{
    /// <summary>Base class for I2C peripheral device drivers</summary>
    public abstract class I2CDeviceCore : DisposableBase,
                                          II2CDevice
    {
        private readonly byte[] _buffer1 = new byte[1];
        private readonly byte[] _buffer2 = new byte[2];
        private readonly byte[] _buffer3 = new byte[3];
        private readonly II2CBus _bus;
        private readonly I2CDevice.Configuration _config;
        private readonly byte[] _registerAddressBuffer = new byte[1];
        private int _timeoutMilliseconds;

        protected I2CDeviceCore(II2CBus bus, ushort address, int clockRateKhz) {
            _config = new I2CDevice.Configuration(address, clockRateKhz);
            _bus = bus;
            _timeoutMilliseconds = 1000;
        }

        public ushort Address {
            get { return _config.Address; }
        }

        public int TimeoutMilliseconds {
            get { return _timeoutMilliseconds; }
            set { _timeoutMilliseconds = value; }
        }

        protected I2CDevice.I2CReadTransaction CreateReadTransaction(params byte[] buffer) {
            return _bus.CreateReadTransaction(buffer);
        }

        protected I2CDevice.I2CWriteTransaction CreateWriteTransaction(params byte[] buffer) {
            return _bus.CreateWriteTransaction(buffer);
        }

        protected int Execute(I2CDevice.I2CTransaction action) {
            lock (_bus) {
                return _bus.Execute(_config,
                                    new[] {
                                        action
                                    },
                                    _timeoutMilliseconds);
            }
        }

        protected int Execute(I2CDevice.I2CTransaction[] actions) {
            lock (_bus) {
                return _bus.Execute(_config, actions, _timeoutMilliseconds);
            }
        }

        protected bool Read(out byte value) {
            lock (_bus) {
                bool success = _bus.Read(_config, _buffer1, _timeoutMilliseconds);
                if (!success) {
                    value = 0;
                    return false;
                }
                value = _buffer1[0];
                return true;
            }
        }

        protected bool Read(byte[] readBuffer) {
            lock (_bus) {
                return _bus.Read(_config, readBuffer, _timeoutMilliseconds);
            }
        }

        protected bool Read(out ushort value, ByteOrder byteOrder) {
            lock (_bus) {
                bool success = _bus.Read(_config, _buffer2, _timeoutMilliseconds);
                if (!success) {
                    value = 0;
                    return false;
                }
                if (byteOrder == ByteOrder.BigEndian) {
                    value = (ushort)(_buffer2[0] << 8 | _buffer2[1]);
                }
                else {
                    value = (ushort)(_buffer2[1] << 8 | _buffer2[0]);
                }
                return true;
            }
        }

        protected bool ReadRegister(byte address, out byte value) {
            lock (_bus) {
                _registerAddressBuffer[0] = address;
                bool success = _bus.WriteRead(_config, _registerAddressBuffer, _buffer1, _timeoutMilliseconds);
                if (!success) {
                    value = 0;
                    return false;
                }
                value = _buffer1[0];
                return true;
            }
        }

        protected bool ReadRegister(byte address, out ushort value, ByteOrder byteOrder) {
            lock (_bus) {
                _registerAddressBuffer[0] = address;
                bool success = _bus.WriteRead(_config, _registerAddressBuffer, _buffer2, _timeoutMilliseconds);
                if (!success) {
                    value = 0;
                    return false;
                }
                if (byteOrder == ByteOrder.BigEndian) {
                    value = (ushort)(_buffer2[0] << 8 | _buffer2[1]);
                }
                else {
                    value = (ushort)(_buffer2[1] << 8 | _buffer2[0]);
                }
                return true;
            }
        }

        protected bool ReadRegister(byte address, byte[] buffer) {
            lock (_bus) {
                _registerAddressBuffer[0] = address;
                return _bus.WriteRead(_config, _registerAddressBuffer, buffer, _timeoutMilliseconds);
            }
        }

        protected bool Write(byte[] writeBuffer) {
            lock (_bus) {
                return _bus.Write(_config, writeBuffer, _timeoutMilliseconds);
            }
        }

        protected bool Write(byte value) {
            lock (_bus) {
                _buffer1[0] = value;
                return _bus.Write(_config, _buffer1, _timeoutMilliseconds);
            }
        }

        protected bool Write(ushort value, ByteOrder byteOrder) {
            lock (_bus) {
                if (byteOrder == ByteOrder.BigEndian) {
                    _buffer2[1] = (byte)(value >> 8);
                    _buffer2[2] = (byte)value;
                }
                else {
                    _buffer2[2] = (byte)(value >> 8);
                    _buffer2[1] = (byte)value;
                }
                return _bus.Write(_config, _buffer2, _timeoutMilliseconds);
            }
        }

        protected bool WriteRead(byte[] writeBuffer, byte[] readBuffer) {
            lock (_bus) {
                return _bus.WriteRead(_config, writeBuffer, readBuffer, _timeoutMilliseconds);
            }
        }

        protected bool WriteRegister(byte address, ushort value, ByteOrder byteOrder) {
            lock (_bus) {
                _buffer3[0] = address;
                if (byteOrder == ByteOrder.BigEndian) {
                    _buffer3[1] = (byte)(value >> 8);
                    _buffer3[2] = (byte)value;
                }
                else {
                    _buffer3[2] = (byte)(value >> 8);
                    _buffer3[1] = (byte)value;
                }
                return _bus.Write(_config, _buffer3, _timeoutMilliseconds);
            }
        }

        protected bool WriteRegister(byte address, byte[] buffer) {
            lock (_bus) {
                var temp = new byte[buffer.Length + 1];
                temp[0] = address;
                buffer.CopyTo(temp, 1);
                return _bus.Write(_config, temp, _timeoutMilliseconds);
            }
        }

        protected bool WriteRegister(byte address, byte value) {
            lock (_bus) {
                _buffer2[0] = address;
                _buffer2[1] = value;
                return _bus.Write(_config, _buffer2, _timeoutMilliseconds);
            }
        }

        I2CDevice.I2CReadTransaction II2CDevice.CreateReadTransaction(params byte[] buffer) {
            return CreateReadTransaction(buffer);
        }

        I2CDevice.I2CWriteTransaction II2CDevice.CreateWriteTransaction(params byte[] buffer) {
            return CreateWriteTransaction(buffer);
        }

        int II2CDevice.Execute(I2CDevice.I2CTransaction action) {
            return Execute(action);
        }

        int II2CDevice.Execute(I2CDevice.I2CTransaction[] actions) {
            return Execute(actions);
        }

        bool II2CDevice.Read(byte[] readBuffer) {
            return Read(readBuffer);
        }

        bool II2CDevice.Read(out byte value) {
            return Read(out value);
        }

        bool II2CDevice.Read(out ushort value, ByteOrder byteOrder) {
            return Read(out value, byteOrder);
        }

        bool II2CDevice.ReadRegister(byte address, out byte value) {
            return ReadRegister(address, out value);
        }

        bool II2CDevice.ReadRegister(byte address, out ushort value, ByteOrder byteOrder) {
            return ReadRegister(address, out value, byteOrder);
        }

        bool II2CDevice.ReadRegister(byte address, byte[] buffer) {
            return ReadRegister(address, buffer);
        }

        bool II2CDevice.Write(byte[] writeBuffer) {
            return Write(writeBuffer);
        }

        bool II2CDevice.Write(byte value) {
            return Write(value);
        }

        bool II2CDevice.Write(ushort value, ByteOrder byteOrder) {
            return Write(value, byteOrder);
        }

        bool II2CDevice.WriteRead(byte[] writeBuffer, byte[] readBuffer) {
            return WriteRead(writeBuffer, readBuffer);
        }

        bool II2CDevice.WriteRegister(byte address, ushort value, ByteOrder byteOrder) {
            return WriteRegister(address, value, byteOrder);
        }

        bool II2CDevice.WriteRegister(byte address, byte[] buffer) {
            return WriteRegister(address, buffer);
        }

        bool II2CDevice.WriteRegister(byte address, byte value) {
            return WriteRegister(address, value);
        }
    }
}