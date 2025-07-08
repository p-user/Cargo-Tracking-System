
using SharedKernel.HealthChecks.Extensions;

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
builder.Services.AddScoped<AuditableEntityInterceptor>();
builder.Services.AddScoped<DispatchDomainEventInterceptor>();

builder.Services.AddDbContext<TrackingDbContext>((sp, options) =>
{
    var auditableInterceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
    var dispatchInterceptor = sp.GetRequiredService<DispatchDomainEventInterceptor>();

    options.AddInterceptors(auditableInterceptor, dispatchInterceptor);
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));

});


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


builder.Services.AddMassTransit<TrackingDbContext>(
    builder.Configuration,
    assembly,
    dbOutboxConfig =>
    {
        dbOutboxConfig.UsePostgres().UseBusOutbox();
    }
);

builder.Services.AddCommonHealthChecks(builder.Configuration);

//exceptions
builder.Services.AddExceptionHandler<CustomExeptionHandler>();
builder.Services.AddAspnetOpenApi("Tracking API", "v1");

var app = builder.Build();
//ToDo: fix these
//app.UseExceptionHandler();
app.UseAspnetOpenApi("v1");
app.UseRouting();

app.MapCarter();
app.MapHealthCheckEndpoint();

app.MapGet("/", () => "Hello World from Tracking Api!");

await app.RunAsync();
