using System.Timers;

namespace PausableTimer
{
    public interface IPausableTimer
    {
        /// <summary>
        /// Gets or sets the interval on which to raise events.
        /// </summary>
        double Interval { get; set; }
        
        /// <summary>
        /// Occurs when the interval has elapsed.
        /// </summary>
        event ElapsedEventHandler Elapsed;
        
        /// <summary>
        /// Gets a value indicating whether the timer is paused.
        /// </summary>
        bool IsPaused { get; }
        
        /// <summary>
        /// 
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