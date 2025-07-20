using JasperFx.Events;
using Marten;
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

#region Db_interceptos



builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("DefaultConnection"));
    opts.Events.StreamIdentity = StreamIdentity.AsString;
    opts.Events.AddEventType(typeof(CargoTrackingInitiated));
    opts.Events.AddEventType(typeof(CargoStatusUpdated));
})
.UseLightweightSessions()
.ApplyAllDatabaseChangesOnStartup();

builder.Services.AddScoped<IDocumentSession>(sp =>
{
    var store = sp.GetRequiredService<IDocumentStore>();
    var publishEndpoint = sp.GetRequiredService<IPublishEndpoint>();

    var sessionOptions = new Marten.Services.SessionOptions
    {
        Tracking=DocumentTracking.IdentityOnly
    };

    var session = store.OpenSession(sessionOptions);
    session.Listeners.Add(new MartenDomainEventDispatcher(publishEndpoint));
    return session;
});

builder.Services.AddDbContext<TrackingDbContext>(options =>
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

#endregion


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
//ToDo: fix these
app.UseExceptionHandler();
app.UseAspnetOpenApi("v1");
app.UseRouting();

app.MapCarter();
app.MapHealthCheckEndpoint();

app.MapGet("/", () => "Hello World from Tracking Api!");

await app.RunAsync();

