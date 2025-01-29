# PausableTimer
[![NuGet](https://img.shields.io/nuget/v/PausableTimer.svg)](https://www.nuget.org/packages/PausableTimer/)

A lightweight C# library that provides a simple timer like `System.Timers.Timer` but with the ability to pause and resume.

## Usage

```csharp
using PausableTimers;

// Start a timer with an interval of 5 seconds
var timer = new PausableTimer();
timer.Interval = 5000;
timer.Elapsed += (sender, e) => Console.WriteLine("Elapsed");
timer.Start();
Console.WriteLine("Timer started");

// Wait for 4.5 seconds and pause timer
await Task.Delay(4500);
timer.Pause();
Console.WriteLine("Timer paused");

// Wait for 5 seconds and resume timer
await Task.Delay(5000);
timer.Resume();
Console.WriteLine("Timer resumed");

// Elapsed shows up ~0.5 seconds after resuming, afterwards timer will elapse every 5 seconds
await Task.Delay(6000);
```