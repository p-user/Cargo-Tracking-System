using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(assembly);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCarter();

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
