using DDDExample.Infrastructure;
using DDDExample.Infrastructure.Persistence;
using DDDExample.Application;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using DDDExample.Domain.Repositories;
using DDDExample.Infrastructure.Repositories.MongoDB;
using DDDExample.Middleware;


var builder = WebApplication.CreateBuilder(args);

// MongoDB settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

// Registrar middleware + repositorio
builder.Services.AddResponseTimeMiddleware();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseTimeMiddleware(); 
app.UseAuthorization();
app.MapControllers();
app.Run();


