using TestProjectMTech.Api.Data;

namespace Test.TestProjectMTech.TestInfrastructure;

public abstract class RepositoryTestBase
{
    private DatabaseFixture _database = null!;

    [SetUp]
    public async Task SetUp()
    {
        var connectionString = await TestPostgresContainer.GetConnectionStringAsync();
        _database = new DatabaseFixture(connectionString);
        
        await _database.InitializeAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _database.DisposeAsync();
    }

    protected WarehouseDbContext CreateContext()
    {
        return _database.CreateContext();
    }
}
