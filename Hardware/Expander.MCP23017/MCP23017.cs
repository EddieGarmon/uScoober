using System;
using uScoober.DataStructures;
using uScoober.Hardware.I2C;

namespace uScoober.Hardware.IO
{
    public class MCP23017 : I2CDeviceCore
    {
        public enum AddressPointer
        {
            Unchanged = -1,
            Increment = 0,
            DoesNotIncrement = 1
        }

        public enum AllDefaultValues
        {
            Low = 0,
            High = 1
        }

        public enum AllLogic
        {
            NonInverted = 0,
            AllInverted = 1
        }

        public enum AllOutputs
        {
            Low,
            High
        }

        public enum AllPins
        {
            Output = 0,
            Input = 1
        }

        public enum AllPorts
        {
            Disabled = 0,
            Enabled = 1
        }

        public enum AllPullUps
        {
            Disabled = 0,
            Enabled = 1
        }

        public enum AllTriggers
        {
            OnAnyEdge = 0,
            OnEdgeOppositeOfDefaultValue = 1
        }

        public enum BankInterruptPinState
        {
            Unchanged = -1,
            ActiveLow = 0,
            ActiveHigh = 1
        }

        public enum BankInterruptType
        {
            Unchanged = -1,
            ActiveDriver = 0, // tied to Vcc
            OpenDrain = 1 // sink to ground        
        }

        public enum ConnectionBetweenBankInterruptPins
        {
            Unchanged = -1,
            Seperated = 0,
            Connected = 1
        }

        public enum InputLogic
        {
            Unchanged = -1,
            Normal = 0,
            Inverted = 1
        }

        public enum InterruptPort
        {
            Unchanged = -1,
            Disable = 0,
            Enable = 1
        }

        public enum InterruptPortDefaultValue
        {
            Unchanged = -1,
            Low = 0,
            High = 1
        }

        public enum InterruptPortType
        {
            Unchanged = -1,
            OnAnyEdge = 0,
            OnEdgeOppositeOfDefaultValue = 1
        }

        public enum Pinstate
        {
            Unchanged = -1,
            Low = 0,
            High = 1
        }

        public enum PortType
        {
            Unchanged = -1,
            Output = 0,
            Input = 1
        }

        public enum PullUpResistor
        {
            Unchanged = -1,
            Disabled = 0,
            Enabled = 1
        }

        public enum SlewRate
        {
            Unchanged = -1,
            Enabled = 0,
            Disabled = 1
        }

        private const ushort DefaultAddress = 0x20; //0100XXX
        private const int DefaultClockRateKhz = 400; // 1 of three recommended frequencies on the datasheet
        private Outputs _output;

        public MCP23017(II2CBus bus, ushort address = DefaultAddress, int clockRateKhz = DefaultClockRateKhz)
            : base(bus, address, clockRateKhz) { }

        public Outputs Output {
            get { return _output ?? (_output = new Outputs()); }
        }

        public byte ReadPinsOnBankA() {
            return Read(Register.GPIO, Bank.A);
        }

        public int ReadPinsOnBankA(int pin) {
            byte ChipRead = Read(Register.GPIO, Bank.A);

            if (pin > 7 || pin < 0) {
                throw new Exception();
            }
            var DeterminingByte = (byte)((1 << pin) & ChipRead);
            if (DeterminingByte == 0) {
                return 0;
            }
            return 1;
        }

        public byte ReadPinsOnBankB() {
            return Read(Register.GPIO, Bank.B);
        }

        public int ReadPinsOnBankB(int pin) {
            byte ChipRead = Read(Register.GPIO, Bank.B);

            if (pin > 7 || pin < 0) {
                throw new Exception();
            }
            var DeterminingByte = (byte)((1 << pin) & ChipRead);
            if (DeterminingByte == 0) {
                return 0;
            }
            return 1;
        }

        public int ReadPinValuesCachedOnInterruptOnBankA(int pin) {
            byte ChipRead = Read(Register.InterruptCapturedValue, Bank.A);

            if (pin > 7 || pin < 0) {
                throw new Exception();
            }
            var DeterminingByte = (byte)((1 << pin) & ChipRead);
            if (DeterminingByte == 0) {
                return 0;
            }
            return 1;
        }

        public byte ReadPinValuesCachedOnInterruptOnBankA() {
            return Read(Register.InterruptCapturedValue, Bank.A);
        }

        public int ReadPinValuesCachedOnInterruptOnBankB(int pin) {
            byte ChipRead = Read(Register.InterruptCapturedValue, Bank.B);

            if (pin > 7 || pin < 0) {
                throw new Exception();
            }
            var DeterminingByte = (byte)((1 << pin) & ChipRead);
            if (DeterminingByte == 0) {
                return 0;
            }
            return 1;
        }

        public byte ReadPinValuesCachedOnInterruptOnBankB() {
            return Read(Register.InterruptCapturedValue, Bank.B);
        }

        public ushort ReadPinValuesCachedOnInterruptOnBothBanks() {
            return ReadBothBanks(Register.InterruptCapturedValue);
        }

        public int ReadWhichPinCausedInterruptOnBankA() {
            byte FlagRead = Read(Register.InterruptFlag, Bank.A);
            if (FlagRead == 0x80) {
                return 7;
            }
            if (FlagRead == 0x40) {
                return 6;
            }
            if (FlagRead == 0x20) {
                return 5;
            }
            if (FlagRead == 0x10) {
                return 4;
            }
            if (FlagRead == 0x08) {
                return 3;
            }
            if (FlagRead == 0x04) {
                return 2;
            }
            if (FlagRead == 0x02) {
                return 1;
            }
            if (FlagRead == 0x01) {
                return 0;
            }
            return -1;
        }

        public int ReadWhichPinCausedInterruptOnBankB() {
            byte FlagRead = Read(Register.InterruptFlag, Bank.B);
            if (FlagRead == 0x80) {
                return 7;
            }
            if (FlagRead == 0x40) {
                return 6;
            }
            if (FlagRead == 0x20) {
                return 5;
            }
            if (FlagRead == 0x10) {
                return 4;
            }
            if (FlagRead == 0x08) {
                return 3;
            }
            if (FlagRead == 0x04) {
                return 2;
            }
            if (FlagRead == 0x02) {
                return 1;
            }
            if (FlagRead == 0x01) {
                return 0;
            }
            return -1;
        }

        public void SetConfigurationOfIOSettingsOnBankA(ConnectionBetweenBankInterruptPins InterruptPinMirror = ConnectionBetweenBankInterruptPins.Unchanged,
                                                        AddressPointer SequentialOperation = AddressPointer.Unchanged,
                                                        SlewRate EnableSlewRate = SlewRate.Unchanged,
                                                        BankInterruptType InterruptConfiguration = BankInterruptType.Unchanged,
                                                        BankInterruptPinState InterruptPolarity = BankInterruptPinState.Unchanged) {
            int mask = Read(Register.IOCON, Bank.A);

            switch (InterruptPolarity) {
                case BankInterruptPinState.Unchanged:
                    break;
                case BankInterruptPinState.ActiveHigh:
                    mask = mask | 0x02;
                    break;
                case BankInterruptPinState.ActiveLow:
                    mask = mask & 0xFD;
                    break;
            }
            switch (InterruptConfiguration) {
                case BankInterruptType.Unchanged:
                    break;
                case BankInterruptType.OpenDrain:
                    mask = mask | 0x04;
                    break;
                case BankInterruptType.ActiveDriver:
                    mask = mask & 0xFB;
                    break;
            }
            switch (EnableSlewRate) {
                case SlewRate.Unchanged:
                    break;
                case SlewRate.Disabled:
                    mask = mask | 0x10;
                    break;
                case SlewRate.Enabled:
                    mask = mask & 0xEF;
                    break;
            }
            switch (SequentialOperation) {
                case AddressPointer.Unchanged:
                    break;
                case AddressPointer.DoesNotIncrement:
                    mask = mask | 0x20;
                    break;
                case AddressPointer.Increment:
                    mask = mask & 0xDF;
                    break;
            }
            switch (InterruptPinMirror) {
                case ConnectionBetweenBankInterruptPins.Unchanged:
                    break;
                case ConnectionBetweenBankInterruptPins.Connected:
                    mask = mask | 0x40;
                    break;
                case ConnectionBetweenBankInterruptPins.Seperated:
                    mask = mask & 0xBF;
                    break;
            }

            Write(Register.IOCON, Bank.A, (byte)mask);
        }

