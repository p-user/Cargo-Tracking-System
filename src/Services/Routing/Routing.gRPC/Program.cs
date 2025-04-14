using Microsoft.EntityFrameworkCore;
using Routing.gRPC.Data;
using Routing.gRPC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddDbContext<RoutingDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<RoutingService>();
app.EnsureSeedData();
app.Run();
