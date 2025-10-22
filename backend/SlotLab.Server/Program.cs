using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SlotLab.Engine.Core;
using SlotLab.Server.Hubs;


using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SlotLab API",
        Version = "v1",
        Description = "Backend API + SignalR Hub per al projecte SlotLab"
    });
});

// 🔹 Política CORS pel frontend (Phaser, HTML local, etc.)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:8080")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// 🔹 Controladors REST
builder.Services.AddControllers();

// 🔹 SignalR i GameManager
builder.Services.AddSignalR();
builder.Services.AddSingleton<GameManager>(_ => new GameManager("slotConfig.json"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ✅ Habilitem CORS abans dels endpoints
app.UseCors("AllowFrontend");

// ⚠️ HTTPS redirection és desactivat en desenvolupament per evitar conflictes
// app.UseHttpsRedirection();
// 📝 IMPORTANT: Re-enable HTTPS when deploying to production (with a valid certificate)

// ✅ Routing dels controladors REST
app.MapControllers();

// ✅ Endpoint del Hub SignalR (WebSocket)
app.MapHub<GameHub>("/gamehub").RequireCors("AllowFrontend");

app.Run();
