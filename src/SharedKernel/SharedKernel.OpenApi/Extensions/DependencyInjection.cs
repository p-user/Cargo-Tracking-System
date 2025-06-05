

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;
using SharedKernel.OpenApi.DocumentTransformers;

namespace SharedKernel.OpenApi.Extensions
{
    public static  class DependencyInjection
    {
        public static WebApplicationBuilder AddAspnetOpenApi(this WebApplicationBuilder builder, string[] versions)
        {
            // Bind OpenApiOptions from configuration
            builder.Services.Configure<OpenApiOptions>(builder.Configuration.GetSection(nameof(OpenApiOptions)));


            foreach (var documentName in versions)
            {
                builder.Services.AddOpenApi(documentName, options =>
                    {
                        //ToDo add authentication
                        options.AddDocumentTransformer<OpenApiVersioningDocumentTransformer>();
                        options.AddOperationTransformer<OpenApiDefaultValuesOperationTransformer>();
                        options.AddSchemaTransformer<EnumSchemaTransformer>();
                    }
                );
            }

            return builder;
        }


        public static WebApplication UseAspnetOpenApi(this WebApplication app)
        {
            app.MapOpenApi();

            app.MapScalarApiReference("/scalar", options =>
            {
                options.WithOpenApiRoutePattern("/openapi/{documentName}.json")
                       .WithTheme(ScalarTheme.Purple)
                       .WithDownloadButton(true);
            });

            return app;
        }
    }
}
