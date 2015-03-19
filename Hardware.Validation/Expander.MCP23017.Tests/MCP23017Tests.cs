using uScoober.Hardware.IO;
using uScoober.TestFramework.Assert;
using uScoober.TestFramework.Mocks;

namespace uScoober.Hardware.Expander
{
    public class MCP23017Tests
    {
        private readonly MockI2CBus _bus;
        private readonly MCP23017 _mcp;

        public MCP23017Tests() {
            _bus = new MockI2CBus();
            _mcp = new MCP23017(_bus); //Defaults to address 0x20
        }

        public void EnablePullUpsOnBankA_Fact() {
            BufferInput(0x55);
            _mcp.SetPullUpResistorsOnBankA(1, MCP23017.PullUpResistor.Enabled);
            ObserveOutput(0x0C, 0x0C, 0x57);

            BufferInput(0xFF);
            _mcp.SetPullUpResistorsOnBankA(Pin3: MCP23017.PullUpResistor.Enabled, Pin2: MCP23017.PullUpResistor.Enabled);
            ObserveOutput(0x0C, 0x0C, 0xFF);

            _mcp.SetPullUpResistorsOnBankA(MCP23017.AllPullUps.Enabled);
            ObserveOutput(0x0C, 0xFF);
        }

        public void EnablePullUpsOnBankB_Fact() {
            BufferInput(0xFD);
            _mcp.SetPullUpResistorsOnBankB(1, MCP23017.PullUpResistor.Enabled);
            ObserveOutput(0x0D, 0x0D, 0xFF);

            BufferInput(0xF0);
            _mcp.SetPullUpResistorsOnBankB(Pin3: MCP23017.PullUpResistor.Enabled, Pin2: MCP23017.PullUpResistor.Enabled);
            ObserveOutput(0x0D, 0x0D, 0xFC);
        }

        public void ReadBankA_Fact() {
            BufferInput(0x55);
            _mcp.ReadPinsOnBankA()
                .ShouldEqual(0x55);
            ObserveOutput(0x12);

            BufferInput(0x02);
            _mcp.ReadPinsOnBankA(0)
                .ShouldEqual(0);
            ObserveOutput(0x12);
        }

        public void ReadBankB_Fact() {
            BufferInput(0x18);
            _mcp.ReadPinsOnBankB()
                .ShouldEqual(0x18);
            ObserveOutput(0x13);

            BufferInput(0xFF);
            _mcp.ReadPinsOnBankB(4)
                .ShouldEqual(1);
            ObserveOutput(0x13);
        }

        public void ReadCachedInterruptValuesBankA_Fact() {
            BufferInput(0x00);
            _mcp.ReadPinValuesCachedOnInterruptOnBankA()
                .ShouldEqual(0x00);
            ObserveOutput(0x10);

            BufferInput(0x07);
            _mcp.ReadPinValuesCachedOnInterruptOnBankA(0)
                .ShouldEqual(1);
            ObserveOutput(0x10);
        }

        public void ReadCachedInterruptValuesBankB_Fact() {
            BufferInput(0xFE);
            _mcp.ReadPinValuesCachedOnInterruptOnBankB()
                .ShouldEqual(0xFE);
            ObserveOutput(0x11);

            BufferInput(0x85);
            _mcp.ReadPinValuesCachedOnInterruptOnBankB(0)
                .ShouldEqual(1);
            ObserveOutput(0x11);
        }

        public void ReadInterruptFlagOnBankA_Fact() {
            BufferInput(0x00);
            _mcp.ReadWhichPinCausedInterruptOnBankA()
                .ShouldEqual(-1);
            ObserveOutput(0X0E);

            BufferInput(0x08);
            _mcp.ReadWhichPinCausedInterruptOnBankA()
                .ShouldEqual(3);
            ObserveOutput(0x0E);
        }

        public void ReadInterruptFlagOnBankB_Fact() {
            BufferInput(0x88);
            _mcp.ReadWhichPinCausedInterruptOnBankB()
                .ShouldEqual(-1);
            ObserveOutput(0X0F);

            BufferInput(0x40);
            _mcp.ReadWhichPinCausedInterruptOnBankB()
                .ShouldEqual(6);
            ObserveOutput(0x0F);
        }

        public void SettingDefaultValuesForInterruptsOnBankA_Fact() {
            BufferInput(0x14);
            _mcp.SetDefaultValuesOfInterruptPortsOnBankA(4, MCP23017.InterruptPortDefaultValue.High);
            ObserveOutput(0x06, 0x06, 0x14);

            BufferInput(0x14);
            _mcp.SetDefaultValuesOfInterruptPortsOnBankA(Pin5: MCP23017.InterruptPortDefaultValue.High);
            ObserveOutput(0x06, 0x06, 0x34);
        }

