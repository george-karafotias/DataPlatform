using Data.Api.Endpoints;
using Data.Core.Address.Interfaces;
using Data.Core.Address.Services;
using Data.Core.Greeklish.Interfaces;
using Data.Core.Greeklish.Services;
using System.Text.Json.Serialization;
using Data.Core.Parking.Interfaces;
using Data.Core.Parking.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "GK APIs",
        Version = "v1",
        Description = "Collection of utility APIs"
    });
});

builder.Services.AddScoped<IAddressParser, AddressParser>();
builder.Services.AddScoped<IGreeklishConverter, GreeklishConverter>();
builder.Services.AddSingleton<IParkingService, ParkingService>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter());
});

// 👇 CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "https://thess-parking-app.netlify.app"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowAngular");

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI();
//}

app.MapGet("/", () => Results.Ok(new
{
    Name = "Data API",
    Version = "1.0"
}));

app.MapGet("/health", () => Results.Ok(new
{
    Status = "Healthy",
    Timestamp = DateTime.UtcNow
}));

var v1 = app.MapGroup("/v1");
v1.MapAddressEndpoints();
v1.MapGreeklishEndpoints();
v1.MapParkingEndpoints();

app.Run();