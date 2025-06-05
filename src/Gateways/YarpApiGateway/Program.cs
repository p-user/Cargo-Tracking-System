
using Scalar.AspNetCore;
using SharedKernel.OpenApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

//services


builder
    .Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Load Scalar UI
// Add Scalar and OpenAPI discovery
builder.AddAspnetOpenApi(["v1"]);
builder.Services.AddEndpointsApiExplorer();



var app = builder.Build();
app.UseRouting();



//Scalar Endpoints

app.MapGet("/", async (HttpContext context) =>
    {
        await context.Response.WriteAsync("Api Gateway.");
    }
);

// Serve Scalar UI with references to backend OpenAPI specs
app.MapScalarApiReference("/scalar", options =>
{
    options
        .WithOpenApiRoutePattern("/openapi/{documentName}.json")
        .WithTheme(ScalarTheme.Purple)
        .WithDownloadButton(true);
});

// Expose OpenAPI spec index for Scalar UI (manually)
app.MapGet("/openapi/index.json", () => Results.Json(new[]
{
    new { name = "Order API", url = "/openapi/order.json" },
    new { name = "Routing API", url = "/openapi/routing.json" }
}));

app.MapReverseProxy();


await app.RunAsync();