        public void SetConfigurationOfIOSettingsOnBankB(ConnectionBetweenBankInterruptPins InterruptPinMirror = ConnectionBetweenBankInterruptPins.Unchanged,
                                                        AddressPointer SequentialOperation = AddressPointer.Unchanged,
                                                        SlewRate EnableSlewRate = SlewRate.Unchanged,
                                                        BankInterruptType InterruptConfiguration = BankInterruptType.Unchanged,
                                                        BankInterruptPinState InterruptPolarity = BankInterruptPinState.Unchanged) {
            int mask = Read(Register.IOCON, Bank.B);

            switch (InterruptPolarity) {
                case BankInterruptPinState.Unchanged:
                    break;
                case BankInterruptPinState.ActiveHigh:
                    mask = mask | 0x02;
                    break;
                case BankInterruptPinState.ActiveLow:
                    mask = mask & 0xFD;
                    break;
            }
            switch (InterruptConfiguration) {
                case BankInterruptType.Unchanged:
                    break;
                case BankInterruptType.OpenDrain:
                    mask = mask | 0x04;
                    break;
                case BankInterruptType.ActiveDriver:
                    mask = mask & 0xFB;
                    break;
            }
            switch (EnableSlewRate) {
                case SlewRate.Unchanged:
                    break;
                case SlewRate.Disabled:
                    mask = mask | 0x10;
                    break;
                case SlewRate.Enabled:
                    mask = mask & 0xEF;
                    break;
            }
            switch (SequentialOperation) {
                case AddressPointer.Unchanged:
                    break;
                case AddressPointer.DoesNotIncrement:
                    mask = mask | 0x20;
                    break;
                case AddressPointer.Increment:
                    mask = mask & 0xDF;
                    break;
            }
            switch (InterruptPinMirror) {
                case ConnectionBetweenBankInterruptPins.Unchanged:
                    break;
                case ConnectionBetweenBankInterruptPins.Connected:
                    mask = mask | 0x40;
                    break;
                case ConnectionBetweenBankInterruptPins.Seperated:
                    mask = mask & 0xBF;
                    break;
            }

            Write(Register.IOCON, Bank.B, (byte)mask);
        }

