

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SharedKernel.Core.Configurations;

namespace SharedKernel.Core.Extensions
{
    public static class KestrelConfigurationExtension
    {
        public static IWebHostBuilder ConfigureCustomKestrelForRest(this IWebHostBuilder builder)
        {
            
                builder.ConfigureKestrel((context, options) =>
                {
                    var kestrelConfig = context.Configuration.GetSection("KestrelEndpoints");

                    if (!kestrelConfig.Exists())
                    {
                        throw new Exception("KestrelConfig not provided!");
                    }

                    var endpoints = new Dictionary<string, KestrelEndpointConfig>();
                    kestrelConfig.Bind(endpoints);

                    bool isLocal = context.HostingEnvironment.IsEnvironment("Local");

                    if (endpoints.TryGetValue("Http", out var httpEndpoint) && httpEndpoint.Port > 0)
                    {
                        Action<ListenOptions> configureHttp = listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                        };

                        if (isLocal)
                        {
                            options.ListenLocalhost(httpEndpoint.Port, configureHttp);
                        }
                        else
                        {
                            options.ListenAnyIP(httpEndpoint.Port, configureHttp);
                        }
                    }

                    if (endpoints.TryGetValue("Https", out var httpsEndpoint) && httpsEndpoint.Port > 0)
                    {
                        Action<ListenOptions> configureHttps = listenOptions =>
                        {
                            listenOptions.UseHttps();
                            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                        };


                        if (isLocal)
                        {
                            options.ListenLocalhost(httpsEndpoint.Port, configureHttps);
                        }
                        else
                        {
                            options.ListenAnyIP(httpsEndpoint.Port, configureHttps);
                        }
                    }
                });
           
               
            

                return builder;
        }

        public static IWebHostBuilder ConfigureCustomKestrelForGrpc(this IWebHostBuilder builder)
        {

            
                builder.ConfigureKestrel((context,options) =>
                {

                var kestrelConfig = context.Configuration.GetSection("KestrelEndpoints");

                if (!kestrelConfig.Exists())
                {
                    throw new Exception("KestrelConfig not provided!");
                }

                var endpoints = new Dictionary<string, KestrelEndpointConfig>();
                kestrelConfig.Bind(endpoints);

                bool isLocal = context.HostingEnvironment.IsEnvironment("Local");

                    if (endpoints.TryGetValue("Http", out var httpEndpoint) && httpEndpoint.Port > 0)
                    {
                        Action<ListenOptions> configureHttp = listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                        };

                        if (isLocal)
                        {
                            options.ListenLocalhost(httpEndpoint.Port, configureHttp);
                        }
                        else
                        {
                            options.ListenAnyIP(httpEndpoint.Port, configureHttp);
                        }

                        
                    }

                    if (endpoints.TryGetValue("Https", out var httpsEndpoint) && httpsEndpoint.Port > 0)
                    {
                        Action<ListenOptions> configureHttps = listenOptions =>
                        {
                            listenOptions.UseHttps();
                            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                        };

                        if (isLocal)
                        {
                            options.ListenLocalhost(httpsEndpoint.Port, configureHttps);
                        }
                        else
                        {
                            options.ListenAnyIP(httpsEndpoint.Port, configureHttps);
                        }
                    }
                });
            

            return builder;
        }
    }
}
