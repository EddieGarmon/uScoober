using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using uScoober.IO.I2C;
using uScoober.Text;

namespace uScoober.IO.I2CBus
{
    public class I2CBusScanner
    {
        public static void Scan(II2CBus bus) {
            if (bus == null) {
                throw new ArgumentNullException("bus");
            }
            int count = 0;
            const int clockRateKhz = 100;
            const int timeout = 100;
            const byte startAddress = 0x08;
            const byte endAddress = 127;
            Debug.Print("Scanning I2C Bus for devices starting at: " + HexString.GetString(startAddress) + " ... ");
            for (byte address = startAddress; address < endAddress; address++) {
                var configuration = new I2CDevice.Configuration(address, clockRateKhz);
                var buffer = new byte[] {
                    0
                };
                bool canRead = bus.Read(configuration, buffer, timeout);
                bool canWrite = bus.Write(configuration, buffer, timeout);
                if (canRead || canWrite) {
                    count++;
                    Debug.Print("Address: 0x" + HexString.GetString(address) + ", Read => " + canRead + ", Write => " + canWrite);
                }
            }
            Debug.Print("Scanning ended at: " + HexString.GetString(endAddress));
            Debug.Print("Scanning found " + count + " devices.");
        }
    }
}