        public void SetConnectionOfChipsINTPins(ConnectionBetweenBankInterruptPins Connection) {
            int bit = 6;
            byte currentState = Read(Register.IOCON, Bank.A);
            bool state = ((int)Connection == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(bit, state, currentState);
            Write(Register.IOCON, Bank.A, ValueToWrite);
        }

        public void SetDefaultValuesOfInterruptPortsOnBankA(byte values) {
            Write(Register.PortDefaultValue, Bank.A, values);
        }

        public void SetDefaultValuesOfInterruptPortsOnBankA(InterruptPortDefaultValue Pin0 = InterruptPortDefaultValue.Unchanged,
                                                            InterruptPortDefaultValue Pin1 = InterruptPortDefaultValue.Unchanged,
                                                            InterruptPortDefaultValue Pin2 = InterruptPortDefaultValue.Unchanged,
                                                            InterruptPortDefaultValue Pin3 = InterruptPortDefaultValue.Unchanged,
                                                            InterruptPortDefaultValue Pin4 = InterruptPortDefaultValue.Unchanged,
                                                            InterruptPortDefaultValue Pin5 = InterruptPortDefaultValue.Unchanged,
                                                            InterruptPortDefaultValue Pin6 = InterruptPortDefaultValue.Unchanged,
                                                            InterruptPortDefaultValue Pin7 = InterruptPortDefaultValue.Unchanged) {
            int mask = Read(Register.PortDefaultValue, Bank.A);

            switch (Pin0) {
                case InterruptPortDefaultValue.Unchanged:
                    break;
                case InterruptPortDefaultValue.High:
                    mask = mask | 0x01;
                    break;
                case InterruptPortDefaultValue.Low:
                    mask = mask & 0xFE;
                    break;
            }
            switch (Pin1) {
                case InterruptPortDefaultValue.Unchanged:
                    break;
                case InterruptPortDefaultValue.High:
                    mask = mask | 0x02;
                    break;
                case InterruptPortDefaultValue.Low:
                    mask = mask & 0xFD;
                    break;
            }
            switch (Pin2) {
                case InterruptPortDefaultValue.Unchanged:
                    break;
                case InterruptPortDefaultValue.High:
                    mask = mask | 0x04;
                    break;
                case InterruptPortDefaultValue.Low:
                    mask = mask & 0xFB;
                    break;
            }
            switch (Pin3) {
                case InterruptPortDefaultValue.Unchanged:
                    break;
                case InterruptPortDefaultValue.High:
                    mask = mask | 0x08;
                    break;
                case InterruptPortDefaultValue.Low:
                    mask = mask & 0xF7;
                    break;
            }
            switch (Pin4) {
                case InterruptPortDefaultValue.Unchanged:
                    break;
                case InterruptPortDefaultValue.High:
                    mask = mask | 0x10;
                    break;
                case InterruptPortDefaultValue.Low:
                    mask = mask & 0xEF;
                    break;
            }
            switch (Pin5) {
                case InterruptPortDefaultValue.Unchanged:
                    break;
                case InterruptPortDefaultValue.High:
                    mask = mask | 0x20;
                    break;
                case InterruptPortDefaultValue.Low:
                    mask = mask & 0xDF;
                    break;
            }
            switch (Pin6) {
                case InterruptPortDefaultValue.Unchanged:
                    break;
                case InterruptPortDefaultValue.High:
                    mask = mask | 0x40;
                    break;
                case InterruptPortDefaultValue.Low:
                    mask = mask & 0xBF;
                    break;
            }
            switch (Pin7) {
                case InterruptPortDefaultValue.Unchanged:
                    break;
                case InterruptPortDefaultValue.High:
                    mask = mask | 0x80;
                    break;
                case InterruptPortDefaultValue.Low:
                    mask = mask & 0x7F;
                    break;
            }
            Write(Register.PortDefaultValue, Bank.A, (byte)mask);
        }

        public void SetDefaultValuesOfInterruptPortsOnBankA(AllDefaultValues bank) {
            if (bank == AllDefaultValues.High) {
                Write(Register.PortDefaultValue, Bank.A, 0xFF);
            }
            else {
                Write(Register.PortDefaultValue, Bank.A, 0x00);
            }
        }

        public void SetDefaultValuesOfInterruptPortsOnBankA(int pin, InterruptPortDefaultValue configuration) {
            byte currentState = Read(Register.PortDefaultValue, Bank.A);
            bool state = ((int)configuration == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.PortDefaultValue, Bank.A, ValueToWrite);
        }

        public void SetDefaultValuesOfInterruptPortsOnBankB(byte values) {
            Write(Register.PortDefaultValue, Bank.B, values);
        }

        public void SetDefaultValuesOfInterruptPortsOnBankB(InterruptPortDefaultValue Pin0 = InterruptPortDefaultValue.Unchanged,
                                                            InterruptPortDefaultValue Pin1 = InterruptPortDefaultValue.Unchanged,
                                                            InterruptPortDefaultValue Pin2 = InterruptPortDefaultValue.Unchanged,
                                                            InterruptPortDefaultValue Pin3 = InterruptPortDefaultValue.Unchanged,
                                                            InterruptPortDefaultValue Pin4 = InterruptPortDefaultValue.Unchanged,
                                                            InterruptPortDefaultValue Pin5 = InterruptPortDefaultValue.Unchanged,
                                                            InterruptPortDefaultValue Pin6 = InterruptPortDefaultValue.Unchanged,
                                                            InterruptPortDefaultValue Pin7 = InterruptPortDefaultValue.Unchanged) {
            int mask = Read(Register.PortDefaultValue, Bank.B);

            switch (Pin0) {
                case InterruptPortDefaultValue.Unchanged:
                    break;
                case InterruptPortDefaultValue.High:
                    mask = mask | 0x01;
                    break;
                case InterruptPortDefaultValue.Low:
                    mask = mask & 0xFE;
                    break;
            }
            switch (Pin1) {
                case InterruptPortDefaultValue.Unchanged:
                    break;
                case InterruptPortDefaultValue.High:
                    mask = mask | 0x02;
                    break;
                case InterruptPortDefaultValue.Low:
                    mask = mask & 0xFD;
                    break;
            }
            switch (Pin2) {
                case InterruptPortDefaultValue.Unchanged:
                    break;
                case InterruptPortDefaultValue.High:
                    mask = mask | 0x04;
                    break;
                case InterruptPortDefaultValue.Low:
                    mask = mask & 0xFB;
                    break;
            }
            switch (Pin3) {
                case InterruptPortDefaultValue.Unchanged:
                    break;
                case InterruptPortDefaultValue.High:
                    mask = mask | 0x08;
                    break;
                case InterruptPortDefaultValue.Low:
                    mask = mask & 0xF7;
                    break;
            }
            switch (Pin4) {
                case InterruptPortDefaultValue.Unchanged:
                    break;
                case InterruptPortDefaultValue.High:
                    mask = mask | 0x10;
                    break;
                case InterruptPortDefaultValue.Low:
                    mask = mask & 0xEF;
                    break;
            }
            switch (Pin5) {
                case InterruptPortDefaultValue.Unchanged:
                    break;
                case InterruptPortDefaultValue.High:
                    mask = mask | 0x20;
                    break;
                case InterruptPortDefaultValue.Low:
                    mask = mask & 0xDF;
                    break;
            }
            switch (Pin6) {
                case InterruptPortDefaultValue.Unchanged:
                    break;
                case InterruptPortDefaultValue.High:
                    mask = mask | 0x40;
                    break;
                case InterruptPortDefaultValue.Low:
                    mask = mask & 0xBF;
                    break;
            }
            switch (Pin7) {
                case InterruptPortDefaultValue.Unchanged:
                    break;
                case InterruptPortDefaultValue.High:
                    mask = mask | 0x80;
                    break;
                case InterruptPortDefaultValue.Low:
                    mask = mask & 0x7F;
                    break;
            }
            Write(Register.PortDefaultValue, Bank.B, (byte)mask);
        }

        public void SetDefaultValuesOfInterruptPortsOnBankB(AllDefaultValues bank) {
            if (bank == AllDefaultValues.High) {
                Write(Register.PortDefaultValue, Bank.B, 0xFF);
            }
            else {
                Write(Register.PortDefaultValue, Bank.B, 0x00);
            }
        }

        public void SetDefaultValuesOfInterruptPortsOnBankB(int pin, InterruptPortDefaultValue configuration) {
            byte currentState = Read(Register.PortDefaultValue, Bank.B);
            bool state = ((int)configuration == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.PortDefaultValue, Bank.B, ValueToWrite);
        }

        public void SetEdgeTriggersForInterruptsOnBankA(byte values) {
            Write(Register.InterruptTypeControl, Bank.A, values);
        }

        public void SetEdgeTriggersForInterruptsOnBankA(InterruptPortType Pin0 = InterruptPortType.Unchanged,
                                                        InterruptPortType Pin1 = InterruptPortType.Unchanged,
                                                        InterruptPortType Pin2 = InterruptPortType.Unchanged,
                                                        InterruptPortType Pin3 = InterruptPortType.Unchanged,
                                                        InterruptPortType Pin4 = InterruptPortType.Unchanged,
                                                        InterruptPortType Pin5 = InterruptPortType.Unchanged,
                                                        InterruptPortType Pin6 = InterruptPortType.Unchanged,
                                                        InterruptPortType Pin7 = InterruptPortType.Unchanged) {
            int mask = Read(Register.InterruptTypeControl, Bank.A);

            switch (Pin0) {
                case InterruptPortType.Unchanged:
                    break;
                case InterruptPortType.OnEdgeOppositeOfDefaultValue:
                    mask = mask | 0x01;
                    break;
                case InterruptPortType.OnAnyEdge:
                    mask = mask & 0xFE;
                    break;
            }
            switch (Pin1) {
                case InterruptPortType.Unchanged:
                    break;
                case InterruptPortType.OnEdgeOppositeOfDefaultValue:
                    mask = mask | 0x02;
                    break;
                case InterruptPortType.OnAnyEdge:
                    mask = mask & 0xFD;
                    break;
            }
            switch (Pin2) {
                case InterruptPortType.Unchanged:
                    break;
                case InterruptPortType.OnEdgeOppositeOfDefaultValue:
                    mask = mask | 0x04;
                    break;
                case InterruptPortType.OnAnyEdge:
                    mask = mask & 0xFB;
                    break;
            }
            switch (Pin3) {
                case InterruptPortType.Unchanged:
                    break;
                case InterruptPortType.OnEdgeOppositeOfDefaultValue:
                    mask = mask | 0x08;
                    break;
                case InterruptPortType.OnAnyEdge:
                    mask = mask & 0xF7;
                    break;
            }
            switch (Pin4) {
                case InterruptPortType.Unchanged:
                    break;
                case InterruptPortType.OnEdgeOppositeOfDefaultValue:
                    mask = mask | 0x10;
                    break;
                case InterruptPortType.OnAnyEdge:
                    mask = mask & 0xEF;
                    break;
            }
            switch (Pin5) {
                case InterruptPortType.Unchanged:
                    break;
                case InterruptPortType.OnEdgeOppositeOfDefaultValue:
                    mask = mask | 0x20;
                    break;
                case InterruptPortType.OnAnyEdge:
                    mask = mask & 0xDF;
                    break;
            }
            switch (Pin6) {
                case InterruptPortType.Unchanged:
                    break;
                case InterruptPortType.OnEdgeOppositeOfDefaultValue:
                    mask = mask | 0x40;
                    break;
                case InterruptPortType.OnAnyEdge:
                    mask = mask & 0xBF;
                    break;
            }
            switch (Pin7) {
                case InterruptPortType.Unchanged:
                    break;
                case InterruptPortType.OnEdgeOppositeOfDefaultValue:
                    mask = mask | 0x80;
                    break;
                case InterruptPortType.OnAnyEdge:
                    mask = mask & 0x7F;
                    break;
            }
            Write(Register.InterruptTypeControl, Bank.A, (byte)mask);
        }

        public void SetEdgeTriggersForInterruptsOnBankA(AllTriggers bank) {
            if (bank == AllTriggers.OnEdgeOppositeOfDefaultValue) {
                Write(Register.PortDefaultValue, Bank.A, 0xFF);
            }
            else {
                Write(Register.PortDefaultValue, Bank.A, 0x00);
            }
        }

        public void SetEdgeTriggersForInterruptsOnBankA(int pin, InterruptPortType edge) {
            byte currentState = Read(Register.InterruptTypeControl, Bank.A);
            bool state = ((int)edge == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.InterruptTypeControl, Bank.A, ValueToWrite);
        }

        public void SetEdgeTriggersForInterruptsOnBankB(byte values) {
            Write(Register.InterruptTypeControl, Bank.B, values);
        }

        public void SetEdgeTriggersForInterruptsOnBankB(InterruptPortType Pin0 = InterruptPortType.Unchanged,
                                                        InterruptPortType Pin1 = InterruptPortType.Unchanged,
                                                        InterruptPortType Pin2 = InterruptPortType.Unchanged,
                                                        InterruptPortType Pin3 = InterruptPortType.Unchanged,
                                                        InterruptPortType Pin4 = InterruptPortType.Unchanged,
                                                        InterruptPortType Pin5 = InterruptPortType.Unchanged,
                                                        InterruptPortType Pin6 = InterruptPortType.Unchanged,
                                                        InterruptPortType Pin7 = InterruptPortType.Unchanged) {
            int mask = Read(Register.InterruptTypeControl, Bank.B);

            switch (Pin0) {
                case InterruptPortType.Unchanged:
                    break;
                case InterruptPortType.OnEdgeOppositeOfDefaultValue:
                    mask = mask | 0x01;
                    break;
                case InterruptPortType.OnAnyEdge:
                    mask = mask & 0xFE;
                    break;
            }
            switch (Pin1) {
                case InterruptPortType.Unchanged:
                    break;
                case InterruptPortType.OnEdgeOppositeOfDefaultValue:
                    mask = mask | 0x02;
                    break;
                case InterruptPortType.OnAnyEdge:
                    mask = mask & 0xFD;
                    break;
            }
            switch (Pin2) {
                case InterruptPortType.Unchanged:
                    break;
                case InterruptPortType.OnEdgeOppositeOfDefaultValue:
                    mask = mask | 0x04;
                    break;
                case InterruptPortType.OnAnyEdge:
                    mask = mask & 0xFB;
                    break;
            }
            switch (Pin3) {
                case InterruptPortType.Unchanged:
                    break;
                case InterruptPortType.OnEdgeOppositeOfDefaultValue:
                    mask = mask | 0x08;
                    break;
                case InterruptPortType.OnAnyEdge:
                    mask = mask & 0xF7;
                    break;
            }
            switch (Pin4) {
                case InterruptPortType.Unchanged:
                    break;
                case InterruptPortType.OnEdgeOppositeOfDefaultValue:
                    mask = mask | 0x10;
                    break;
                case InterruptPortType.OnAnyEdge:
                    mask = mask & 0xEF;
                    break;
            }
            switch (Pin5) {
                case InterruptPortType.Unchanged:
                    break;
                case InterruptPortType.OnEdgeOppositeOfDefaultValue:
                    mask = mask | 0x20;
                    break;
                case InterruptPortType.OnAnyEdge:
                    mask = mask & 0xDF;
                    break;
            }
            switch (Pin6) {
                case InterruptPortType.Unchanged:
                    break;
                case InterruptPortType.OnEdgeOppositeOfDefaultValue:
                    mask = mask | 0x40;
                    break;
                case InterruptPortType.OnAnyEdge:
                    mask = mask & 0xBF;
                    break;
            }
            switch (Pin7) {
                case InterruptPortType.Unchanged:
                    break;
                case InterruptPortType.OnEdgeOppositeOfDefaultValue:
                    mask = mask | 0x80;
                    break;
                case InterruptPortType.OnAnyEdge:
                    mask = mask & 0x7F;
                    break;
            }
            Write(Register.InterruptTypeControl, Bank.B, (byte)mask);
        }

        public void SetEdgeTriggersForInterruptsOnBankB(AllTriggers bank) {
            if (bank == AllTriggers.OnEdgeOppositeOfDefaultValue) {
                Write(Register.PortDefaultValue, Bank.B, 0xFF);
            }
            else {
                Write(Register.PortDefaultValue, Bank.B, 0x00);
            }
        }

        public void SetEdgeTriggersForInterruptsOnBankB(int pin, InterruptPortType edge) {
            byte currentState = Read(Register.InterruptTypeControl, Bank.B);
            bool state = ((int)edge == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.InterruptTypeControl, Bank.B, ValueToWrite);
        }

        public void SetInterruptPortsOnBankA(byte values) {
            Write(Register.InterruptEnable, Bank.A, values);
        }

        public void SetInterruptPortsOnBankA(InterruptPort Pin0 = InterruptPort.Unchanged,
                                             InterruptPort Pin1 = InterruptPort.Unchanged,
                                             InterruptPort Pin2 = InterruptPort.Unchanged,
                                             InterruptPort Pin3 = InterruptPort.Unchanged,
                                             InterruptPort Pin4 = InterruptPort.Unchanged,
                                             InterruptPort Pin5 = InterruptPort.Unchanged,
                                             InterruptPort Pin6 = InterruptPort.Unchanged,
                                             InterruptPort Pin7 = InterruptPort.Unchanged) {
            int mask = Read(Register.InterruptEnable, Bank.A);

            switch (Pin0) {
                case InterruptPort.Unchanged:
                    break;
                case InterruptPort.Enable:
                    mask = mask | 0x01;
                    break;
                case InterruptPort.Disable:
                    mask = mask & 0xFE;
                    break;
            }
            switch (Pin1) {
                case InterruptPort.Unchanged:
                    break;
                case InterruptPort.Enable:
                    mask = mask | 0x02;
                    break;
                case InterruptPort.Disable:
                    mask = mask & 0xFD;
                    break;
            }
            switch (Pin2) {
                case InterruptPort.Unchanged:
                    break;
                case InterruptPort.Enable:
                    mask = mask | 0x04;
                    break;
                case InterruptPort.Disable:
                    mask = mask & 0xFB;
                    break;
            }
            switch (Pin3) {
                case InterruptPort.Unchanged:
                    break;
                case InterruptPort.Enable:
                    mask = mask | 0x08;
                    break;
                case InterruptPort.Disable:
                    mask = mask & 0xF7;
                    break;
            }
            switch (Pin4) {
                case InterruptPort.Unchanged:
                    break;
                case InterruptPort.Enable:
                    mask = mask | 0x10;
                    break;
                case InterruptPort.Disable:
                    mask = mask & 0xEF;
                    break;
            }
            switch (Pin5) {
                case InterruptPort.Unchanged:
                    break;
                case InterruptPort.Enable:
                    mask = mask | 0x20;
                    break;
                case InterruptPort.Disable:
                    mask = mask & 0xDF;
                    break;
            }
            switch (Pin6) {
                case InterruptPort.Unchanged:
                    break;
                case InterruptPort.Enable:
                    mask = mask | 0x40;
                    break;
                case InterruptPort.Disable:
                    mask = mask & 0xBF;
                    break;
            }
            switch (Pin7) {
                case InterruptPort.Unchanged:
                    break;
                case InterruptPort.Enable:
                    mask = mask | 0x80;
                    break;
                case InterruptPort.Disable:
                    mask = mask & 0x7F;
                    break;
            }
            Write(Register.InterruptEnable, Bank.A, (byte)mask);
        }

        public void SetInterruptPortsOnBankA(AllPorts bank) {
            if (bank == AllPorts.Enabled) {
                Write(Register.InterruptEnable, Bank.A, 0xFF);
            }
            else {
                Write(Register.InterruptEnable, Bank.A, 0x00);
            }
        }

        public void SetInterruptPortsOnBankA(int pin, InterruptPort InterruptPort) {
            byte currentState = Read(Register.InterruptEnable, Bank.A);
            bool state = ((int)InterruptPort == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.InterruptEnable, Bank.A, ValueToWrite);
        }

        public void SetInterruptPortsOnBankB(byte values) {
            Write(Register.InterruptEnable, Bank.B, values);
        }

        public void SetInterruptPortsOnBankB(InterruptPort Pin0 = InterruptPort.Unchanged,
                                             InterruptPort Pin1 = InterruptPort.Unchanged,
                                             InterruptPort Pin2 = InterruptPort.Unchanged,
                                             InterruptPort Pin3 = InterruptPort.Unchanged,
                                             InterruptPort Pin4 = InterruptPort.Unchanged,
                                             InterruptPort Pin5 = InterruptPort.Unchanged,
                                             InterruptPort Pin6 = InterruptPort.Unchanged,
                                             InterruptPort Pin7 = InterruptPort.Unchanged) {
            int mask = Read(Register.InterruptEnable, Bank.B);

            switch (Pin0) {
                case InterruptPort.Unchanged:
                    break;
                case InterruptPort.Enable:
                    mask = mask | 0x01;
                    break;
                case InterruptPort.Disable:
                    mask = mask & 0xFE;
                    break;
            }
            switch (Pin1) {
                case InterruptPort.Unchanged:
                    break;
                case InterruptPort.Enable:
                    mask = mask | 0x02;
                    break;
                case InterruptPort.Disable:
                    mask = mask & 0xFD;
                    break;
            }
            switch (Pin2) {
                case InterruptPort.Unchanged:
                    break;
                case InterruptPort.Enable:
                    mask = mask | 0x04;
                    break;
                case InterruptPort.Disable:
                    mask = mask & 0xFB;
                    break;
            }
            switch (Pin3) {
                case InterruptPort.Unchanged:
                    break;
                case InterruptPort.Enable:
                    mask = mask | 0x08;
                    break;
                case InterruptPort.Disable:
                    mask = mask & 0xF7;
                    break;
            }
            switch (Pin4) {
                case InterruptPort.Unchanged:
                    break;
                case InterruptPort.Enable:
                    mask = mask | 0x10;
                    break;
                case InterruptPort.Disable:
                    mask = mask & 0xEF;
                    break;
            }
            switch (Pin5) {
                case InterruptPort.Unchanged:
                    break;
                case InterruptPort.Enable:
                    mask = mask | 0x20;
                    break;
                case InterruptPort.Disable:
                    mask = mask & 0xDF;
                    break;
            }
            switch (Pin6) {
                case InterruptPort.Unchanged:
                    break;
                case InterruptPort.Enable:
                    mask = mask | 0x40;
                    break;
                case InterruptPort.Disable:
                    mask = mask & 0xBF;
                    break;
            }
            switch (Pin7) {
                case InterruptPort.Unchanged:
                    break;
                case InterruptPort.Enable:
                    mask = mask | 0x80;
                    break;
                case InterruptPort.Disable:
                    mask = mask & 0x7F;
                    break;
            }
            Write(Register.InterruptEnable, Bank.B, (byte)mask);
        }

        public void SetInterruptPortsOnBankB(AllPorts bank) {
            if (bank == AllPorts.Enabled) {
                Write(Register.InterruptEnable, Bank.B, 0xFF);
            }
            else {
                Write(Register.InterruptEnable, Bank.B, 0x00);
            }
        }

        public void SetInterruptPortsOnBankB(int pin, InterruptPort InterruptPort) {
            byte currentState = Read(Register.InterruptEnable, Bank.B);
            bool state = ((int)InterruptPort == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.InterruptEnable, Bank.B, ValueToWrite);
        }

        public void SetINTPinAsOpenDrainOnBankA(BankInterruptType Configuration) {
            int pin = 2;
            byte currentState = Read(Register.IOCON, Bank.A);
            bool state = ((int)Configuration == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.IOCON, Bank.A, ValueToWrite);
        }

        public void SetINTPinAsOpenDrainOnBankB(BankInterruptType Configuration) {
            int pin = 2;
            byte currentState = Read(Register.IOCON, Bank.B);
            bool state = ((int)Configuration == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.IOCON, Bank.B, ValueToWrite);
        }

        public void SetINTPinPolarityOnBankA(BankInterruptPinState Polarity) {
            int pin = 1;
            byte currentState = Read(Register.IOCON, Bank.A);
            bool state = ((int)Polarity == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.IOCON, Bank.A, ValueToWrite);
        }

        public void SetINTPinPolarityOnBankB(BankInterruptPinState Polarity) {
            int pin = 1;
            byte currentState = Read(Register.IOCON, Bank.B);
            bool state = ((int)Polarity == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.IOCON, Bank.B, ValueToWrite);
        }

        public void SetIOPinDirectionOnBankA(byte values) {
            Write(Register.IODirection, Bank.A, values);
        }

        public void SetIOPinDirectionOnBankA(PortType Pin0 = PortType.Unchanged,
                                             PortType Pin1 = PortType.Unchanged,
                                             PortType Pin2 = PortType.Unchanged,
                                             PortType Pin3 = PortType.Unchanged,
                                             PortType Pin4 = PortType.Unchanged,
                                             PortType Pin5 = PortType.Unchanged,
                                             PortType Pin6 = PortType.Unchanged,
                                             PortType Pin7 = PortType.Unchanged) {
            int mask = Read(Register.IODirection, Bank.A);

            switch (Pin0) {
                case PortType.Unchanged:
                    break;
                case PortType.Input:
                    mask = mask | 0x01;
                    break;
                case PortType.Output:
                    mask = mask | 0xFE;
                    break;
            }
            switch (Pin1) {
                case PortType.Unchanged:
                    break;
                case PortType.Input:
                    mask = mask | 0x02;
                    break;
                case PortType.Output:
                    mask = mask | 0xFD;
                    break;
            }
            switch (Pin2) {
                case PortType.Unchanged:
                    break;
                case PortType.Input:
                    mask = mask | 0x04;
                    break;
                case PortType.Output:
                    mask = mask | 0xFB;
                    break;
            }
            switch (Pin3) {
                case PortType.Unchanged:
                    break;
                case PortType.Input:
                    mask = mask | 0x08;
                    break;
                case PortType.Output:
                    mask = mask | 0xF7;
                    break;
            }
            switch (Pin4) {
                case PortType.Unchanged:
                    break;
                case PortType.Input:
                    mask = mask | 0x10;
                    break;
                case PortType.Output:
                    mask = mask | 0xEF;
                    break;
            }
            switch (Pin5) {
                case PortType.Unchanged:
                    break;
                case PortType.Input:
                    mask = mask | 0x20;
                    break;
                case PortType.Output:
                    mask = mask | 0xDF;
                    break;
            }
            switch (Pin6) {
                case PortType.Unchanged:
                    break;
                case PortType.Input:
                    mask = mask | 0x40;
                    break;
                case PortType.Output:
                    mask = mask | 0xBF;
                    break;
            }
            switch (Pin7) {
                case PortType.Unchanged:
                    break;
                case PortType.Input:
                    mask = mask | 0x80;
                    break;
                case PortType.Output:
                    mask = mask | 0x7F;
                    break;
            }

            Write(Register.IODirection, Bank.A, (byte)mask);
        }

        public void SetIOPinDirectionOnBankA(AllPins bank) {
            if (bank == AllPins.Input) {
                Write(Register.IODirection, Bank.A, 0xFF);
            }
            else {
                Write(Register.IODirection, Bank.A, 0x00);
            }
        }

        public void SetIOPinDirectionOnBankA(int pin, PortType PortDirection) {
            byte currentState = Read(Register.IODirection, Bank.A);
            bool state = ((int)PortDirection == 1) ? (true) : (false);
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.IODirection, Bank.A, ValueToWrite);
        }

