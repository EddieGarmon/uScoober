using System;
using Microsoft.SPOT.Hardware;
using uScoober.DataStructures;

namespace uScoober.Hardware
{
    public static class Signals
    {
        public static void DisposeAll() {
            AnalogInput.DisposeActive();
            DigitalInput.DisposeActive();
            DigitalInterrupt.DisposeActive();
            DigitalOutput.DisposeActive();
            PwmOutput.DisposeActive();
        }

        private static Exception NoBuilderFound() {
            return new Exception("No builder found. Are you missing uScoober.Hardware.Spot.* or other adapters?");
        }

        public static class AnalogInput
        {
            private static readonly WeakCache Cache = new WeakCache();

            static AnalogInput() {
                NewInstance = (channel, name) => { throw NoBuilderFound(); };
            }

            public static BuildAnalogInput NewInstance { private get; set; }

            public static IAnalogInput Bind(AnalogChannel channel, string name = null) {
                var result = NewInstance(channel, name);
                Cache.Add(channel, result);
                return result;
            }

            public static IAnalogInput Get(AnalogChannel channel) {
                return (IAnalogInput)Cache.GetIfActive(channel);
            }

            internal static void DisposeActive() {
                Cache.DisposeItemsAndClear();
            }
        }

        public static class DigitalInput
        {
            private static readonly WeakCache Cache = new WeakCache();

            static DigitalInput() {
                NewInstance = (pin, name, mode) => { throw NoBuilderFound(); };
            }

            public static BuildDigitalInput NewInstance { private get; set; }

            public static IDigitalInput Bind(Pin pin, string name = null, ResistorMode internalResistorMode = ResistorMode.Disabled) {
                var result = NewInstance(pin, name, internalResistorMode);
                Cache.Add(pin, result);
                return result;
            }

            public static IDigitalInput Get(Pin pin) {
                return (IDigitalInput)Cache.GetIfActive(pin);
            }

            internal static void DisposeActive() {
                Cache.DisposeItemsAndClear();
            }
        }

        public static class DigitalInterrupt
        {
            private static readonly WeakCache Cache = new WeakCache();

            static DigitalInterrupt() {
                NewInstance = (pin, name, mode, interruptMode, milliseconds) => { throw NoBuilderFound(); };
            }

            public static BuildDigitalInterrupt NewInstance { private get; set; }

            public static IDigitalInterrupt Bind(Pin pin,
                                                 string name = null,
                                                 ResistorMode internalResistorMode = ResistorMode.Disabled,
                                                 InterruptMode interruptMode = InterruptMode.InterruptNone,
                                                 int debounceMilliseconds = DigitalInterupt.DebounceDefault) {
                var result = NewInstance(pin, name, internalResistorMode, interruptMode, debounceMilliseconds);
                Cache.Add(pin, result);
                return result;
            }

            public static IDigitalInterrupt Bind(Pin pin,
                                                 InterruptHandler handler,
                                                 string name = null,
                                                 ResistorMode internalResistorMode = ResistorMode.Disabled,
                                                 InterruptMode interruptMode = InterruptMode.InterruptNone,
                                                 int debounceMilliseconds = 0) {
                var result = NewInstance(pin, name, internalResistorMode, interruptMode, debounceMilliseconds);
                result.OnInterrupt += handler;
                Cache.Add(pin, result);
                return result;
            }

            public static IDigitalInterrupt Get(Pin pin) {
                return (IDigitalInterrupt)Cache.GetIfActive(pin);
            }

            internal static void DisposeActive() {
                Cache.DisposeItemsAndClear();
            }
        }

        public static class DigitalOutput
        {
            private static readonly WeakCache Cache = new WeakCache();

            static DigitalOutput() {
                NewInstance = (pin, state, name) => { throw NoBuilderFound(); };
            }

            public static BuildDigitalOutput NewInstance { private get; set; }

            public static IDigitalOutput Bind(Pin pin, string name = null, bool initialState = false) {
                var result = NewInstance(pin, name, initialState);
                Cache.Add(pin, result);
                return result;
            }

            public static IDigitalOutput Get(Pin pin) {
                return (IDigitalOutput)Cache.GetIfActive(pin);
            }

            internal static void DisposeActive() {
                Cache.DisposeItemsAndClear();
            }
        }

        public static class PwmOutput
        {
            private static readonly WeakCache Cache = new WeakCache();

            static PwmOutput() {
                NewInstance = (channel, name) => { throw NoBuilderFound(); };
            }

            public static BuildPwmOutput NewInstance { private get; set; }

            public static IPwmOutput Bind(PwmChannel channel, string name = null) {
                var result = NewInstance(channel, name);
                Cache.Add(channel, result);
                return result;
            }

            public static IPwmOutput Get(Pin pin) {
                return (IPwmOutput)Cache.GetIfActive(pin);
            }

            internal static void DisposeActive() {
                Cache.DisposeItemsAndClear();
            }
        }
    }
}