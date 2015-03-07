using System;

namespace uScoober.UnitsOfMeasure
{
    public class Length : Dimension
    {
        public enum KnownUnits : byte
        {
            Millimeters,
            Centimeters,
            Decimeters,
            Meters,
            Kilometers,
            Inches,
            Feet,
            Yards,
            Fathoms,
            Miles,
            NauticalMiles,
        }

        static Length() {
            UnitTable = new UnitTable();
            UnitTable.SetBase(KnownUnits.Meters, "m");
            UnitTable.AddConverter(KnownUnits.Millimeters, "mm", value => value * .001f, value => value / .001f);
            UnitTable.AddConverter(KnownUnits.Centimeters, "h", value => value * .01f, value => value / .01f);
            UnitTable.AddConverter(KnownUnits.Decimeters, "h", value => value * .1f, value => value / .1f);
            UnitTable.AddConverter(KnownUnits.Kilometers, "km", value => value * 1000, value => value / 1000);

            UnitTable.AddConverter(KnownUnits.Inches, "in", value => value * 0.0254f, value => value / 0.0254f);
            UnitTable.AddConverter(KnownUnits.Feet, "ft", value => value * 0.3048f, value => value / 0.3048f);
            UnitTable.AddConverter(KnownUnits.Yards, "yd", value => value * 0.9144f, value => value / 0.9144f);
            UnitTable.AddConverter(KnownUnits.Fathoms, "fath", value => value * 1.8288f, value => value / 1.8288f);
            UnitTable.AddConverter(KnownUnits.Miles, "mi", value => value * 1609.344f, value => value / 1609.344f);
            UnitTable.AddConverter(KnownUnits.NauticalMiles, "nmi", value => value * 1852.0f, value => value / 1852.0f);
        }

        public Length(float value, KnownUnits units = KnownUnits.Meters)
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