        public void SetIOPinDirectionOnBankB(byte values) {
            Write(Register.IODirection, Bank.B, values);
        }

        public void SetIOPinDirectionOnBankB(PortType Pin0 = PortType.Unchanged,
                                             PortType Pin1 = PortType.Unchanged,
                                             PortType Pin2 = PortType.Unchanged,
                                             PortType Pin3 = PortType.Unchanged,
                                             PortType Pin4 = PortType.Unchanged,
                                             PortType Pin5 = PortType.Unchanged,
                                             PortType Pin6 = PortType.Unchanged,
                                             PortType Pin7 = PortType.Unchanged) {
            int mask = Read(Register.IODirection, Bank.B);

            switch (Pin0) {
                case PortType.Unchanged:
                    break;
                case PortType.Input:
                    mask = mask | 0x01;
                    break;
                case PortType.Output:
                    mask = mask & 0xFE;
                    break;
            }
            switch (Pin1) {
                case PortType.Unchanged:
                    break;
                case PortType.Input:
                    mask = mask | 0x02;
                    break;
                case PortType.Output:
                    mask = mask & 0xFD;
                    break;
            }
            switch (Pin2) {
                case PortType.Unchanged:
                    break;
                case PortType.Input:
                    mask = mask | 0x04;
                    break;
                case PortType.Output:
                    mask = mask & 0xFB;
                    break;
            }
            switch (Pin3) {
                case PortType.Unchanged:
                    break;
                case PortType.Input:
                    mask = mask | 0x08;
                    break;
                case PortType.Output:
                    mask = mask & 0xF7;
                    break;
            }
            switch (Pin4) {
                case PortType.Unchanged:
                    break;
                case PortType.Input:
                    mask = mask | 0x10;
                    break;
                case PortType.Output:
                    mask = mask & 0xEF;
                    break;
            }
            switch (Pin5) {
                case PortType.Unchanged:
                    break;
                case PortType.Input:
                    mask = mask | 0x20;
                    break;
                case PortType.Output:
                    mask = mask & 0xDF;
                    break;
            }
            switch (Pin6) {
                case PortType.Unchanged:
                    break;
                case PortType.Input:
                    mask = mask | 0x40;
                    break;
                case PortType.Output:
                    mask = mask & 0xBF;
                    break;
            }
            switch (Pin7) {
                case PortType.Unchanged:
                    break;
                case PortType.Input:
                    mask = mask | 0x80;
                    break;
                case PortType.Output:
                    mask = mask & 0x7F;
                    break;
            }
            Write(Register.IODirection, Bank.B, (byte)mask);
        }

