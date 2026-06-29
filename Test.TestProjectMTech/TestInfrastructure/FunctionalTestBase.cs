using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Test.TestProjectMTech.TestInfrastructure;

public abstract class FunctionalTestBase
{
    private static readonly JsonSerializerOptions SerializerOptions = CreateSerializerOptions();
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

    protected static Task<T?> ReadJsonAsync<T>(HttpResponseMessage response)
    {
        return response.Content.ReadFromJsonAsync<T>(SerializerOptions);
    }

    private static JsonSerializerOptions CreateSerializerOptions()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        return options;
    }
}
