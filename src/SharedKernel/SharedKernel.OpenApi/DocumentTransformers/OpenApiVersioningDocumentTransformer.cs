

using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace SharedKernel.OpenApi.DocumentTransformers
{
    public class OpenApiVersioningDocumentTransformer : IOpenApiDocumentTransformer
    {

        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;
        private readonly IOptions<OpenApiOptions> _options;

        public OpenApiVersioningDocumentTransformer(IApiVersionDescriptionProvider apiVersionDescriptionProvider, IOptions<OpenApiOptions> options)
        {
            _apiVersionDescriptionProvider=apiVersionDescriptionProvider;
            _options=options;
        }

        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            var apiVersionDescription = _apiVersionDescriptionProvider.ApiVersionDescriptions
           .FirstOrDefault(v => string.Equals(v.GroupName, context.DocumentName, StringComparison.OrdinalIgnoreCase));


            if (apiVersionDescription is not null)
            {
                document.Info.Title += $" - API Version {apiVersionDescription.ApiVersion}";
            }

            return Task.CompletedTask;
        }
    }
}