        public void SetIOPinDirectionOnBankB(AllPins bank) {
            if (bank == AllPins.Input) {
                Write(Register.IODirection, Bank.B, 0xFF);
            }
            else {
                Write(Register.IODirection, Bank.B, 0x00);
            }
        }

        public void SetIOPinDirectionOnBankB(int pin, PortType PortDirection) {
            byte currentState = Read(Register.IODirection, Bank.B);
            bool state = ((int)PortDirection == 1) ? (true) : (false);
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.IODirection, Bank.B, ValueToWrite);
        }

        public void SetPullUpResistorsOnBankA(byte pinout) {
            Write(Register.PullUP, Bank.A, pinout);
        }

        public void SetPullUpResistorsOnBankA(PullUpResistor Pin0 = PullUpResistor.Unchanged,
                                              PullUpResistor Pin1 = PullUpResistor.Unchanged,
                                              PullUpResistor Pin2 = PullUpResistor.Unchanged,
                                              PullUpResistor Pin3 = PullUpResistor.Unchanged,
                                              PullUpResistor Pin4 = PullUpResistor.Unchanged,
                                              PullUpResistor Pin5 = PullUpResistor.Unchanged,
                                              PullUpResistor Pin6 = PullUpResistor.Unchanged,
                                              PullUpResistor Pin7 = PullUpResistor.Unchanged) {
            int mask = Read(Register.PullUP, Bank.A);

            switch (Pin0) {
                case PullUpResistor.Unchanged:
                    break;
                case PullUpResistor.Enabled:
                    mask = mask | 0x01;
                    break;
                case PullUpResistor.Disabled:
                    mask = mask & 0xFE;
                    break;
            }
            switch (Pin1) {
                case PullUpResistor.Unchanged:
                    break;
                case PullUpResistor.Enabled:
                    mask = mask | 0x02;
                    break;
                case PullUpResistor.Disabled:
                    mask = mask & 0xFD;
                    break;
            }
            switch (Pin2) {
                case PullUpResistor.Unchanged:
                    break;
                case PullUpResistor.Enabled:
                    mask = mask | 0x04;
                    break;
                case PullUpResistor.Disabled:
                    mask = mask & 0xFB;
                    break;
            }
            switch (Pin3) {
                case PullUpResistor.Unchanged:
                    break;
                case PullUpResistor.Enabled:
                    mask = mask | 0x08;
                    break;
                case PullUpResistor.Disabled:
                    mask = mask & 0xF7;
                    break;
            }
            switch (Pin4) {
                case PullUpResistor.Unchanged:
                    break;
                case PullUpResistor.Enabled:
                    mask = mask | 0x10;
                    break;
                case PullUpResistor.Disabled:
                    mask = mask & 0xEF;
                    break;
            }
            switch (Pin5) {
                case PullUpResistor.Unchanged:
                    break;
                case PullUpResistor.Enabled:
                    mask = mask | 0x20;
                    break;
                case PullUpResistor.Disabled:
                    mask = mask & 0xDF;
                    break;
            }
            switch (Pin6) {
                case PullUpResistor.Unchanged:
                    break;
                case PullUpResistor.Enabled:
                    mask = mask | 0x40;
                    break;
                case PullUpResistor.Disabled:
                    mask = mask & 0xBF;
                    break;
            }
            switch (Pin7) {
                case PullUpResistor.Unchanged:
                    break;
                case PullUpResistor.Enabled:
                    mask = mask | 0x80;
                    break;
                case PullUpResistor.Disabled:
                    mask = mask & 0x7F;
                    break;
            }
            Write(Register.PullUP, Bank.A, (byte)mask);
        }

