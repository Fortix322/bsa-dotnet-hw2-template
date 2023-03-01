using CoolParking.WebAPI.Interfaces;

namespace CoolParking.WebAPI.Services
{
    public class WithdrawalTimerService : TimerService, IWithdrawalTimerService
    {
        public WithdrawalTimerService(double interval) : base(interval)
        {
        }
    }
}
