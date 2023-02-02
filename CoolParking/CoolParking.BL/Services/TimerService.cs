// TODO: implement class TimerService from the ITimerService interface.
//       Service have to be just wrapper on System Timers.

using CoolParking.BL.Interfaces;
using System;
using System.Timers;

class TimerService : ITimerService
{
    public TimerService(double interval)
    {
        _timer = new Timer(interval);
        _timer.Elapsed += FireElapsedEvent;
    }

    public double Interval 
    {
        get
        {
            return _timer.Interval;
        }
        set => _timer.Interval = value;
    }

    public event ElapsedEventHandler Elapsed;

    public void Dispose()
    {
        Stop();
        _timer.Elapsed -= FireElapsedEvent;

        Delegate[] clientList = Elapsed.GetInvocationList();
        foreach (var d in clientList)
            Elapsed -= (d as ElapsedEventHandler);
    }

    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }
    private void FireElapsedEvent(object sender, ElapsedEventArgs e)
    {
        Elapsed.Invoke(sender, e);
    }

    private Timer _timer;
}