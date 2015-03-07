using System.Threading;
using uScoober.IO;

namespace uScoober.Hardware.Motors
{
    /// <summary>
    /// Class to controll an Electronic Speed Controler (ESC) for Airplanes (NO REVERSE)
    /// </summary>
    public class BrushlessSpeedController
    {
        private readonly IPulseWidthModulatedOutput _pwm;

        /// <summary>
        /// </summary>
        /// <param name="pwmOutput">PWM pin</param>
        public BrushlessSpeedController(IPulseWidthModulatedOutput pwmOutput) {
            _pwm = pwmOutput;
            _pwm.SetPulse(PeriodNanoSeconds, 0);
        }

        public void Arm() {
            SetPpm(LowNanoSeconds);
            _pwm.Start();
            Thread.Sleep(5000);
        }

        public void Disarm() {
            _pwm.Stop();
        }

        public void Full() {
            SetPpm(HighNanoSeconds);
        }

        /// <summary>
        /// Sets the outputpower, from 0 to 100
        /// </summary>
        /// <param name="power">0...100</param>
        public void SetPower(int power) {
            if (power < 0) {
                power = 0;
            }
            if (power > 100) {
                power = 100;
            }
            _pwm.SetPulse(PeriodNanoSeconds, PowerDeltaNanoSeconds * (uint)power + LowNanoSeconds);
        }

        public void SetPpm(uint high) {
            _pwm.SetPulse(PeriodNanoSeconds, high);
        }

        public void Stop() {
            _pwm.SetPulse(PeriodNanoSeconds, LowNanoSeconds);
        }

        private const uint HighNanoSeconds = 23 * 100 * 1000; // 2.3 ms
        private const uint LowNanoSeconds = 10 * 100 * 1000; // 1 ms
        private const uint PeriodNanoSeconds = 20 * 1000 * 1000; // 20 ms
        private const uint PowerDeltaNanoSeconds = (HighNanoSeconds - LowNanoSeconds) / 100;
    }
}