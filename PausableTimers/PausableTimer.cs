using System.Diagnostics;
using System.Timers;

namespace PausableTimers
{
    public class PausableTimer : IPausableTimer
    {        
        /// <inheritdoc />
        public bool IsPaused => _state == TimerState.Paused;
        
        /// <inheritdoc />
        public double Interval
        {
            get => _timer.Interval;
            set
            {
                _originalInterval = value;
                _timer.Interval = value;
                if (_state == TimerState.Running)
                {
                    _remainingInterval = value;
                }
            }
        }
        
        /// <inheritdoc />
        public event ElapsedEventHandler Elapsed;
        
        private readonly Timer _timer = new Timer();
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private double _remainingInterval;
        private double _originalInterval;
        private TimerState _state = TimerState.Stopped;
        
        /// <inheritdoc />
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
        
        /// <inheritdoc />
        public void Stop()
        {
            ResetState();
            _timer.Stop();
            _stopwatch.Reset();
            
            _state = TimerState.Stopped;
        }
        
        /// <inheritdoc />
        public void Pause()
        {
            if (_state != TimerState.Running) return;

            _stopwatch.Stop();
            _remainingInterval -= _stopwatch.Elapsed.TotalMilliseconds;
            _timer.Stop();
            
            _state = TimerState.Paused;
        }
        
        /// <inheritdoc />
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
            _remainingInterval = _originalInterval;
            _timer.Interval = _originalInterval;
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