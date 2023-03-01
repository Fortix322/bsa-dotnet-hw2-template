using CoolParking.WebAPI.Interfaces;

namespace CoolParking.WebAPI.Services
{
    public class LogTimerService : TimerService, ILogTimerService
    {
        public LogTimerService(double interval) : base(interval) { }
    }
}
