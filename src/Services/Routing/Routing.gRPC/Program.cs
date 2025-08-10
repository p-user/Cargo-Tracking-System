

using SharedKernel.Core.Exeptions.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureCustomKestrelForGrpc();

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



//Add Correlation ID middleware
//builder.Services.AddDefaultCorrelationId(options =>
//{
//    options.UpdateTraceIdentifier = true;
//    options.IncludeInResponse = true;
//    options.ResponseHeader = "X-Correlation-ID";
//});

#endregion


#region Database 
var basePath = AppContext.BaseDirectory;
builder.Services.AddDbContext<RoutingDbContext>((sp, options) =>
{
    var auditableInterceptor = sp.GetRequiredService<AuditableEntityInterceptor>();

    options.AddInterceptors(auditableInterceptor);

    string dbPath;
    if (builder.Environment.IsEnvironment("Local")) //ToDo: set static file for env
    {
        var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var localDbFolder = Path.Combine(appDataFolder, "CargoTrackingSystem");
        Directory.CreateDirectory(localDbFolder);
        dbPath = Path.Combine(localDbFolder, "routingDb.sqlite");
    }
    else
    {
        var dataFolder = Path.Combine(AppContext.BaseDirectory, "data");
        Directory.CreateDirectory(dataFolder);
        dbPath = Path.Combine(dataFolder, "routingDb.sqlite");
    }
    var connectionString = $"Data Source={dbPath}";
    options.UseSqlite(connectionString);


});

#endregion

builder.Services.AddScoped<AuditableEntityInterceptor>();
builder.Services.AddUnitOfWorkWithOutbox<RoutingDbContext>();
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddGrpcSwagger();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAspnetOpenApi("Routing gRPC", "v1");
builder.Services.AddUnitOfWorkWithOutbox<RoutingDbContext>();
builder.Services.AddMassTransit<RoutingDbContext>(
    builder.Configuration,
    Assembly.GetExecutingAssembly(),
    dbOutboxConfig =>
    {
        dbOutboxConfig.UseSqlite();
        dbOutboxConfig.UseBusOutbox();
    }
);


builder.Services.AddHttpClient<GoogleMapsService>();
builder.Services.AddSingleton<GoogleMapsService>();
builder.Services.AddScoped<IRoutingApplicationService,RoutingApplicationService>();

builder.Services.AddCommonHealthChecks(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
var app = builder.Build();
//app.UseCorrelationId();



// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.UseRouting();
app.MapHealthCheckEndpoint();

app.UseAspnetOpenApi("v1");

app.MapGrpcService<RoutingService>();

app.EnsureSeedData();
app.Run();
