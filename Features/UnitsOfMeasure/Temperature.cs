using System;

namespace uScoober.UnitsOfMeasure
{
    public class Temperature : Dimension
    {
        public enum KnownUnits : byte
        {
            Celsius,
            Fahrenheit,
            Kelvin,
        }

        static Temperature() {
            UnitTable = new UnitTable();
            UnitTable.SetBase(KnownUnits.Celsius, "°C");
            UnitTable.AddConverter(KnownUnits.Fahrenheit, "°F", value => (value - 32) / 1.8f, value => (value * 1.8f) + 32);
            UnitTable.AddConverter(KnownUnits.Kelvin, "°K", value => value + 273.15f, value => value - 273.15f);
        }

        public Temperature(float value, KnownUnits units = KnownUnits.Celsius)
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