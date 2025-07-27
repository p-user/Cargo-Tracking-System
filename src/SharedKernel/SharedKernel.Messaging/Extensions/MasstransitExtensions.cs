

using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MassTransit.EntityFrameworkCoreIntegration;

namespace SharedKernel.Messaging.Extensions
{
    public static class MasstransitExtensions
    {
        public static IServiceCollection AddMassTransit<TDbContext>(this IServiceCollection _services, IConfiguration _configuration, Assembly assembly, Action<IEntityFrameworkOutboxConfigurator> dbOutboxConfigurator) where TDbContext : DbContext
        {
            _services.AddMassTransit(config =>

            {
                config.SetKebabCaseEndpointNameFormatter();

               
                  config.AddConsumers(assembly);
                

               
                config.AddEntityFrameworkOutbox<TDbContext>(o =>
                {
                    // How often the background service polls the outbox table.
                    o.QueryDelay = TimeSpan.FromSeconds(1000);
                  

                    // Idempotency (Inbox) configuration
                    o.DuplicateDetectionWindow = TimeSpan.FromMinutes(30);
                    

                    dbOutboxConfigurator?.Invoke(o);
                });



                config.UsingRabbitMq((context, config) => 
                {
                    config.Host(new Uri(_configuration["MessageBroker:Host"]!), host =>
                    {
                        host.Username(_configuration["MessageBroker:UserName"]);
                        host.Password(_configuration["MessageBroker:Password"]);
                    });



                    config.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(5)));
                    config.ConfigureEndpoints(context);

                    
                });
            });
            return _services;
        }
    }
}