        public void SetPullUpResistorsOnBankA(AllPullUps bank) {
            if (bank == AllPullUps.Enabled) {
                Write(Register.PullUP, Bank.A, 0xFF);
            }
            else {
                Write(Register.PullUP, Bank.A, 0x00);
            }
        }

        public void SetPullUpResistorsOnBankA(int pin, PullUpResistor resistor) //pin number <= 7
        {
            byte currentState = Read(Register.PullUP, Bank.A);
            bool state = ((int)resistor == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.PullUP, Bank.A, ValueToWrite);
        }

        public void SetPullUpResistorsOnBankB(byte pinout) {
            Write(Register.PullUP, Bank.B, pinout);
        }

        public void SetPullUpResistorsOnBankB(PullUpResistor Pin0 = PullUpResistor.Unchanged,
                                              PullUpResistor Pin1 = PullUpResistor.Unchanged,
                                              PullUpResistor Pin2 = PullUpResistor.Unchanged,
                                              PullUpResistor Pin3 = PullUpResistor.Unchanged,
                                              PullUpResistor Pin4 = PullUpResistor.Unchanged,
                                              PullUpResistor Pin5 = PullUpResistor.Unchanged,
                                              PullUpResistor Pin6 = PullUpResistor.Unchanged,
                                              PullUpResistor Pin7 = PullUpResistor.Unchanged) {
            int mask = Read(Register.PullUP, Bank.B);

            switch (Pin0) {
                case PullUpResistor.Unchanged:
                    break;
                case PullUpResistor.Enabled:
                    mask = mask | 0x01;
                    break;
                case PullUpResistor.Disabled:
                    mask = mask & 0xFE;
                    break;
            }
            switch (Pin1) {
                case PullUpResistor.Unchanged:
                    break;
                case PullUpResistor.Enabled:
                    mask = mask | 0x02;
                    break;
                case PullUpResistor.Disabled:
                    mask = mask & 0xFD;
                    break;
            }
            switch (Pin2) {
                case PullUpResistor.Unchanged:
                    break;
                case PullUpResistor.Enabled:
                    mask = mask | 0x04;
                    break;
                case PullUpResistor.Disabled:
                    mask = mask & 0xFB;
                    break;
            }
            switch (Pin3) {
                case PullUpResistor.Unchanged:
                    break;
                case PullUpResistor.Enabled:
                    mask = mask | 0x08;
                    break;
                case PullUpResistor.Disabled:
                    mask = mask & 0xF7;
                    break;
            }
            switch (Pin4) {
                case PullUpResistor.Unchanged:
                    break;
                case PullUpResistor.Enabled:
                    mask = mask | 0x10;
                    break;
                case PullUpResistor.Disabled:
                    mask = mask & 0xEF;
                    break;
            }
            switch (Pin5) {
                case PullUpResistor.Unchanged:
                    break;
                case PullUpResistor.Enabled:
                    mask = mask | 0x20;
                    break;
                case PullUpResistor.Disabled:
                    mask = mask & 0xDF;
                    break;
            }
            switch (Pin6) {
                case PullUpResistor.Unchanged:
                    break;
                case PullUpResistor.Enabled:
                    mask = mask | 0x40;
                    break;
                case PullUpResistor.Disabled:
                    mask = mask & 0xBF;
                    break;
            }
            switch (Pin7) {
                case PullUpResistor.Unchanged:
                    break;
                case PullUpResistor.Enabled:
                    mask = mask | 0x80;
                    break;
                case PullUpResistor.Disabled:
                    mask = mask & 0x7F;
                    break;
            }
            Write(Register.PullUP, Bank.B, (byte)mask);
        }

        public void SetPullUpResistorsOnBankB(AllPullUps bank) {
            if (bank == AllPullUps.Enabled) {
                Write(Register.PullUP, Bank.B, 0xFF);
            }
            else {
                Write(Register.PullUP, Bank.B, 0x00);
            }
        }

        public void SetPullUpResistorsOnBankB(int pin, PullUpResistor resistor) //pin number <= 7
        {
            byte currentState = Read(Register.PullUP, Bank.B);
            bool state = ((int)resistor == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.PullUP, Bank.B, ValueToWrite);
        }

        public void SetSequentialOperationOnBankA(AddressPointer SeqOP) {
            int pin = 5;
            byte currentState = Read(Register.IOCON, Bank.A);
            bool state = ((int)SeqOP == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.IOCON, Bank.A, ValueToWrite);
        }

        public void SetSequentialOperationOnBankB(AddressPointer SeqOP) {
            int pin = 5;
            byte currentState = Read(Register.IOCON, Bank.B);
            bool state = ((int)SeqOP == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.IOCON, Bank.B, ValueToWrite);
        }

        public void SetSlewRateOnBankA(SlewRate Enabled) {
            int pin = 4;
            byte currentState = Read(Register.IOCON, Bank.A);
            bool state = ((int)Enabled == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.IOCON, Bank.A, ValueToWrite);
        }

        public void SetSlewRateOnBankB(SlewRate Enabled) {
            int pin = 4;
            byte currentState = Read(Register.IOCON, Bank.B);
            bool state = ((int)Enabled == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.IOCON, Bank.B, ValueToWrite);
        }

        public void SetTypeOfInputLogicOnBankA(byte values) {
            Write(Register.InputPolarity, Bank.A, values);
        }

