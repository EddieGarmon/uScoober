using System;
using System.Diagnostics;
using Microsoft.SPOT.Hardware;

namespace uScoober
{
    [DebuggerStepThrough]
    public class Stopwatch
    {
        private long _lastSplit;
        private long _started;

        public double CumulativeSplit() {
            if (_started == 0) {
                throw new Exception("Stopwatch is not running.");
            }
            return ComputeElapsed();
        }

        public double Split() {
            if (_started == 0) {
                throw new Exception("Stopwatch is not running.");
            }
            var temp = GetMachineTicks();
            var result = (temp - _lastSplit) / (1.0 * TimeSpan.TicksPerSecond);
            _lastSplit = temp;
            return result;
        }

        public void Start() {
            if (_started != 0) {
                throw new Exception("Stopwatch already running.");
            }
            _started = GetMachineTicks();
            _lastSplit = _started;
        }

        public double Stop() {
            if (_started == 0) {
                return 0;
            }
            var result = ComputeElapsed();
            _started = 0;
            return result;
        }

        private double ComputeElapsed() {
            return (GetMachineTicks() - _started) / (1.0 * TimeSpan.TicksPerSecond);
        }

        public static Stopwatch StartNew() {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            return stopwatch;
        }

        public static double TimeExecution(int iterations, Action action) {
            if (action == null) {
                throw new ArgumentNullException("action");
            }
            long start = GetMachineTicks();
            for (int i = 0; i < iterations; i++) {
                action();
            }
            long end = GetMachineTicks();
            return (end - start) / (iterations * 1.0 * TimeSpan.TicksPerSecond);
        }

        private static long GetMachineTicks() {
            return Utility.GetMachineTime()
                          .Ticks;
        }
    }
}