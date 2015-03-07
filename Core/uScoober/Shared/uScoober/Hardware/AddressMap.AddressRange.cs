namespace uScoober.Hardware
{
    public partial class AddressMap
    {
        private class _AddressRange
        {
            public _AddressRange(ushort startAddress, ushort endAddress, AddressModifier map, IHaveAddressedMemory handler) {
                StartAddress = startAddress;
                EndAddress = endAddress;
                Map = map;
                Handler = handler;
            }

            public ushort EndAddress { get; private set; }

            public IHaveAddressedMemory Handler { get; private set; }

            public AddressModifier Map { get; private set; }

            public ushort StartAddress { get; private set; }
        }
    }
}