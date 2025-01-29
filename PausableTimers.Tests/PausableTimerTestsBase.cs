namespace PausableTimers.Tests;

public abstract class PausableTimerTestsBase<T> where T : IPausableTimer, new()
{
    protected T Timer;
    protected int EventFired;

    [SetUp]
    public void SetUp()
    {
        Timer = new T();
        Timer.Interval = 100;
        Timer.Elapsed += (_, _) => EventFired++;
        EventFired = 0;
    }

    [TearDown]
    public void TearDown()
    {
        Timer.Dispose();
    }

    [Test]
    public async Task Timer_Should_Fire_Elapsed_After_Interval()
    {
        Timer.Start();
        await Task.Delay(150);
        Assert.That(EventFired, Is.EqualTo(1), "Timer did not fire after interval");
    }

    [Test]
    public async Task Timer_Should_Not_Fire_After_Stop()
    {
        Timer.Start();
        await Task.Delay(50);
        Timer.Stop();
        EventFired = 0;
        await Task.Delay(100);
        Assert.That(EventFired, Is.EqualTo(0), "Timer fired after being stopped");
    }

    [Test]
    public async Task Timer_Should_Pause_And_Not_Fire()
    {
        Timer.Start();
        await Task.Delay(50);
        Timer.Pause();
        EventFired = 0;
        await Task.Delay(100);
        Assert.That(EventFired, Is.EqualTo(0), "Timer fired after being paused");
    }

    [Test]
    public async Task Timer_Should_Resume_From_Paused_State()
    {
        Timer.Start();
        await Task.Delay(50);
        Timer.Pause();
        await Task.Delay(50);
        Timer.Resume();
        await Task.Delay(60);
        Assert.That(EventFired, Is.EqualTo(1), "Timer did not fire after being resumed");
    }

    [Test]
    public async Task Timer_Should_Reset_When_Stopped()
    {
        Timer.Start();
        await Task.Delay(50);
        Timer.Stop();
        EventFired = 0;
        await Task.Delay(100);
        Assert.That(EventFired, Is.EqualTo(0), "Timer fired after being stopped");
    }

    [Test]
    public async Task Timer_Should_Remember_Original_Interval()
    {
        Timer.Start();
        await Task.Delay(90);
        Timer.Pause();
        await Task.Delay(50);
        EventFired = 0;
        Timer.Resume();
        await Task.Delay(60);
        Assert.That(EventFired, Is.EqualTo(1), "Timer interval incorrect after pause");
    }
}