        public void SetTypeOfInputLogicOnBankA(InputLogic Pin0 = InputLogic.Unchanged,
                                               InputLogic Pin1 = InputLogic.Unchanged,
                                               InputLogic Pin2 = InputLogic.Unchanged,
                                               InputLogic Pin3 = InputLogic.Unchanged,
                                               InputLogic Pin4 = InputLogic.Unchanged,
                                               InputLogic Pin5 = InputLogic.Unchanged,
                                               InputLogic Pin6 = InputLogic.Unchanged,
                                               InputLogic Pin7 = InputLogic.Unchanged) {
            int mask = Read(Register.InputPolarity, Bank.A);

            switch (Pin0) {
                case InputLogic.Unchanged:
                    break;
                case InputLogic.Inverted:
                    mask = mask | 0x01;
                    break;
                case InputLogic.Normal:
                    mask = mask & 0xFE;
                    break;
            }
            switch (Pin1) {
                case InputLogic.Unchanged:
                    break;
                case InputLogic.Inverted:
                    mask = mask | 0x02;
                    break;
                case InputLogic.Normal:
                    mask = mask & 0xFD;
                    break;
            }
            switch (Pin2) {
                case InputLogic.Unchanged:
                    break;
                case InputLogic.Inverted:
                    mask = mask | 0x04;
                    break;
                case InputLogic.Normal:
                    mask = mask & 0xFB;
                    break;
            }
            switch (Pin3) {
                case InputLogic.Unchanged:
                    break;
                case InputLogic.Inverted:
                    mask = mask | 0x08;
                    break;
                case InputLogic.Normal:
                    mask = mask & 0xF7;
                    break;
            }
            switch (Pin4) {
                case InputLogic.Unchanged:
                    break;
                case InputLogic.Inverted:
                    mask = mask | 0x10;
                    break;
                case InputLogic.Normal:
                    mask = mask & 0xEF;
                    break;
            }
            switch (Pin5) {
                case InputLogic.Unchanged:
                    break;
                case InputLogic.Inverted:
                    mask = mask | 0x20;
                    break;
                case InputLogic.Normal:
                    mask = mask & 0xDF;
                    break;
            }
            switch (Pin6) {
                case InputLogic.Unchanged:
                    break;
                case InputLogic.Inverted:
                    mask = mask | 0x40;
                    break;
                case InputLogic.Normal:
                    mask = mask & 0xBF;
                    break;
            }
            switch (Pin7) {
                case InputLogic.Unchanged:
                    break;
                case InputLogic.Inverted:
                    mask = mask | 0x80;
                    break;
                case InputLogic.Normal:
                    mask = mask & 0x7F;
                    break;
            }
            Write(Register.InputPolarity, Bank.A, (byte)mask);
        }

        public void SetTypeOfInputLogicOnBankA(AllLogic bank) {
            if (bank == AllLogic.AllInverted) {
                Write(Register.InputPolarity, Bank.A, 0xFF);
            }
            else {
                Write(Register.InputPolarity, Bank.A, 0x00);
            }
        }

