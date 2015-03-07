using System;

namespace uScoober.UnitsOfMeasure
{
    public class UnitDescriptor
    {
        /// <summary>
        /// Creates a 'Base' unit
        /// </summary>
        /// <param name="units"></param>
        /// <param name="abbreviation"></param>
        public UnitDescriptor(Enum units, string abbreviation)
            : this(units, abbreviation, value => value, value => value) { }

        /// <summary>
        /// Creates a computed unit.
        /// </summary>
        /// <param name="units"></param>
        /// <param name="abbreviation"></param>
        /// <param name="toBase"></param>
        /// <param name="fromBase"></param>
        public UnitDescriptor(Enum units, string abbreviation, DimensionValueConverter toBase, DimensionValueConverter fromBase) {
            Units = units;
            Abbreviation = abbreviation;
            ToBase = toBase;
            FromBase = fromBase;
        }

        public string Abbreviation { get; private set; }

        public DimensionValueConverter FromBase { get; private set; }

        public DimensionValueConverter ToBase { get; private set; }

        public Enum Units { get; private set; }
    }
}