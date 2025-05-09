using SharedKernel.Messaging.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
