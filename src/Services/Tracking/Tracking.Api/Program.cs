


using Serilog;
using SharedKernel.Core.Extensions;
using SharedKernel.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureCustomKestrelForRest();
builder.Services.AddMassTransit(builder.Configuration);
builder.Host.UseSerilog();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.MapGet("/", () => "Hello World!");

app.Run();
