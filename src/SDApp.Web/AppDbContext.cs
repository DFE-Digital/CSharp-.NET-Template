using Microsoft.EntityFrameworkCore;

namespace SDApp.Web;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public const string ConnectionName = "AppDb";
    
    public static void Configure(DbContextOptionsBuilder builder, string connectionString) =>
        builder
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .UseSeeding((context, _) => ((AppDbContext)context).SeedData())
            .UseAsyncSeeding((context, _, cancellationToken) => ((AppDbContext)context).SeedDataAsync(cancellationToken));
    
#pragma warning disable CA1822  // Method can be static
    private void SeedData()
    {
        // Add seed data here; ensure that both SeedData() and SeedDataAsync() are implemented
        // https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding
    }

    private async Task SeedDataAsync(CancellationToken cancellationToken = default)
    {
        // Add seed data here; ensure that both SeedData() and SeedDataAsync() are implemented
        // https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding
    }
#pragma warning restore CA1822
}