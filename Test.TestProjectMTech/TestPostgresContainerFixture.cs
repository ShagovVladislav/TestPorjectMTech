using Test.TestProjectMTech.TestInfrastructure;

namespace Test.TestProjectMTech;

[SetUpFixture]
public sealed class TestPostgresContainerFixture
{
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await TestPostgresContainer.DisposeAsync();
    }
}
