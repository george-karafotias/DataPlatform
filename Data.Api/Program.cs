using Data.Api.Endpoints;
using Data.Core.Address.Interfaces;
using Data.Core.Address.Services;
using Data.Core.Greeklish.Interfaces;
using Data.Core.Greeklish.Services;

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

var app = builder.Build();

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

app.Run();