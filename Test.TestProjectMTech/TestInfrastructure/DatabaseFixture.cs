using Microsoft.EntityFrameworkCore;
using Npgsql;
using TestProjectMTech.Api.Data;

namespace Test.TestProjectMTech.TestInfrastructure;

public sealed class DatabaseFixture : IAsyncDisposable
{
    private const string DefaultConnectionString =
        "Host=localhost;Port=5000;Database=warehouse_db;Username=postgres;Password=postgres";

    public DatabaseFixture()
    {
        var baseConnectionString = Environment.GetEnvironmentVariable("TEST_DB_CONNECTION_STRING")
            ?? DefaultConnectionString;

        var builder = new NpgsqlConnectionStringBuilder(baseConnectionString)
        {
            Database = $"warehouse_tests_{Guid.NewGuid():N}"
        };

        ConnectionString = builder.ConnectionString;
    }

    public string ConnectionString { get; }

    public WarehouseDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<WarehouseDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new WarehouseDbContext(options);
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await using var context = CreateContext();

        await context.Database.EnsureDeletedAsync(cancellationToken);
        await context.Database.EnsureCreatedAsync(cancellationToken);
        await ResetIdentitySequencesAsync(context, cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await using var context = CreateContext();

        await context.Database.EnsureDeletedAsync();
    }

    private static Task ResetIdentitySequencesAsync(
        WarehouseDbContext context,
        CancellationToken cancellationToken)
    {
        return context.Database.ExecuteSqlRawAsync(
            """
            SELECT setval(pg_get_serial_sequence('categories', 'Id'), COALESCE((SELECT MAX("Id") FROM categories), 1));
            SELECT setval(pg_get_serial_sequence('products', 'Id'), COALESCE((SELECT MAX("Id") FROM products), 1));
            """,
            cancellationToken);
    }
}
