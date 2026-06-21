using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.TestProjectMTech.TestInfrastructure;
using TestProjectMTech.Api.Domain;
using TestProjectMTech.Api.Repositories;

namespace Test.TestProjectMTech.Integration;

public class CategoryRepositoryTests : RepositoryTestBase
{
    [Test]
    public async Task GetAllCategories_Should_Return_All_Categories()
    {
        await using var context = CreateContext();
        var repository = new CategoryRepository(context);

        var result = await repository.GetAllCategories(CancellationToken.None);

        result.Should().HaveCount(3);
        result.Select(category => category.Name).Should().BeEquivalentTo(
            "Телевизоры",
            "Смартфоны",
            "Ноутбуки");
    }

    [Test]
    public async Task CreateCategory_Should_Save_Category()
    {
        await using var context = CreateContext();
        var repository = new CategoryRepository(context);

        var result = await repository.CreateCategory(
            new Category
            {
                Name = "Аэрогрили"
            },
            CancellationToken.None);

        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be("Аэрогрили");

        await using var verificationContext = CreateContext();
        var savedCategory = await verificationContext.Categories
            .AsNoTracking()
            .SingleAsync(category => category.Id == result.Id);

        savedCategory.Name.Should().Be("Аэрогрили");
    }
}