        public void SettingDefaultValuesForInterruptsOnBankB_Fact() {
            BufferInput(0xCC);
            _mcp.SetDefaultValuesOfInterruptPortsOnBankB(0, MCP23017.InterruptPortDefaultValue.High);
            ObserveOutput(0x07, 0x07, 0xCD);

            BufferInput(0xCC);
            _mcp.SetDefaultValuesOfInterruptPortsOnBankB(Pin6: MCP23017.InterruptPortDefaultValue.High);
            ObserveOutput(0x07, 0x07, 0xCC);
        }

        public void SettingInputPolarityOnBankA_Fact() {
            BufferInput(0xA9);
            _mcp.SetTypeOfInputLogicOnBankA(5, MCP23017.InputLogic.NotInvertedLogic);
            ObserveOutput(0x02, 0x02, 0x89);

            BufferInput(0xA9);
            _mcp.SetTypeOfInputLogicOnBankA(Pin7: MCP23017.InputLogic.InvertedLogic,
                                            Pin5: MCP23017.InputLogic.InvertedLogic,
                                            Pin3: MCP23017.InputLogic.InvertedLogic);
            ObserveOutput(0x02, 0x02, 0xA9);
        }

        public void SettingInputPolarityOnBankB_Fact() {
            BufferInput(0x55);
            _mcp.SetTypeOfInputLogicOnBankB(6, MCP23017.InputLogic.NotInvertedLogic);
            ObserveOutput(0x03, 0x03, 0x15);

            BufferInput(0x56);
            _mcp.SetTypeOfInputLogicOnBankB(Pin4: MCP23017.InputLogic.InvertedLogic, Pin3: MCP23017.InputLogic.InvertedLogic);
            ObserveOutput(0x03, 0x03, 0x5E);
        }

        public void SettingInterruptPortsOnBankA_Fact() {
            BufferInput(0x08);
            _mcp.SetInterruptPortsOnBankA(2, MCP23017.InterruptPort.Enable);
            ObserveOutput(0x04, 0x04, 0x0C);

            BufferInput(0x00);
            _mcp.SetInterruptPortsOnBankA(Pin7: MCP23017.InterruptPort.Enable,
                                          Pin6: MCP23017.InterruptPort.Enable,
                                          Pin5: MCP23017.InterruptPort.Enable,
                                          Pin4: MCP23017.InterruptPort.Enable,
                                          Pin3: MCP23017.InterruptPort.Enable,
                                          Pin2: MCP23017.InterruptPort.Enable);
            ObserveOutput(0x04, 0x04, 0xFC);
        }

        public void SettingInterruptPortsOnBankB_Fact() {
            BufferInput(0x0A);
            _mcp.SetInterruptPortsOnBankB(2, MCP23017.InterruptPort.Enable);
            ObserveOutput(0x05, 0x05, 0x0E);

            BufferInput(0x00);
            _mcp.SetInterruptPortsOnBankB(Pin7: MCP23017.InterruptPort.Enable,
                                          Pin6: MCP23017.InterruptPort.Enable,
                                          Pin4: MCP23017.InterruptPort.Enable,
                                          Pin3: MCP23017.InterruptPort.Enable,
                                          Pin2: MCP23017.InterruptPort.Enable);
            ObserveOutput(0x05, 0x05, 0xDC);
        }

        public void SettingInterruptTriggersForInterruptsOnBankA_Fact() {
            BufferInput(0x30);
            _mcp.SetEdgeTriggersForInterruptsOnBankA(5, MCP23017.InterruptPortType.OnEdgeOppositeOfDefaultValue);
            ObserveOutput(0x08, 0x08, 0x30);

            BufferInput(0x30);
            _mcp.SetEdgeTriggersForInterruptsOnBankA(Pin1: MCP23017.InterruptPortType.OnEdgeOppositeOfDefaultValue);
            ObserveOutput(0x08, 0x08, 0x32);
        }

        public void SettingInterruptTriggersForInterruptsOnBankB_Fact() {
            BufferInput(0x3D);
            _mcp.SetEdgeTriggersForInterruptsOnBankB(7, MCP23017.InterruptPortType.OnEdgeOppositeOfDefaultValue);
            ObserveOutput(0x09, 0x09, 0xBD);

            BufferInput(0x3D);
            _mcp.SetEdgeTriggersForInterruptsOnBankB(Pin1: MCP23017.InterruptPortType.OnEdgeOppositeOfDefaultValue,
                                                     Pin0: MCP23017.InterruptPortType.OnEdgeOppositeOfDefaultValue,
                                                     Pin2: MCP23017.InterruptPortType.OnEdgeOppositeOfDefaultValue);
            ObserveOutput(0x09, 0x09, 0x3F);
        }

        public void SetupIODirectionOnBankB_Fact() {
            BufferInput(0x00);
            _mcp.SetIOPinDirectionOnBankB(4, MCP23017.PortType.Input);
            ObserveOutput(0x01, 0x01, 0x10);

            BufferInput(0x00);
            _mcp.SetIOPinDirectionOnBankB(MCP23017.PortType.Input);
            ObserveOutput(0x01, 0x01, 0x01);
        }

