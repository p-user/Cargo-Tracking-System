

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureCustomKestrelForGrpc(builder.Environment.EnvironmentName);

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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddGrpcSwagger();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Routing gRPC Service",
        Version = "v1"
    });

   

});


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
