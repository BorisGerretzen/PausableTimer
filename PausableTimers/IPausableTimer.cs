using System;
using System.Timers;

namespace PausableTimers
{
    public interface IPausableTimer : IDisposable
    {
        /// <summary>
        /// Gets or sets the interval on which to raise events.
        /// If the timer is running, setting this property will reset the remaining interval.
        /// </summary>
        double Interval { get; set; }
        
        /// <summary>
        /// Occurs when the interval has elapsed.
        /// </summary>
        event ElapsedEventHandler Elapsed;
        
        /// <summary>
        /// Gets the current state of the timer.
        /// </summary>
        TimerState State { get; }
        
        /// <summary>
        /// Starts the timer. If the timer is paused, it will resume.
        /// </summary>
        void Start();
        
        /// <summary>
        /// Stops the timer.
        /// </summary>
        void Stop();
        
        /// <summary>
        /// Pauses the timer.
        /// </summary>
        void Pause();
        
        /// <summary>
        /// Resumes the timer.
        /// </summary>
        void Resume();
    }
}