        public void SetupIODirectionsOnBankA_Fact() {
            BufferInput(0x00);
            _mcp.SetIOPinDirectionOnBankA(6, MCP23017.PortType.Input);
            ObserveOutput(0x00, 0x00, 0x40);

            BufferInput(0x00);
            _mcp.SetIOPinDirectionOnBankA(MCP23017.PortType.Input, Pin7: MCP23017.PortType.Input);
            ObserveOutput(0x00, 0x00, 0x81);

            _mcp.SetIOPinDirectionOnBankA(MCP23017.AllPins.Input);
            ObserveOutput(0x00, 0xFF);
        }

        public void WriteRegisterBankA_Fact() {
            BufferInput(0x55);
            _mcp.WriteToPinsOnBankA(1, MCP23017.Pinstate.High);
            ObserveOutput(0x14, 0x14, 0x57);

            BufferInput(0x55);
            _mcp.WriteToPinsOnBankA(Pin3: MCP23017.Pinstate.High, Pin2: MCP23017.Pinstate.High);
            ObserveOutput(0x14, 0x14, 0x5D);
        }

        public void WriteRegisterBankB_Fact() {
            BufferInput(0x57);
            _mcp.WriteToPinsOnBankB(1, MCP23017.Pinstate.Low);
            ObserveOutput(0x15, 0x15, 0x55);

            BufferInput(0x13);
            _mcp.WriteToPinsOnBankB(Pin3: MCP23017.Pinstate.High, Pin2: MCP23017.Pinstate.High, Pin0: MCP23017.Pinstate.High);
            ObserveOutput(0x15, 0x15, 0x1F);
        }

        public void WriteRegisterIOCONForBankA_Fact() {
            BufferInput(0xFF);
            _mcp.SetConnectionOfChipsINTPins(MCP23017.ConnectionBetweenBankInterruptPins.Seperated);
            ObserveOutput(0x0A, 0x0A, 0xBF);

            BufferInput(0xBF);
            _mcp.SetSequentialOperationOnBankA(MCP23017.AddressPointer.Increment);
            ObserveOutput(0x0A, 0x0A, 0x9F);

            BufferInput(0x9F);
            _mcp.SetSlewRateOnBankA(MCP23017.SlewRate.Enabled);
            ObserveOutput(0x0A, 0x0A, 0x8F);

            BufferInput(0x8F);
            _mcp.SetINTPinAsOpenDrainOnBankA(MCP23017.BankInterruptType.ActiveDriver);
            ObserveOutput(0x0A, 0x0A, 0x8B);

            BufferInput(0x8B);
            _mcp.SetINTPinPolarityOnBankA(MCP23017.BankInterruptPinState.ActiveLow);
            ObserveOutput(0x0A, 0x0A, 0x89);

            BufferInput(0x8B);
            _mcp.SetConfigurationOfIOSettingsOnBankA(MCP23017.ConnectionBetweenBankInterruptPins.Seperated,
                                                     EnableSlewRate: MCP23017.SlewRate.Disabled,
                                                     InterruptConfiguration: MCP23017.BankInterruptType.OpenDrain);
            ObserveOutput(0x0A, 0x0A, 0x9F);
        }

        public void WriteRegisterIOCONForBankB_Fact() {
            BufferInput(0xFF);
            _mcp.SetConnectionOfChipsINTPins(MCP23017.ConnectionBetweenBankInterruptPins.Seperated);
            ObserveOutput(0x0A, 0x0A, 0xBF);

            BufferInput(0xBF);
            _mcp.SetSequentialOperationOnBankB(MCP23017.AddressPointer.Increment);
            ObserveOutput(0x0B, 0x0B, 0x9F);

            BufferInput(0x9F);
            _mcp.SetSlewRateOnBankB(MCP23017.SlewRate.Enabled);
            ObserveOutput(0x0B, 0x0B, 0x8F);

            BufferInput(0x8F);
            _mcp.SetINTPinAsOpenDrainOnBankB(MCP23017.BankInterruptType.ActiveDriver);
            ObserveOutput(0x0B, 0x0B, 0x8B);

            BufferInput(0x8B);
            _mcp.SetINTPinPolarityOnBankB(MCP23017.BankInterruptPinState.ActiveLow);
            ObserveOutput(0x0B, 0x0B, 0x89);

            BufferInput(0x00);
            _mcp.SetConfigurationOfIOSettingsOnBankB(MCP23017.ConnectionBetweenBankInterruptPins.Seperated,
                                                     EnableSlewRate: MCP23017.SlewRate.Disabled,
                                                     InterruptConfiguration: MCP23017.BankInterruptType.ActiveDriver);
            ObserveOutput(0x0B, 0x0B, 0x10);
        }

        private void BufferInput(params byte[] input) {
            _bus.BufferInputFor(_mcp.Address, input);
        }

        private void ObserveOutput(params byte[] expected) {
            _bus.ShouldObserveOutput(_mcp.Address, expected);
        }
    }
}