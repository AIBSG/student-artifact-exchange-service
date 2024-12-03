using System.Text.Json.Serialization;
using Artifact_Service_Api.AppData;
using Artifact_Service_Api.Extensions;
using Artifact_Service_Api.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<JwtOptions>(builder.Configuration.GetRequiredSection(nameof(JwtOptions)));
builder.Services.AddAddAuthentication(builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>());

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFileStorageService, FileStorageSercvice>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ITagService, TagService>();



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
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();


app.UseCors("Test");
app.MapControllers();
app.Run();