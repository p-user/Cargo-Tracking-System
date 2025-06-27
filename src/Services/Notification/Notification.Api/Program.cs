using CorrelationId;
using CorrelationId.DependencyInjection;
using MassTransit;
using Serilog;
using SharedKernel.Core.Extensions;
using SharedKernel.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureCustomKestrelForRest();

#region Serilog

string environment = builder.Environment.EnvironmentName;
string serviceName = builder.Environment.ApplicationName;


builder.Host.UseSerilog();

//Add Correlation ID middleware
builder.Services.AddDefaultCorrelationId(options =>
{
    options.UpdateTraceIdentifier = true;
    options.IncludeInResponse = true;
    options.ResponseHeader = "X-Correlation-ID";
});

#endregion

//builder.Services.AddMassTransit(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseCorrelationId();

app.MapGet("/", () => "Hello World!");

app.Run();
