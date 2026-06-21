namespace Test.TestProjectMTech.TestInfrastructure;

public abstract class FunctionalTestBase
{
    private DatabaseFixture _database = null!;
    private CustomWebApplicationFactory _factory = null!;

    protected HttpClient Client { get; private set; } = null!;

    [SetUp]
    public async Task SetUp()
    {
        _database = new DatabaseFixture();
        await _database.InitializeAsync();

        _factory = new CustomWebApplicationFactory(_database.ConnectionString);
        Client = _factory.CreateClient();
    }

    [TearDown]
    public async Task TearDown()
    {
        Client.Dispose();
        await _factory.DisposeAsync();
        await _database.DisposeAsync();
    }
}
