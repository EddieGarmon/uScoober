using System;
using System.Collections;

namespace uScoober.UnitsOfMeasure
{
    public class UnitTable
    {
        private readonly IDictionary _descriptors = new Hashtable();

        internal void AddConverter(Enum units, string abbreviation, DimensionValueConverter toBase, DimensionValueConverter fromBase) {
            _descriptors.Add(units, new UnitDescriptor(units, abbreviation, toBase, fromBase));
        }

        internal UnitDescriptor Lookup(Enum units) {
            return (UnitDescriptor)_descriptors[units];
        }

        internal void SetBase(Enum units, string abbreviation) {
            _descriptors.Add(units, new UnitDescriptor(units, abbreviation));
        }
    }
}