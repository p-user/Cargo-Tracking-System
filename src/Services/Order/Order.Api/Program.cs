
using SharedKernel.Core.Exeptions.Handlers;

SelfLog.Enable(Console.Error);
var builder = WebApplication.CreateBuilder(args);


var assembly = typeof(Program).Assembly;

string environment = builder.Environment.EnvironmentName;
string serviceName = builder.Environment.ApplicationName;

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



//Add Correlation ID middleware
//builder.Services.AddDefaultCorrelationId(options =>
//{
//    options.UpdateTraceIdentifier = true;
//    options.IncludeInResponse = true;
//    options.ResponseHeader = "X-Correlation-ID";
//});

#endregion

#region Db_interceptos
builder.Services.AddScoped<AuditableEntityInterceptor>();
builder.Services.AddScoped<DispatchDomainEventInterceptor<OrderDbContext>>();

builder.Services.AddDbContext<OrderDbContext>((sp, options) =>
{
    var auditableInterceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
    var dispatchInterceptor = sp.GetRequiredService<DispatchDomainEventInterceptor<OrderDbContext>>();

    options.AddInterceptors(auditableInterceptor, dispatchInterceptor);
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
   

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
builder.Services.AddHostedService<OutboxProcessor<OrderDbContext>>();
builder.Services.AddMassTransit(builder.Configuration, assembly);

//execeptions
builder.Services.AddExceptionHandler<CustomExeptionHandler>();
builder.Services.AddAspnetOpenApi("Order API", "v1");


var app = builder.Build();
//app.UseCorrelationId();

app.ApplyMigrations<OrderDbContext>();
app.UseRouting();
app.MapCarter();
app.UseAspnetOpenApi("v1");

app.UseHttpsRedirection();

Log.Information("Test log");
await app.RunAsync();

