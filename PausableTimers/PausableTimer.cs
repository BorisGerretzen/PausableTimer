using System.Diagnostics;
using System.Timers;

namespace PausableTimers
{
    public class PausableTimer : IPausableTimer
    {        
        /// <inheritdoc />
        public TimerState State { get; private set; } = TimerState.Stopped;

        /// <inheritdoc />
        public double Interval
        {
            get => _timer.Interval;
            set
            {
                _originalInterval = value;
                _timer.Interval = value;
                if (State == TimerState.Running)
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

        /// <inheritdoc />
        public void Start()
        {
            if (State == TimerState.Paused)
            {
                Resume();
            }
            else if (State == TimerState.Stopped)
            {
                ResetState();
                _stopwatch.Start();
                _timer.Start();
            }
            
            State = TimerState.Running;
        }
        
        /// <inheritdoc />
        public void Stop()
        {
            ResetState();
            _timer.Stop();
            _stopwatch.Reset();
            
            State = TimerState.Stopped;
        }
        
        /// <inheritdoc />
        public void Pause()
        {
            if (State != TimerState.Running) return;

            _stopwatch.Stop();
            _remainingInterval -= _stopwatch.Elapsed.TotalMilliseconds;
            _timer.Stop();
            
            State = TimerState.Paused;
        }
        
        /// <inheritdoc />
        public void Resume()
        {
            if (State != TimerState.Paused) return;

            _stopwatch.Restart();
            _timer.Interval = _remainingInterval;
            _timer.Start();
            
            State = TimerState.Running;
        }
        
        public PausableTimer()
        {
            _timer.Elapsed += TimerCallback;
        }

        private void TimerCallback(object state, ElapsedEventArgs e)
        {
            if (State == TimerState.Paused || State == TimerState.Stopped) return;

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