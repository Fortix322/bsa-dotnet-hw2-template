using CoolParking.BL.Interfaces;
using CoolParking.WebAPI.Interfaces;
using CoolParking.WebAPI.Services;

namespace CoolParking.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddSingleton<ILogService>(s => new LogService(Settings.LogPath));
            services.AddSingleton<ILogTimerService>(s => new LogTimerService(Settings.LoggingTimeIntervalInSeconds));
            services.AddSingleton<IWithdrawalTimerService>(s => new WithdrawalTimerService(Settings.WithdrawalTimeIntervalInSeconds));

            services.AddSingleton<IParkingService, ParkingService>(s => new ParkingService(
                s.GetRequiredService<IWithdrawalTimerService>(),
                s.GetRequiredService<ILogTimerService>(),
                s.GetRequiredService<ILogService>()));
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
