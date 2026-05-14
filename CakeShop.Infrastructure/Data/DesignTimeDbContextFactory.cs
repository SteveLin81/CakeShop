using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CakeShop.Infrastructure.Data;

// 供 dotnet ef migrations 設計時期使用
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CakeShopDbContext>
{
    // 讀取環境變數 DB_CONN，若未設定則使用開發預設值
    private const string DefaultConn =
        "Host=127.0.0.1;Port=5432;Database=TESTDB;Username=testdb0101;Password=testdb0101";

    public CakeShopDbContext CreateDbContext(string[] args)
    {
        var connStr = Environment.GetEnvironmentVariable("DB_CONN") ?? DefaultConn;
        var options = new DbContextOptionsBuilder<CakeShopDbContext>()
            .UseNpgsql(connStr)
            .UseSnakeCaseNamingConvention()
            .Options;
        return new CakeShopDbContext(options);
    }
}
