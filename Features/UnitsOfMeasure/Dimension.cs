using System;

namespace uScoober.UnitsOfMeasure
{
    public abstract class Dimension
    {
        private readonly float _baseUnitsValue;
        private Enum _currentUnits;
        private float _currentUnitsValue;

        protected Dimension(float value, UnitDescriptor descriptor) {
            _currentUnitsValue = value;
            _currentUnits = descriptor.Units;
            _baseUnitsValue = descriptor.ToBase(value);
        }

        public float Value {
            get { return _currentUnitsValue; }
            set { _currentUnitsValue = value; }
        }

        protected Enum CurrentUnits {
            get { return _currentUnits; }
            set {
                var descriptor = GetDescriptorFor(value);
                _currentUnitsValue = descriptor.FromBase(_baseUnitsValue);
                _currentUnits = value;
            }
        }

        public override string ToString() {
            return _currentUnitsValue.ToString("F3") + " " + GetDescriptorFor(_currentUnits)
                                                                 .Abbreviation;
        }

        protected abstract UnitDescriptor GetDescriptorFor(Enum units);
    }
}