using EC.CommonService.Services;
using CakeShop.Core.Interfaces;
using CakeShop.Infrastructure.Data;
using CakeShop.Infrastructure.Repositories;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// ── DbContext ────────────────────────────────────────────────────────
builder.Services.AddDbContext<CakeShopDbContext>(options =>
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("Default"))
        .UseSnakeCaseNamingConvention());

// ── Encryption（Singleton，無狀態）────────────────────────────────────
builder.Services.AddSingleton<IEncryptionService>(
    sp => new EncryptionService(sp.GetRequiredService<IConfiguration>()));

// ── Repositories（Scoped，與 DbContext 生命週期一致）───────────────────
builder.Services.AddScoped<IProductRepository,      ProductRepository>();
builder.Services.AddScoped<IUserRepository,         UserRepository>();
builder.Services.AddScoped<ICartRepository,         CartRepository>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();

// ── Business Services（Scoped）────────────────────────────────────────
builder.Services.AddScoped<IEmailService,        EmailService>();
builder.Services.AddScoped<IAuthService,         AuthService>();
builder.Services.AddScoped<IProductService,      ProductService>();
builder.Services.AddScoped<ICartService,         CartService>();
builder.Services.AddScoped<IContactService,      ContactService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();

// ── System Log ────────────────────────────────────────────────────────
builder.Services.AddSingleton<ISystemLogService, SystemLogService>();

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
        Title       = "CakeShop API",
        Version     = "v1",
        Description = "甜蜜烘焙坊 API（Entity Framework Core + PostgreSQL）"
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
            logger.LogInformation("✔ 資料庫連線成功（{Db}）",
                ctx.Database.GetDbConnection().Database);
        else
            logger.LogWarning("⚠ 無法連線至資料庫，請確認 PostgreSQL 服務與連線字串");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "✗ 資料庫連線失敗");
    }
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

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

// ── Swagger（僅開發環境或設定啟用時）────────────────────────────────────
if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("EnableSwagger"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CakeShop API v1");
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
