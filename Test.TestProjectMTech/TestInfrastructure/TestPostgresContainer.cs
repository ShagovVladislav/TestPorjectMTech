using Testcontainers.PostgreSql;

namespace Test.TestProjectMTech.TestInfrastructure;

public static class TestPostgresContainer
{
    private static readonly SemaphoreSlim Lock = new(1, 1);
    private static PostgreSqlContainer? _container;

    public static async Task<string> GetConnectionStringAsync()
    {
        if (_container is not null)
            return _container.GetConnectionString();

        await Lock.WaitAsync();
        
        try
        {
            if (_container is null)
            {
                _container = new PostgreSqlBuilder("postgres:16")
                    .WithDatabase("warehouse_tests")
                    .WithUsername("postgres")
                    .WithPassword("postgres")
                    .Build();

                await _container.StartAsync();
            }

            return _container.GetConnectionString();
        }
        finally
        {
            Lock.Release();
        }
    }

    public static async ValueTask DisposeAsync()
    {
        if (_container is null)
            return;

        await _container.DisposeAsync();
        _container = null;
    }
}
