using Microsoft.EntityFrameworkCore;
using IOC;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuração do FluentValidation (apenas ativação do pipeline)
builder.Services.AddFluentValidationAutoValidation();

// Configurar a injeção de dependência usando a camada IOC
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "My API V1"));
}

app.UseAuthorization();

app.MapControllers();

app.Run();
