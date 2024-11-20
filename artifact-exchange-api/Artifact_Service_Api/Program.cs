using System.Text.Json.Serialization;
using Artifact_Service_Api.AppData;
using Artifact_Service_Api.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFileStorageService, FileStorageSercvice>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Test",
                          policy =>
                          {
                              policy.WithOrigins("http://127.0.0.1:5500")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // Здесь я указал свою строку подключения

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Test");
app.MapControllers();

app.Run();