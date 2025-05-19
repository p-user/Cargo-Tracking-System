

using CorrelationId;
using CorrelationId.DependencyInjection;
using Scalar.AspNetCore;
using Serilog;
using Shared.Logging;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Serilog

string environment = builder.Environment.EnvironmentName;
string serviceName = builder.Environment.ApplicationName;

Serilogger.ConfigureLogger(environment, serviceName);
builder.Host.UseSerilog();

//Add Correlation ID middleware
builder.Services.AddDefaultCorrelationId(options =>
{
    options.UpdateTraceIdentifier = true;
    options.IncludeInResponse = true;
    options.ResponseHeader = "X-Correlation-ID";
});

#endregion

builder.Services.AddScoped<AuditableEntityInterceptor>();
builder.Services.AddScoped<DispatchDomainEventInterceptor<RoutingDbContext>>();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddMassTransit(builder.Configuration,Assembly.GetExecutingAssembly());
builder.Services.AddHostedService<OutboxProcessor>();
builder.Services.AddDbContext<RoutingDbContext>((sp, options) =>
{
    var auditableInterceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
    var dispatchInterceptor = sp.GetRequiredService<DispatchDomainEventInterceptor<RoutingDbContext>>();

    options.AddInterceptors(auditableInterceptor, dispatchInterceptor);
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));

});
builder.Services.AddHttpClient<GoogleMapsService>();
builder.Services.AddSingleton<GoogleMapsService>();
builder.Services.AddScoped<IRoutingApplicationService,RoutingApplicationService>();
builder.Services.AddOpenApi();
builder.Host.UseSerilog();

var app = builder.Build();
app.UseCorrelationId();
app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Configure the HTTP request pipeline.
app.MapGrpcService<RoutingService>();
app.EnsureSeedData();
app.Run();
