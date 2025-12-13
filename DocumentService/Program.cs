using DotNetEnv;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using DocumentService.src.Interface;
using DocumentService.src.repository;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables de entorno
Env.Load();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar el repositorio como Singleton para mantener los datos en memoria
builder.Services.AddSingleton<IDocumentRepository, DocumentRepository>();

builder.Services.AddHttpContextAccessor();


// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "InsightFlow Documents API v1");
        c.RoutePrefix = string.Empty; // Swagger UI en la raíz
    });
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new
{
    Status = "Healthy",
    Service = "Documents Service",
    Timestamp = DateTime.UtcNow,
    Storage = "In-Memory"
}))
.WithName("HealthCheck")
.WithTags("Health");

app.MapControllers();

app.Run();