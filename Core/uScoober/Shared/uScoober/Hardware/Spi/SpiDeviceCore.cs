namespace uScoober.Hardware.Spi
{
    public abstract class SpiDeviceCore : DisposableBase,
                                          ISpiDevice
    {
        private readonly ISpiBus _bus;
        private readonly SpiDeviceSettings _settings;

        protected SpiDeviceCore(ISpiBus bus, SpiDeviceSettings settings) {
            _bus = bus;
            _settings = settings;
        }

        protected void Read(byte[] buffer) {
            lock (_bus) {
                if (_settings.SoftChipSelectEnabled) {
                    _settings.ChipSelect.Write(_settings.ChipSelectActiveState);
                }
                _bus.Read(_settings, buffer);
                if (_settings.SoftChipSelectEnabled) {
                    _settings.ChipSelect.Write(!_settings.ChipSelectActiveState);
                }
            }
        }

        protected void Read(ushort[] buffer, ByteOrder byteOrder) {
            lock (_bus) {
                if (_settings.SoftChipSelectEnabled) {
                    _settings.ChipSelect.Write(_settings.ChipSelectActiveState);
                }
                _bus.Read(_settings, buffer, byteOrder);
                if (_settings.SoftChipSelectEnabled) {
                    _settings.ChipSelect.Write(!_settings.ChipSelectActiveState);
                }
            }
        }

        protected void Write(byte[] buffer) {
            lock (_bus) {
                if (_settings.SoftChipSelectEnabled) {
                    _settings.ChipSelect.Write(_settings.ChipSelectActiveState);
                }
                _bus.Write(_settings, buffer);
                if (_settings.SoftChipSelectEnabled) {
                    _settings.ChipSelect.Write(!_settings.ChipSelectActiveState);
                }
            }
        }

        protected void Write(ushort[] buffer, ByteOrder byteOrder) {
            lock (_bus) {
                if (_settings.SoftChipSelectEnabled) {
                    _settings.ChipSelect.Write(_settings.ChipSelectActiveState);
                }
                _bus.Write(_settings, buffer, byteOrder);
                if (_settings.SoftChipSelectEnabled) {
                    _settings.ChipSelect.Write(!_settings.ChipSelectActiveState);
                }
            }
        }

        protected void WriteRead(byte[] writeBuffer, byte[] readBuffer, int startReadingAtOffset = 0) {
            lock (_bus) {
                if (_settings.SoftChipSelectEnabled) {
                    _settings.ChipSelect.Write(_settings.ChipSelectActiveState);
                }
                _bus.WriteRead(_settings, writeBuffer, readBuffer, startReadingAtOffset);
                if (_settings.SoftChipSelectEnabled) {
                    _settings.ChipSelect.Write(!_settings.ChipSelectActiveState);
                }
            }
        }

        protected void WriteRead(ushort[] writeBuffer, ushort[] readBuffer, ByteOrder byteOrder, int startReadingAtOffset = 0) {
            lock (_bus) {
                if (_settings.SoftChipSelectEnabled) {
                    _settings.ChipSelect.Write(_settings.ChipSelectActiveState);
                }
                _bus.WriteRead(_settings, writeBuffer, readBuffer, byteOrder, startReadingAtOffset);
                if (_settings.SoftChipSelectEnabled) {
                    _settings.ChipSelect.Write(!_settings.ChipSelectActiveState);
                }
            }
        }

        protected void WriteRead(byte[] writeBuffer,
                                 int writeOffset,
                                 int writeCount,
                                 byte[] readBuffer,
                                 int readOffset,
                                 int readCount,
                                 int startReadingAtOffset = 0) {
            lock (_bus) {
                if (_settings.SoftChipSelectEnabled) {
                    _settings.ChipSelect.Write(_settings.ChipSelectActiveState);
                }
                _bus.WriteRead(_settings, writeBuffer, writeOffset, writeCount, readBuffer, readOffset, readCount, startReadingAtOffset);
                if (_settings.SoftChipSelectEnabled) {
                    _settings.ChipSelect.Write(!_settings.ChipSelectActiveState);
                }
            }
        }

        protected void WriteRead(ushort[] writeBuffer,
                                 int writeOffset,
                                 int writeCount,
                                 ushort[] readBuffer,
                                 int readOffset,
                                 int readCount,
                                 ByteOrder byteOrder,
                                 int startReadingAtOffset) {
            lock (_bus) {
                if (_settings.SoftChipSelectEnabled) {
                    _settings.ChipSelect.Write(_settings.ChipSelectActiveState);
                }
                _bus.WriteRead(_settings, writeBuffer, writeOffset, writeCount, readBuffer, readOffset, readCount, byteOrder, startReadingAtOffset);
                if (_settings.SoftChipSelectEnabled) {
                    _settings.ChipSelect.Write(!_settings.ChipSelectActiveState);
                }
            }
        }

        void ISpiDevice.Read(byte[] buffer) {
            Read(buffer);
        }

        void ISpiDevice.Read(ushort[] buffer, ByteOrder byteOrder) {
            Read(buffer, byteOrder);
        }

        void ISpiDevice.Write(byte[] buffer) {
            Write(buffer);
        }

        void ISpiDevice.Write(ushort[] buffer, ByteOrder byteOrder) {
            Write(buffer, byteOrder);
        }

        void ISpiDevice.WriteRead(byte[] writeBuffer, byte[] readBuffer, int startReadingAtOffset) {
            WriteRead(writeBuffer, readBuffer, startReadingAtOffset);
        }

        void ISpiDevice.WriteRead(ushort[] writeBuffer, ushort[] readBuffer, ByteOrder byteOrder, int startReadingAtOffset) {
            WriteRead(writeBuffer, readBuffer, byteOrder, startReadingAtOffset);
        }

        void ISpiDevice.WriteRead(byte[] writeBuffer,
                                  int writeOffset,
                                  int writeCount,
                                  byte[] readBuffer,
                                  int readOffset,
                                  int readCount,
                                  int startReadingAtOffset) {
            WriteRead(writeBuffer, writeOffset, writeCount, readBuffer, readOffset, readCount, startReadingAtOffset);
        }

        void ISpiDevice.WriteRead(ushort[] writeBuffer,
                                  int writeOffset,
                                  int writeCount,
                                  ushort[] readBuffer,
                                  int readOffset,
                                  int readCount,
                                  ByteOrder byteOrder,
                                  int startReadingAtOffset) {
            WriteRead(writeBuffer, writeOffset, writeCount, readBuffer, readOffset, readCount, byteOrder, startReadingAtOffset);
        }
    }
}