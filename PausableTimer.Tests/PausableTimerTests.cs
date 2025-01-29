namespace PausableTimer.Tests;

    [TestFixture]
    public class PausableTimerTests
    {
        private PausableTimer _timer;
        private bool _eventFired;
        
        [SetUp]
        public void SetUp()
        {
            _timer = new PausableTimer();
            _timer.Interval = 100;
            _timer.Elapsed += (sender, args) => _eventFired = true;
            _eventFired = false;
        }

        [TearDown]
        public void TearDown()
        {
            _timer.Dispose();
        }

        [Test]
        public async Task Timer_Should_Fire_Elapsed_After_Interval()
        {
            _timer.Start();
            await Task.Delay(150);
            Assert.That(_eventFired, "Timer did not fire the elapsed event");
        }

        [Test]
        public async Task Timer_Should_Not_Fire_After_Stop()
        {
            _timer.Start();
            await Task.Delay(50);
            _timer.Stop();
            _eventFired = false;
            await Task.Delay(100);
            Assert.That(!_eventFired, "Timer fired after being stopped");
        }

        [Test]
        public async Task Timer_Should_Pause_And_Not_Fire()
        {
            _timer.Start();
            await Task.Delay(50);
            _timer.Pause();
            _eventFired = false;
            await Task.Delay(100);
            Assert.That(!_eventFired, "Timer fired while paused");
        }

        [Test]
        public async Task Timer_Should_Resume_From_Paused_State()
        {
            _timer.Start();
            await Task.Delay(50);
            _timer.Pause();
            await Task.Delay(50);
            _timer.Resume();
            await Task.Delay(60);
            Assert.That(_eventFired, "Timer did not fire after resuming");
        }

        [Test]
        public async Task Timer_Should_Reset_When_Stopped()
        {
            _timer.Start();
            await Task.Delay(50);
            _timer.Stop();
            _eventFired = false;
            await Task.Delay(100);
            Assert.That(!_eventFired, "Timer fired after being stopped and reset");
        }
    }