        public void SetTypeOfInputLogicOnBankA(int pin, InputLogic logic) {
            byte currentState = Read(Register.InputPolarity, Bank.A);
            bool state = ((int)logic == 1) ? (true) : (false);
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.InputPolarity, Bank.A, ValueToWrite);
        }

        public void SetTypeOfInputLogicOnBankB(byte values) {
            Write(Register.InputPolarity, Bank.B, values);
        }

        public void SetTypeOfInputLogicOnBankB(InputLogic Pin0 = InputLogic.Unchanged,
                                               InputLogic Pin1 = InputLogic.Unchanged,
                                               InputLogic Pin2 = InputLogic.Unchanged,
                                               InputLogic Pin3 = InputLogic.Unchanged,
                                               InputLogic Pin4 = InputLogic.Unchanged,
                                               InputLogic Pin5 = InputLogic.Unchanged,
                                               InputLogic Pin6 = InputLogic.Unchanged,
                                               InputLogic Pin7 = InputLogic.Unchanged) {
            int mask = Read(Register.InputPolarity, Bank.B);

            switch (Pin0) {
                case InputLogic.Unchanged:
                    break;
                case InputLogic.Inverted:
                    mask = mask | 0x01;
                    break;
                case InputLogic.Normal:
                    mask = mask & 0xFE;
                    break;
            }
            switch (Pin1) {
                case InputLogic.Unchanged:
                    break;
                case InputLogic.Inverted:
                    mask = mask | 0x02;
                    break;
                case InputLogic.Normal:
                    mask = mask & 0xFD;
                    break;
            }
            switch (Pin2) {
                case InputLogic.Unchanged:
                    break;
                case InputLogic.Inverted:
                    mask = mask | 0x04;
                    break;
                case InputLogic.Normal:
                    mask = mask & 0xFB;
                    break;
            }
            switch (Pin3) {
                case InputLogic.Unchanged:
                    break;
                case InputLogic.Inverted:
                    mask = mask | 0x08;
                    break;
                case InputLogic.Normal:
                    mask = mask & 0xF7;
                    break;
            }
            switch (Pin4) {
                case InputLogic.Unchanged:
                    break;
                case InputLogic.Inverted:
                    mask = mask | 0x10;
                    break;
                case InputLogic.Normal:
                    mask = mask & 0xEF;
                    break;
            }
            switch (Pin5) {
                case InputLogic.Unchanged:
                    break;
                case InputLogic.Inverted:
                    mask = mask | 0x20;
                    break;
                case InputLogic.Normal:
                    mask = mask & 0xDF;
                    break;
            }
            switch (Pin6) {
                case InputLogic.Unchanged:
                    break;
                case InputLogic.Inverted:
                    mask = mask | 0x40;
                    break;
                case InputLogic.Normal:
                    mask = mask & 0xBF;
                    break;
            }
            switch (Pin7) {
                case InputLogic.Unchanged:
                    break;
                case InputLogic.Inverted:
                    mask = mask | 0x80;
                    break;
                case InputLogic.Normal:
                    mask = mask & 0x7F;
                    break;
            }
            Write(Register.InputPolarity, Bank.B, (byte)mask);
        }

        public void SetTypeOfInputLogicOnBankB(AllLogic bank) {
            if (bank == AllLogic.AllInverted) {
                Write(Register.InputPolarity, Bank.B, 0xFF);
            }
            else {
                Write(Register.InputPolarity, Bank.B, 0x00);
            }
        }

        public void SetTypeOfInputLogicOnBankB(int pin, InputLogic logic) {
            byte currentState = Read(Register.InputPolarity, Bank.B);
            bool state = ((int)logic == 1) ? true : false;
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.InputPolarity, Bank.B, ValueToWrite);
        }

        public void WriteToPinsOnBankA(byte values) {
            Write(Register.OutputLatch, Bank.A, values);
        }

        public void WriteToPinsOnBankA(Pinstate Pin0 = Pinstate.Unchanged,
                                       Pinstate Pin1 = Pinstate.Unchanged,
                                       Pinstate Pin2 = Pinstate.Unchanged,
                                       Pinstate Pin3 = Pinstate.Unchanged,
                                       Pinstate Pin4 = Pinstate.Unchanged,
                                       Pinstate Pin5 = Pinstate.Unchanged,
                                       Pinstate Pin6 = Pinstate.Unchanged,
                                       Pinstate Pin7 = Pinstate.Unchanged) {
            int mask = Read(Register.OutputLatch, Bank.A);

            switch (Pin0) {
                case Pinstate.Unchanged:
                    break;
                case Pinstate.High:
                    mask = mask | 0x01;
                    break;
                case Pinstate.Low:
                    mask = mask & 0xFE;
                    break;
            }
            switch (Pin1) {
                case Pinstate.Unchanged:
                    break;
                case Pinstate.High:
                    mask = mask | 0x02;
                    break;
                case Pinstate.Low:
                    mask = mask & 0xFD;
                    break;
            }
            switch (Pin2) {
                case Pinstate.Unchanged:
                    break;
                case Pinstate.High:
                    mask = mask | 0x04;
                    break;
                case Pinstate.Low:
                    mask = mask & 0xFB;
                    break;
            }
            switch (Pin3) {
                case Pinstate.Unchanged:
                    break;
                case Pinstate.High:
                    mask = mask | 0x08;
                    break;
                case Pinstate.Low:
                    mask = mask & 0xF7;
                    break;
            }
            switch (Pin4) {
                case Pinstate.Unchanged:
                    break;
                case Pinstate.High:
                    mask = mask | 0x10;
                    break;
                case Pinstate.Low:
                    mask = mask & 0xEF;
                    break;
            }
            switch (Pin5) {
                case Pinstate.Unchanged:
                    break;
                case Pinstate.High:
                    mask = mask | 0x20;
                    break;
                case Pinstate.Low:
                    mask = mask & 0xDF;
                    break;
            }
            switch (Pin6) {
                case Pinstate.Unchanged:
                    break;
                case Pinstate.High:
                    mask = mask | 0x40;
                    break;
                case Pinstate.Low:
                    mask = mask & 0xBF;
                    break;
            }
            switch (Pin7) {
                case Pinstate.Unchanged:
                    break;
                case Pinstate.High:
                    mask = mask | 0x80;
                    break;
                case Pinstate.Low:
                    mask = mask & 0x7F;
                    break;
            }
            Write(Register.OutputLatch, Bank.A, (byte)mask);
        }

        public void WriteToPinsOnBankA(AllOutputs bank) {
            if (bank == AllOutputs.High) {
                Write(Register.OutputLatch, Bank.A, 0xFF);
            }
            else {
                Write(Register.OutputLatch, Bank.A, 0x00);
            }
        }

        public void WriteToPinsOnBankA(int pin, Pinstate value) {
            bool state = (value == Pinstate.High) ? true : false;
            byte currentState = Read(Register.OutputLatch, Bank.A);
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.OutputLatch, Bank.A, ValueToWrite);
        }

        public void WriteToPinsOnBankB(byte values) {
            Write(Register.OutputLatch, Bank.B, values);
        }

        public void WriteToPinsOnBankB(Pinstate Pin0 = Pinstate.Unchanged,
                                       Pinstate Pin1 = Pinstate.Unchanged,
                                       Pinstate Pin2 = Pinstate.Unchanged,
                                       Pinstate Pin3 = Pinstate.Unchanged,
                                       Pinstate Pin4 = Pinstate.Unchanged,
                                       Pinstate Pin5 = Pinstate.Unchanged,
                                       Pinstate Pin6 = Pinstate.Unchanged,
                                       Pinstate Pin7 = Pinstate.Unchanged) {
            int mask = Read(Register.OutputLatch, Bank.B);

            switch (Pin0) {
                case Pinstate.Unchanged:
                    break;
                case Pinstate.High:
                    mask = mask | 0x01;
                    break;
                case Pinstate.Low:
                    mask = mask & 0xFE;
                    break;
            }
            switch (Pin1) {
                case Pinstate.Unchanged:
                    break;
                case Pinstate.High:
                    mask = mask | 0x02;
                    break;
                case Pinstate.Low:
                    mask = mask & 0xFD;
                    break;
            }
            switch (Pin2) {
                case Pinstate.Unchanged:
                    break;
                case Pinstate.High:
                    mask = mask | 0x04;
                    break;
                case Pinstate.Low:
                    mask = mask & 0xFB;
                    break;
            }
            switch (Pin3) {
                case Pinstate.Unchanged:
                    break;
                case Pinstate.High:
                    mask = mask | 0x08;
                    break;
                case Pinstate.Low:
                    mask = mask & 0xF7;
                    break;
            }
            switch (Pin4) {
                case Pinstate.Unchanged:
                    break;
                case Pinstate.High:
                    mask = mask | 0x10;
                    break;
                case Pinstate.Low:
                    mask = mask & 0xEF;
                    break;
            }
            switch (Pin5) {
                case Pinstate.Unchanged:
                    break;
                case Pinstate.High:
                    mask = mask | 0x20;
                    break;
                case Pinstate.Low:
                    mask = mask & 0xDF;
                    break;
            }
            switch (Pin6) {
                case Pinstate.Unchanged:
                    break;
                case Pinstate.High:
                    mask = mask | 0x40;
                    break;
                case Pinstate.Low:
                    mask = mask & 0xBF;
                    break;
            }
            switch (Pin7) {
                case Pinstate.Unchanged:
                    break;
                case Pinstate.High:
                    mask = mask | 0x80;
                    break;
                case Pinstate.Low:
                    mask = mask & 0x7F;
                    break;
            }
            Write(Register.OutputLatch, Bank.B, (byte)mask);
        }

        public void WriteToPinsOnBankB(AllOutputs bank) {
            if (bank == AllOutputs.High) {
                Write(Register.OutputLatch, Bank.B, 0xFF);
            }
            else {
                Write(Register.OutputLatch, Bank.B, 0x00);
            }
        }

        public void WriteToPinsOnBankB(int pin, Pinstate value) {
            bool state = (value == Pinstate.High) ? true : false;
            byte currentState = Read(Register.OutputLatch, Bank.B);
            byte ValueToWrite = SinglePinByteCalculation(pin, state, currentState);
            Write(Register.OutputLatch, Bank.B, ValueToWrite);
        }

        private byte Read(Register register, Bank bus) {
            byte buf = 0x00;
            if (bus == Bank.A) {
                if (ReadRegister((byte)register, out buf)) {
                    return buf;
                }
                return 0x00;
            }
            int AlternateBus = (int)register + 1;
            if (ReadRegister((byte)AlternateBus, out buf)) {
                return buf;
            }
            return 0x00;
        }

        private ushort ReadBothBanks(Register register) {
            byte a = Read(register, Bank.A);
            byte b = Read(register, Bank.B);
            var result = (ushort)((a << 8) | b);
            return result;
        }

        private byte SinglePinByteCalculation(int pin, bool state, byte currentState) {
            if (pin < 0 || pin > 7) {
                throw new Exception();
            }

            byte ValueToWrite;
            if (state) {
                ValueToWrite = (byte)((1 << pin) | currentState);
            }
            else {
                ValueToWrite = (byte)(~(1 << pin) & currentState);
            }
            return ValueToWrite;
        }

        private void Write(Register register, Bank bus, byte value1) {
            if (bus == Bank.A) {
                WriteRegister((byte)register, value1);
            }
            else {
                int AlternateBus = (int)register + 1;
                WriteRegister((byte)AlternateBus, value1);
            }
        }

        private enum Bank
        {
            A,
            B
        }

        public class Inputs
        {
            private readonly WeakCache Cache = new WeakCache();

            public IDigitalInput Bind(Pin pin, string name = null, ResistorMode internalResistorMode = ResistorMode.Disabled) {
                var result = NewInput(pin, name, internalResistorMode);
                Cache.Add(pin, result);
                return result;
            }

            public IDigitalInput Get(Pin pin) {
                return (IDigitalInterrupt)Cache.GetIfActive(pin);
            }

            internal void DisposeActive() {
                Cache.DisposeItemsAndClear();
            }

            private IDigitalInput NewInput(Pin pin, string name = null, ResistorMode internalResistorMode = ResistorMode.Disabled) {
                throw new NotImplementedException();
            }
        }

        public class Outputs
        {
            private readonly WeakCache Cache = new WeakCache();

            public IDigitalOutput Bind(Pin pin, string name = null, bool initialState = false) {
                if (pin == Pin.None) {
                    return null;
                }

                var result = new Instance(pin, name, initialState);
                Cache.Add(pin, result);
                return result;
            }

            public IDigitalOutput Get(Pin pin) {
                return (IDigitalOutput)Cache.GetIfActive(pin);
            }

            internal void DisposeActive() {
                Cache.DisposeItemsAndClear();
            }

            private class Instance : IDigitalOutput
            {
                public Instance(Pin pin, string name = null, bool initialState = false) {
                    Pin = pin;
                    Name = name;
                    State = initialState;
                }

                public string Name { get; private set; }

                public Pin Pin { get; private set; }

                public bool State { get; private set; }

                public void Dispose() {
                    throw new NotImplementedException();
                }

                public void Write(bool state) {
                    throw new NotImplementedException();
                }
            }
        }

        private enum Register : byte
        {
            IODirection = 0x00,
            InputPolarity = 0x02,
            InterruptEnable = 0x04,
            PortDefaultValue = 0x06,
            InterruptTypeControl = 0x08,
            IOCON = 0x0A,
            PullUP = 0x0C,
            InterruptFlag = 0x0E,
            InterruptCapturedValue = 0x10,
            GPIO = 0x12,
            OutputLatch = 0x14
        }
    }
}