﻿using System;
using System.Threading;
using Microsoft.SPOT.Hardware;
using uScoober;
using uScoober.Hardware;

namespace NetduinoPlus2.Tests
{
    internal class SpotPwmOutput : DisposableBase,
                                   IPulseWidthModulatedOutput
    {
        private readonly PWM _pwm;
        private bool _isActive;

        public SpotPwmOutput(Cpu.PWMChannel pwmChannel, string id = null) {
            _pwm = new PWM(pwmChannel, 100000u, 0, PWM.ScaleFactor.Nanoseconds, false);
            Id = id ?? "PwmOut-" + pwmChannel;
        }

        public uint Duration {
            get {
                ThrowIfDisposed();
                return _pwm.Duration;
            }
        }

        public double DutyCycle {
            get {
                ThrowIfDisposed();
                return (double)_pwm.Duration / _pwm.Period;
            }
            set {
                ThrowIfDisposed();
                if (value < 0 || value > 1) {
                    throw new Exception("Duty cycle must be in the range [0,1].");
                }
                _pwm.Duration = (uint)(_pwm.Period * value);
            }
        }

        public double Frequency {
            get {
                ThrowIfDisposed();
                return (double)_pwm.Scale / _pwm.Period;
            }
            set {
                ThrowIfDisposed();
                // NB: keep current duty cycle for new frequency
                double dutyCycle = (double)_pwm.Duration / _pwm.Period;
                double newPeriod = (double)_pwm.Scale / value;
                double newDuration = newPeriod * dutyCycle;
                _pwm.Period = (uint)newPeriod;
                _pwm.Duration = (uint)newDuration;
            }
        }

        public string Id { get; private set; }

        public bool IsActive {
            get { return _isActive; }
        }

        public uint Period {
            get {
                ThrowIfDisposed();
                return _pwm.Period;
            }
        }

        public void RampTo(double finalDutyCycle, ushort durationMilliseconds = 1000, ushort stepCount = 100) {
            //todo: validate inputs
            int stepTime = durationMilliseconds / stepCount;
            var stepDelta = (uint)(((finalDutyCycle - DutyCycle) / stepCount) * _pwm.Period);
            for (uint i = 0; i < stepCount; i++) {
                _pwm.Duration += stepDelta;
                Thread.Sleep(stepTime);
            }
        }

        public void SetDutyCycleAndFrequency(double dutyCycle, uint frequencyHz) {
            ThrowIfDisposed();
            if (dutyCycle < 0 || dutyCycle > 1) {
                throw new Exception("Duty cycle must be in the range [0,1].");
            }
            if (frequencyHz == 0) {
                throw new Exception("Frequency must be greater than 0Hz.");
            }
            double newPeriod = (double)PWM.ScaleFactor.Nanoseconds / frequencyHz;
            double newDuration = dutyCycle * newPeriod;
            _pwm.Period = (uint)newPeriod;
            _pwm.Duration = (uint)newDuration;
        }

        public void SetPulse(uint periodNanoseconds, uint highDurationNanoseconds) {
            ThrowIfDisposed();
            _pwm.Period = periodNanoseconds;
            _pwm.Duration = highDurationNanoseconds;
        }

        public void Start() {
            ThrowIfDisposed();
            _pwm.Start();
            _isActive = true;
        }

        public void Stop() {
            ThrowIfDisposed();
            _pwm.Stop();
            _isActive = false;
        }

        protected override void DisposeManagedResources() {
            if (_isActive) {
                _pwm.Stop();
                _isActive = false;
            }
            _pwm.Dispose();
        }

        ~SpotPwmOutput() {
            Dispose(DisposeReason.Finalizer);
        }
    }
}