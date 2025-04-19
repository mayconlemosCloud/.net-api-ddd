using Microsoft.EntityFrameworkCore;
using Infrastructure;
using IOC;
using Application.Mappings;
using AutoMapper;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuração do FluentValidation (apenas ativação do pipeline)
builder.Services.AddFluentValidationAutoValidation();

// Configurar a injeção de dependência usando a camada IOC
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
