using Serilog.Debugging;
SelfLog.Enable(Console.Error);
var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

string environment = builder.Environment.EnvironmentName;
string serviceName = builder.Environment.ApplicationName;

#region Serilog
Serilogger.ConfigureLogger(environment, serviceName);
builder.Host.UseSerilog();

//Add Correlation ID middleware
builder.Services.AddDefaultCorrelationId(options =>
{
    options.UpdateTraceIdentifier = true;
    options.IncludeInResponse = true;
    options.ResponseHeader = "X-Correlation-ID";
});

#endregion


builder.Services.AddScoped<AuditableEntityInterceptor>();
builder.Services.AddScoped<DispatchDomainEventInterceptor<OrderDbContext>>();

builder.Services.AddDbContext<OrderDbContext>((sp, options) =>
{
    var auditableInterceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
    var dispatchInterceptor = sp.GetRequiredService<DispatchDomainEventInterceptor<OrderDbContext>>();

    options.AddInterceptors(auditableInterceptor, dispatchInterceptor);
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
   

});


builder.Services.AddAutoMapper(assembly);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddCarter();
builder.Services.AddHostedService<OutboxProcessor<OrderDbContext>>();

builder.Services.AddMassTransit(builder.Configuration, assembly);

//execeptions
builder.Services.AddExceptionHandler<CustomExeptionHandler>();

var app = builder.Build();
app.UseCorrelationId();
app.UseSerilogRequestLogging();
app.ApplyMigrations<OrderDbContext>();
app.UseRouting();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    
}
app.MapCarter();

Log.Information("Test log");
app.Run();

AppDomain.CurrentDomain.ProcessExit += (s, e) => Serilogger.CloseAndFlush();
