using Elastic.Serilog.Sinks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shared.Logging
{
    public static class Serilogger
    {
        private static IConfigurationRoot _configuration;

        static Serilogger()
        {
            //Load the appsettings.json from the Shared.Logging 

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("sharedlogging.appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }
        public static void ConfigureLogger(string environment, string serviceName)
        {
            
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                .Enrich.WithProperty("ServiceName", serviceName)
                .Enrich.WithProperty("Environment", environment)
                .Enrich.WithCorrelationId()
                .WriteTo.Elasticsearch()
                .CreateLogger();

            Log.Information("Logger configured for {ServiceName} in {Environment} environment", serviceName, environment);
        }

        public static void CloseAndFlush()
        {
            Log.CloseAndFlush();
        }
    }
}
