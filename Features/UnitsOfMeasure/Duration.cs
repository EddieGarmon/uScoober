using System;

namespace uScoober.UnitsOfMeasure
{
    /// <summary>
    /// A representation of an amount of Time
    /// </summary>
    public class Duration : Dimension
    {
        public enum KnownUnits : byte
        {
            Seconds,
            Minutes,
            Hours
        }

        static Duration() {
            __unitTable = new UnitTable();
            __unitTable.SetBase(KnownUnits.Seconds, "s");
            __unitTable.AddConverter(KnownUnits.Minutes, "m", value => value * 60, value => value / 60);
            __unitTable.AddConverter(KnownUnits.Hours, "h", value => value * 3600, value => value / 3600);
        }

        public Duration(float value, KnownUnits units = KnownUnits.Seconds)
            : base(value, __unitTable.Lookup(units)) { }

        public KnownUnits Units {
            get { return (KnownUnits)CurrentUnits; }
            set { CurrentUnits = value; }
        }

        protected override UnitDescriptor GetDescriptorFor(Enum units) {
            return __unitTable.Lookup(units);
        }

        private static readonly UnitTable __unitTable;
    }
}