using CakeShop.Business.Services;
using CakeShop.Core.Interfaces;
using CakeShop.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CakeShop API",
        Version = "v1",
        Description = "甜蜜烘焙坊 多語系購物網站 API（SHA-256 密碼雜湊 + AES-256-GCM Token 加密）"
    });
});

// 加密服務（Singleton，無狀態）
builder.Services.AddSingleton<IEncryptionService, EncryptionService>();

// Repository（Singleton 保留記憶體資料）
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<ICartRepository, CartRepository>();
builder.Services.AddSingleton<IUserRepository>(sp =>
    new UserRepository(sp.GetRequiredService<IEncryptionService>()));

// Business Services
builder.Services.AddSingleton<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IContactService, ContactService>();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CakeShop API v1");
    c.RoutePrefix = "swagger";
});

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
