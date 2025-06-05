using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Text.Json;

namespace SharedKernel.OpenApi.DocumentTransformers
{
    internal class OpenApiDefaultValuesOperationTransformer : IOpenApiOperationTransformer
    {
        public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            var apiDescription = context.Description;
            var actionDescriptor = apiDescription.ActionDescriptor;
            var endpointMetadata = actionDescriptor.EndpointMetadata;

            operation.Deprecated |= apiDescription.IsDeprecated();

            foreach (var responseType in context.Description.SupportedResponseTypes)
            {
                var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
                var response = operation.Responses[responseKey];

                foreach (var contentType in response.Content.Keys)
                {
                    if (responseType.ApiResponseFormats.All(x => x.MediaType != contentType))
                    {
                        response.Content.Remove(contentType);
                    }
                }
            }

            if (operation.Parameters == null)
            {
                return Task.CompletedTask;
            }

  
            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.FirstOrDefault(p => p.Name == parameter.Name);
                if (description == null)
                    continue;

                parameter.Description ??= description.ModelMetadata?.Description;

                if (
                    parameter.Schema.Default == null
                    && description.DefaultValue != null
                    && description.DefaultValue is not DBNull
                    && description.ModelMetadata is { } modelMetadata
                )
                {

                    var json = JsonSerializer.Serialize(description.DefaultValue, modelMetadata.ModelType);
                    parameter.Schema.Default = CreateOpenApiAny(description.DefaultValue);

                   
                }

                parameter.Required |= description.IsRequired;
            }

            return Task.CompletedTask;
        }

        private static IOpenApiAny? CreateOpenApiAny(object? value)
        {
            return value switch
            {
                null => null,
                int i => new OpenApiInteger(i),
                long l => new OpenApiLong(l),
                float f => new OpenApiFloat(f),
                double d => new OpenApiDouble(d),
                bool b => new OpenApiBoolean(b),
                string s => new OpenApiString(s),
                DateTime dt => new OpenApiString(dt.ToString("o")), 
                Enum e => new OpenApiString(e.ToString()),
                _ => new OpenApiString(JsonSerializer.Serialize(value)) 
            };
        }

    }
}
