using CakeShop.Core.Interfaces;
using CakeShop.Infrastructure.Data;
using CakeShop.Infrastructure.Repositories;
using EC.B2E.Filters;
using EC.CommonService.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// ── DbContext ─────────────────────────────────────────────────────────
builder.Services.AddDbContext<CakeShopDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
           .UseSnakeCaseNamingConvention());

// ── Encryption（Singleton）────────────────────────────────────────────
builder.Services.AddSingleton<IEncryptionService>(
    sp => new EncryptionService(sp.GetRequiredService<IConfiguration>()));

// ── B2E 專屬 Repository / Service ─────────────────────────────────────
builder.Services.AddScoped<IB2eUserRepository,            B2eUserRepository>();
builder.Services.AddScoped<IB2eAuthService,               B2eAuthService>();
builder.Services.AddScoped<B2eAuthFilter>();

// ── 商品管理 ──────────────────────────────────────────────────────────
builder.Services.AddScoped<IProductRepository,            ProductRepository>();
builder.Services.AddScoped<IProductService,               ProductService>();
builder.Services.AddScoped<IProductManagementService,     ProductManagementService>();

// ── 公告管理 ──────────────────────────────────────────────────────────
builder.Services.AddScoped<IAnnouncementRepository,       AnnouncementRepository>();
builder.Services.AddScoped<IAnnouncementManagementService, AnnouncementManagementService>();

// ── B2C 帳號管理 ─────────────────────────────────────────────────────
builder.Services.AddScoped<IUserRepository,               UserRepository>();
builder.Services.AddScoped<IB2cUserManagementService,     B2cUserManagementService>();

// ── Rate Limiting ─────────────────────────────────────────────────────
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("login", opt =>
    {
        opt.PermitLimit         = 5;
        opt.Window              = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit          = 0;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// ── CORS ─────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(p =>
    {
        var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
        if (origins is { Length: > 0 })
            p.WithOrigins(origins).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
        else
            p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    }));

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "CakeShop B2E Admin API",
        Version     = "v1",
        Description = "後台管理 API（商品、公告、B2C帳號）"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name        = "Authorization",
        Type        = SecuritySchemeType.Http,
        Scheme      = "Bearer",
        In          = ParameterLocation.Header,
        Description = "輸入 B2E Token"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ── 啟動時驗證資料庫連線 ──────────────────────────────────────────────
await using (var scope = app.Services.CreateAsyncScope())
{
    var ctx    = scope.ServiceProvider.GetRequiredService<CakeShopDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        if (await ctx.Database.CanConnectAsync())
            logger.LogInformation("✔ 資料庫連線成功（{Db}）", ctx.Database.GetDbConnection().Database);
        else
            logger.LogWarning("⚠ 無法連線至資料庫");
    }
    catch (Exception ex) { logger.LogError(ex, "✗ 資料庫連線失敗"); }
}

// ── Security Headers ─────────────────────────────────────────────────
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"]  = "nosniff";
    context.Response.Headers["X-Frame-Options"]         = "SAMEORIGIN";
    context.Response.Headers["X-XSS-Protection"]        = "1; mode=block";
    context.Response.Headers["Referrer-Policy"]         = "strict-origin-when-cross-origin";
    context.Response.Headers["Permissions-Policy"]      = "geolocation=(), microphone=(), camera=()";
    await next();
});

app.UseHttpsRedirection();

// ── Swagger（僅開發環境或設定啟用時）────────────────────────────────────
if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("EnableSwagger"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "B2E Admin API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseStaticFiles();
app.UseCors();
app.UseRateLimiter();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.Run();
