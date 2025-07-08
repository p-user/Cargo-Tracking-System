using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;

namespace SharedKernel.HealthChecks.Extensions
{
    public static class DependencyInjection
    {
        public static IHealthChecksBuilder AddCommonHealthChecks(this IServiceCollection services, IConfiguration config)
        {
            var hcBuilder = services.AddHealthChecks();
            var defaultConn = config.GetConnectionString("Default");
            var provider = config["DatabaseProvider"]?.ToLowerInvariant();

            if (!string.IsNullOrWhiteSpace(defaultConn) && !string.IsNullOrWhiteSpace(provider))
            {
                switch (provider)
                {
                    case "sqlserver":
                        hcBuilder.AddSqlServer(defaultConn, name: "sqlserver");
                        break;
                    case "postgres":
                        hcBuilder.AddNpgSql(defaultConn, name: "postgresql");
                        break;
                    case "sqlite":
                        hcBuilder.AddSqlite(defaultConn, name: "sqlite");
                        break;
                    default:
                        throw new InvalidOperationException($"Unsupported database provider: {provider}");
                }
            }

            // RabbitMQ
            var rabbitConn = config["MessageBroker:Host"];

            if (!string.IsNullOrWhiteSpace(rabbitConn))
            {
                services.AddSingleton<IConnection>(_ =>
                {
                    var factory = new ConnectionFactory
                    {
                        Uri = new Uri(rabbitConn)
                    };

                    return (IConnection)factory.CreateConnectionAsync();
                });

                hcBuilder.AddRabbitMQ(name: "rabbitmq");

            }

            // Elasticsearch
            var elasticUri = config["ElasticSearch:Uri"];
            if (!string.IsNullOrWhiteSpace(elasticUri))
            {
                hcBuilder.AddElasticsearch(elasticUri, name: "elasticsearch");

            }

            // Kibana
            var kibanaUri = config["Kibana:Uri"];
            if (!string.IsNullOrWhiteSpace(kibanaUri))
            {
                hcBuilder.AddUrlGroup(new Uri(kibanaUri), name: "kibana");

            }

            return hcBuilder;
        }


        public static void MapHealthCheckEndpoint(this WebApplication app)
        {
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }
    }
}
