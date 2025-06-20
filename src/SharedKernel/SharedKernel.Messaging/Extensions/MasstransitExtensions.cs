

using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SharedKernel.Extensions
{
    public static class MasstransitExtensions
    {
        public static IServiceCollection AddMassTransit (this IServiceCollection _services, IConfiguration _configuration, Assembly? assembly=null)
        {
            _services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();
                if (assembly != null)
                {
                    config.AddConsumers(assembly);
                }

                config.UsingRabbitMq((context, config) => 
                {
                    config.Host(new Uri(_configuration["MessageBroker:Host"]!), host =>
                    {
                        host.Username(_configuration["MessageBroker:UserName"]);
                        host.Password(_configuration["MessageBroker:Password"]);
                    });


                    config.UseRawJsonSerializer();
                    config.ConfigureEndpoints(context);
                });
            });
            return _services;
        }
    }
}
