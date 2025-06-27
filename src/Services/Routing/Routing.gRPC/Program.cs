


using MassTransit;

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

builder.Services.AddScoped<AuditableEntityInterceptor>();
builder.Services.AddScoped<DispatchDomainEventInterceptor<RoutingDbContext>>();

builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddGrpcSwagger();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Routing gRPC Service",
        Version = "v1"
    });


});


builder.Services.AddMassTransit<RoutingDbContext>(
    builder.Configuration,
    Assembly.GetExecutingAssembly(),
    dbOutboxConfig =>
    {
        dbOutboxConfig.UseSqlite().UseBusOutbox();
    }
);


var basePath = AppContext.BaseDirectory;
builder.Services.AddDbContext<RoutingDbContext>((sp, options) =>
{
    var auditableInterceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
    var dispatchInterceptor = sp.GetRequiredService<DispatchDomainEventInterceptor<RoutingDbContext>>();

    options.AddInterceptors(auditableInterceptor, dispatchInterceptor);

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
builder.Services.AddHttpClient<GoogleMapsService>();
builder.Services.AddSingleton<GoogleMapsService>();
builder.Services.AddScoped<IRoutingApplicationService,RoutingApplicationService>();

var app = builder.Build();
//app.UseCorrelationId();



// Configure the HTTP request pipeline.
app.UseRouting();

    
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Routing gRPC v1");
});

app.MapGrpcService<RoutingService>();

app.EnsureSeedData();
app.Run();
