namespace uScoober.Hardware.Display
{
    internal abstract class RegisterBasedCharacterDisplayDriver : CharacterDisplayDriver
    {
        private readonly DisplayPins _displayPins;

        protected RegisterBasedCharacterDisplayDriver(DisplayPins displayPins)
            : base(displayPins.TransferMode) {
            _displayPins = displayPins;
        }

        public bool BackLightEnabled { get; set; }

        public DisplayPins DisplayPins {
            get { return _displayPins; }
        }
    }
}