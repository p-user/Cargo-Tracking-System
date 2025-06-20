

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace SharedKernel.Core.Extensions
{
    public static class KestrelConfigurationExtension
    {
        public static IWebHostBuilder ConfigureCustomKestrelForRest(this IWebHostBuilder builder, string environmentName)
        {
            if (!string.Equals(environmentName, "Local", StringComparison.OrdinalIgnoreCase))
            {
                builder.ConfigureKestrel(options =>
                {

                    options.ListenAnyIP(8080, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                    });


                    options.ListenAnyIP(8081, listenOptions =>
                    {
                        listenOptions.UseHttps();
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
                    });
                });
            }

            return builder;
        }

        public static IWebHostBuilder ConfigureCustomKestrelForGrpc(this IWebHostBuilder builder, string environmentName)
        {
            if (!string.Equals(environmentName, "Local", StringComparison.OrdinalIgnoreCase))
            {
                builder.ConfigureKestrel(options =>
                {

                    options.ListenAnyIP(8080, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                    });


                    options.ListenAnyIP(8081, listenOptions =>
                    {
                        listenOptions.UseHttps();
                        listenOptions.Protocols = HttpProtocols.Http2;
                    });
                });
            }

            return builder;
        }
    }
}
