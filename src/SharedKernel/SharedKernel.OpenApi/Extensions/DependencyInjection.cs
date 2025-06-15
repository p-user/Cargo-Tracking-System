

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;



namespace SharedKernel.OpenApi.Extensions
{
    public static class DependencyInjection
    {

            public static IServiceCollection AddAspnetOpenApi(this IServiceCollection services)
            {
                services.AddOpenApi();
                return services;
            }

        public static IApplicationBuilder UseAspnetOpenApi(this WebApplication app, string apiTitle = "API")
        {
            app.MapOpenApi();
            app.MapScalarApiReference("/scalar", options =>
            {
               options.WithTitle(apiTitle);
                options.WithOpenApiRoutePattern("/openapi/v1.json");
            });


            return app;
        }

    }



}
