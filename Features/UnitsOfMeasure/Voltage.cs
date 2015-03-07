using System;

namespace uScoober.UnitsOfMeasure
{
    public class Voltage : Dimension
    {
        public enum KnownUnits : byte
        {
            MilliVolts,
            Volts,
            KiloVolts,
        }

        static Voltage() {
            UnitTable = new UnitTable();
            UnitTable.SetBase(KnownUnits.Volts, "V");
            UnitTable.AddConverter(KnownUnits.MilliVolts, "mV", value => value * 0.001f, value => value / 0.001f);
            UnitTable.AddConverter(KnownUnits.KiloVolts, "kV", value => value * 1000, value => value / 1000);
        }

        public Voltage(float value, KnownUnits units = KnownUnits.Volts)
            : base(value, UnitTable.Lookup(units)) { }

        public KnownUnits Units {
            get { return (KnownUnits)CurrentUnits; }
            set { CurrentUnits = value; }
        }

        protected override UnitDescriptor GetDescriptorFor(Enum units) {
            return UnitTable.Lookup(units);
        }

        private static readonly UnitTable UnitTable;
    }
}