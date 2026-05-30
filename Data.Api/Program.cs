using Data.Api.Endpoints;
using Data.Core.Address.Interfaces;
using Data.Core.Address.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAddressParser, AddressParser>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var v1 = app.MapGroup("/v1");
v1.MapAddressEndpoints();

app.Run();