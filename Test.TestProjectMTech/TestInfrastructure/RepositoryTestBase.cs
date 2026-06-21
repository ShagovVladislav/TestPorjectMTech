using TestProjectMTech.api.Data;

namespace Test.TestProjectMTech.TestInfrastructure;

public abstract class RepositoryTestBase
{
    private DatabaseFixture _database = null!;

    [SetUp]
    public async Task SetUp()
    {
        _database = new DatabaseFixture();
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
