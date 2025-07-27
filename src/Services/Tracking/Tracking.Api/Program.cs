using JasperFx.Events;
using Marten;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Core.DDD;
using SharedKernel.Messaging.Extensions;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;
builder.WebHost.ConfigureCustomKestrelForRest();

#region Serilog
var serilogOptions = builder.Configuration.BindOptions<SerilogOptions>();

// Conditionally enable Serilog
if (serilogOptions.Enabled)
{
    builder.AddCustomSerilog(configurator: opt =>
    {
        opt.ElasticSearchUrl = serilogOptions.ElasticSearchUrl;
        opt.UseConsole = serilogOptions.UseConsole;
        opt.LogTemplate = serilogOptions.LogTemplate;
    });
}
#endregion


builder.Services.AddScoped(typeof(AggregateRepository<>));

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("DefaultConnection"));
    opts.Events.StreamIdentity = StreamIdentity.AsGuid;
    opts.Events.AddEventTypes(
        assembly.GetTypes().Where(t => t.IsClass && typeof(IDomainEvent).IsAssignableFrom(t))
    );
   

})
.UseLightweightSessions()
.ApplyAllDatabaseChangesOnStartup();


builder.Services.AddDbContext<TrackingDbContext>((sp,options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddMassTransit<TrackingDbContext>(
    builder.Configuration,
    Assembly.GetExecutingAssembly(),
    dbOutboxConfig =>
    {
        dbOutboxConfig.UsePostgres().UseBusOutbox();
    }
);



builder.Services.AddAutoMapper(assembly);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddCarter();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddCommonHealthChecks(builder.Configuration);

//exceptions
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddAspnetOpenApi("Tracking API", "v1");

var app = builder.Build();
app.UseExceptionHandler(options => { });
app.UseAspnetOpenApi("v1");
app.UseRouting();

app.MapCarter();
app.MapHealthCheckEndpoint();

app.MapGet("/", () => "Hello World from Tracking Api!");

await app.RunAsync();

