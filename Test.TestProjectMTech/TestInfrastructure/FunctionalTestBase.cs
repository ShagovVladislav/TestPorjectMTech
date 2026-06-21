namespace Test.TestProjectMTech.TestInfrastructure;

public abstract class FunctionalTestBase
{
    private DatabaseFixture _database = null!;
    private CustomWebApplicationFactory _factory = null!;

    protected HttpClient Client { get; private set; } = null!;

    [SetUp]
    public async Task SetUp()
    {
        var connectionString = await TestPostgresContainer.GetConnectionStringAsync();
        _database = new DatabaseFixture(connectionString);

        _factory = new CustomWebApplicationFactory(_database.ConnectionString);
        Client = _factory.CreateClient();
    }

    [TearDown]
    public async Task TearDown()
    {
        Client?.Dispose();
        
        if (_factory is not null)
            await _factory.DisposeAsync();
        
        if (_database is not null)
            await _database.DisposeAsync();
    }
}
