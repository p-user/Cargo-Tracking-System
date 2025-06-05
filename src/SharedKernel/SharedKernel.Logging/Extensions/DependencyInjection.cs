using Elastic.Channels;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;
using Serilog.Settings.Configuration;
using SharedKernel.Logging;

namespace SharedKernel.Logging.Extensions
{
    public static class DependencyInjection
    {
        public static WebApplicationBuilder AddCustomSerilog(this WebApplicationBuilder builder, Action<LoggerConfiguration>? extraConfigure = null, Action<SerilogOptions>? configurator = null)
        {
            //Load the appsettings.json from each microservice
            var serilogOptions = builder.Configuration.GetSection(nameof(SerilogOptions))
            .Get<SerilogOptions>() ?? new SerilogOptions();

            configurator?.Invoke(serilogOptions);

            //For preventing duplicate write logs by .net default logs provider 
            builder.Logging.ClearProviders();
            builder.Host.UseSerilog((context, services, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(
                    context.Configuration,
                    new ConfigurationReaderOptions { SectionName = nameof(SerilogOptions) });

                extraConfigure?.Invoke(loggerConfiguration);

                loggerConfiguration
                    .Enrich.WithProperty("Application", builder.Environment.ApplicationName)
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails();

                if (!string.IsNullOrEmpty(serilogOptions.ElasticSearchUrl))
                {
                    loggerConfiguration.WriteTo.Elasticsearch(
                        [new Uri(serilogOptions.ElasticSearchUrl)],
                        opts =>
                        {
                            opts.DataStream = new DataStreamName($"{builder.Environment.ApplicationName.ToLowerInvariant()}-{builder.Environment.EnvironmentName.ToLowerInvariant()}-{DateTime.UtcNow:yyyy-MM}");

                            opts.BootstrapMethod = BootstrapMethod.Failure;

                            opts.ConfigureChannel = channelOpts =>
                            {
                                channelOpts.BufferOptions = new BufferOptions
                                {
                                    ExportMaxConcurrency = 10
                                };
                            };
                        });
                }
            });

            return builder;


        }
    }
}
