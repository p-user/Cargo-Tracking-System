
using Scalar.AspNetCore;
using SharedKernel.OpenApi;
using SharedKernel.OpenApi.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


// Add Scalar + OpenAPI
builder.Services.AddOpenApi();


var app = builder.Build();

app.UseRouting();

app.MapReverseProxy();

app.MapScalarApiReference(options =>
{
    options.WithTitle("Cargo Tracking API Gateway");
    options.DefaultFonts = false;

    // Add a document entry for each microservice.
    // Note: The routePattern ('/openapi/order.json') must match the YARP route path.Ndryshe spunon

    options.AddDocument(
        documentName: "Orders",
        title: "Orders Service API",
        routePattern: "/openapi/order.json"
    );


    options.AddDocument(
        documentName: "Route",
        title: "Route Service API",
        routePattern: "/openapi/route.json"
    );

});




await app.RunAsync();
