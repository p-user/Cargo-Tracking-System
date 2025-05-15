

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<AuditableEntityInterceptor>();
builder.Services.AddScoped<DispatchDomainEventInterceptor<RoutingDbContext>>();
builder.Services.AddGrpc();
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

// Configure the HTTP request pipeline.
app.MapGrpcService<RoutingService>();
app.EnsureSeedData();
app.Run();
