using System;
using System.Text;
using uScoober.DataStructures;
using uScoober.Text;

namespace uScoober.Hardware
{
    public partial class AddressMap : Ring,
                                      IHaveAddressedMemory
    {
        public delegate ushort AddressModifier(ushort address);

        public int TotalBytesAvailable { get; private set; }

        public void MapRange(ushort startAddress, ushort endAddress, AddressModifier addressModifier, IHaveAddressedMemory handler) {
            if (startAddress >= endAddress) {
                throw new Exception("Invalid start and end addresses.");
            }
            var newRange = new _AddressRange(startAddress, endAddress, addressModifier, handler);
            TotalBytesAvailable += endAddress - startAddress + 1;

            // NB: ranges in sorted order...
            Link previous = null;
            var enumerator = GetForwardEnumerator();
            while (enumerator.MoveNext()) {
                var range = (_AddressRange)enumerator.Current;
                if (range.EndAddress < startAddress) {
                    previous = enumerator.CurrentLink;
                    continue;
                }
                if (endAddress < range.StartAddress) {
                    InsertAfter(previous, newRange);
                    return;
                }
                var message = new StringBuilder("Address range overlaps are not allowed.", 80);
                if (previous != null) {
                    var previousRange = (_AddressRange)previous.Value;
                    message.Append("\r\n\tPrevious: [");
                    message.Append(HexString.GetString(previousRange.StartAddress));
                    message.Append(",");
                    message.Append(HexString.GetString(previousRange.EndAddress));
                    message.Append("]");
                }
                message.Append("\r\n\t **NEW**: [");
                message.Append(HexString.GetString(startAddress));
                message.Append(",");
                message.Append(HexString.GetString(endAddress));
                message.Append("]\r\n\t    Next: [");
                message.Append(HexString.GetString(range.StartAddress));
                message.Append(",");
                message.Append(HexString.GetString(range.EndAddress));
                message.Append("]");
                throw new Exception(message.ToString());
            }

            InsertTail(newRange);
        }

        public byte ReadMemory(ushort address) {
            var range = FindRange(address);
            return range.Handler.ReadMemory(range.Map(address));
        }

        public void ReadMemory(ushort startAddress, byte[] buffer, int bufferStartIndex = 0, int length = -1) {
            var range = FindRange(startAddress);
            range.Handler.ReadMemory(range.Map(startAddress), buffer, bufferStartIndex, length);
        }

        public void WriteMemory(ushort address, byte value) {
            var range = FindRange(address);
            range.Handler.WriteMemory(range.Map(address), value);
        }

        public void WriteMemory(ushort startAddress, byte[] buffer, int bufferStartIndex = 0, int length = -1) {
            var range = FindRange(startAddress);
            range.Handler.WriteMemory(range.Map(startAddress), buffer, bufferStartIndex, length);
        }

        private _AddressRange FindRange(ushort address) {
            var enumerator = GetForwardEnumerator();
            while (enumerator.MoveNext()) {
                var range = (_AddressRange)enumerator.Current;
                if (range.StartAddress <= address && address <= range.EndAddress) {
                    return range;
                }
            }
            throw new Exception("Unmapped address: " + HexString.GetString(address));
        }
    }
}