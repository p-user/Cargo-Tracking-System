


using Serilog;
using SharedKernel.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(builder.Configuration);
builder.Host.UseSerilog();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.MapGet("/", () => "Hello World!");

app.Run();
