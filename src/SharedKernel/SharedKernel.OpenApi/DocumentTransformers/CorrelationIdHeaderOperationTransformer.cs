
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace SharedKernel.OpenApi.DocumentTransformers
{
    public class CorrelationIdHeaderOperationTransformer : IOpenApiOperationTransformer
    {
        private const string CorrelationIdHeaderName = "X-Correlation-ID";
        public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            // Add the Correlation ID header to the operation parameters
            operation.Parameters.Add(
                new OpenApiParameter
                {
                    Name = CorrelationIdHeaderName,
                    In = ParameterLocation.Header,
                    Description = "Correlation ID for tracking requests across systems",
                    Required = false, // Set to true if the header is mandatory
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Example = new OpenApiString("123e4567-e89b-12d3-a456-426614174000"), //Example GUID
                    },
                }
            );

            return Task.CompletedTask;
        }
    }
}
