using Serilog;
using Serilog.Events;

namespace KPI.DB.Database.Migrations.Extensions
{
    public static class SerilogLoggingExtensions
    {
        public static void ConfigureLogging()
        {
            LoggerConfiguration loggerConfiguration = new LoggerConfiguration().
                MinimumLevel.Debug().
                MinimumLevel.Override("Microsoft", LogEventLevel.Information).
                Enrich.FromLogContext();

            Log.Logger = loggerConfiguration.CreateLogger();
        }
    }
}
