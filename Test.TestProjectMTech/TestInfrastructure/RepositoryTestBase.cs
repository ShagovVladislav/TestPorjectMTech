using TestProjectMTech.Api.Data;
using TestProjectMTech.Api.Repositories;

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
        if (_database is not null)
            await _database.DisposeAsync();
    }

    protected WarehouseDbContext CreateContext()
    {
        return _database.CreateContext();
    }

    protected ProductRepository CreateProductRepository()
    {
        return new ProductRepository(_database.CreateContextFactory());
    }

    protected CategoryRepository CreateCategoryRepository()
    {
        return new CategoryRepository(_database.CreateContextFactory());
    }
}
