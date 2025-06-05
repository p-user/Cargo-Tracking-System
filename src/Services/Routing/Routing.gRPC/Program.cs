

using SharedKernel.Logging;
using SharedKernel.Logging.Extensions;
using SharedKernel.OpenApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddAspnetOpenApi(["v1"]);

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
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
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

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
    //app.MapOpenApi();
   // app.MapScalarApiReference();
}

// Configure the HTTP request pipeline.
app.UseRouting();
app.MapGrpcService<RoutingService>();
app.UseAspnetOpenApi();
app.EnsureSeedData();
app.Run();
