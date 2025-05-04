
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddMassTransit(builder.Configuration);

builder.Services.AddDbContext<RoutingDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<GoogleMapsService>();
builder.Services.AddSingleton<GoogleMapsService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<RoutingService>();
app.EnsureSeedData();
app.Run();
