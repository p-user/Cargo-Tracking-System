
using Microsoft.EntityFrameworkCore.Diagnostics;
using Order.Api.Processors;
using SharedKernel.Data.Interceptors;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;


builder.Services.AddScoped<AuditableEntityInterceptor>();
builder.Services.AddScoped<DistpachDomainEventInterceptor>();

builder.Services.AddDbContext<OrderDbContext>((sp, options) =>
{
    var auditableInterceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
    var dispatchInterceptor = sp.GetRequiredService<DistpachDomainEventInterceptor>();

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
builder.Services.AddCarter();
builder.Services.AddHostedService<OutboxProcessor>();

builder.Services.AddMassTransit(builder.Configuration, assembly);

//execeptions
builder.Services.AddExceptionHandler<CustomExeptionHandler>();

var app = builder.Build();

app.ApplyMigrations<OrderDbContext>();


app.MapCarter();
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//    app.MapScalarApiReference(options =>
//    {
//        options
//                .WithTitle("Cargo Tracking System")
//                .WithTheme(ScalarTheme.Purple)
//                .WithDownloadButton(true)
//                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
//    });
//}


app.Run();
