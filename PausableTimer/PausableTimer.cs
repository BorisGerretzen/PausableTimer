using System;
using System.Diagnostics;
using System.Timers;

namespace PausableTimer
{
    public class PausableTimer : IPausableTimer, IDisposable
    {        
        public bool IsPaused => _state == TimerState.Paused;
        public double Interval
        {
            get => _timer.Interval;
            set
            {
                _timer.Interval = value;
                if (_state == TimerState.Running)
                {
                    _remainingInterval = value;
                }
            }
        }
        public event ElapsedEventHandler Elapsed;
        
        private readonly Timer _timer = new Timer();
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private double _remainingInterval;
        private TimerState _state = TimerState.Stopped;

        public void Start()
        {
            if (IsPaused)
            {
                Resume();
            }
            else if (_state == TimerState.Stopped)
            {
                ResetState();
                _stopwatch.Start();
                _timer.Start();
            }
            
            _state = TimerState.Running;
        }

        public void Stop()
        {
            ResetState();
            _timer.Stop();
            _stopwatch.Reset();
            
            _state = TimerState.Stopped;
        }

        public void Pause()
        {
            if (_state != TimerState.Running) return;

            _stopwatch.Stop();
            _remainingInterval -= _stopwatch.Elapsed.TotalMilliseconds;
            _timer.Stop();
            
            _state = TimerState.Paused;
        }

        public void Resume()
        {
            if (_state != TimerState.Paused) return;

            _stopwatch.Restart();
            _timer.Interval = _remainingInterval;
            _timer.Start();
            
            _state = TimerState.Running;
        }

        public PausableTimer()
        {
            _timer.Elapsed += TimerCallback;
        }

        private void TimerCallback(object state, ElapsedEventArgs e)
        {
            if (_state == TimerState.Paused || _state == TimerState.Stopped) return;

            _stopwatch.Restart();
            Elapsed?.Invoke(this, e);
        }

        private void ResetState()
        {
            _remainingInterval = Interval;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}