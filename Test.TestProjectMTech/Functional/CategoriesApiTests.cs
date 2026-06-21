using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Test.TestProjectMTech.TestInfrastructure;
using TestProjectMTech.api.Domain;
using TestProjectMTech.api.DTO.Requests;

namespace Test.TestProjectMTech.Functional;

public class CategoriesApiTests : FunctionalTestBase
{
    [Test]
    public async Task GetCategories_Should_Return_All_Categories()
    {
        var response = await Client.GetAsync("/api/categories");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var categories = await response.Content.ReadFromJsonAsync<List<Category>>();
        categories.Should().NotBeNull();
        categories.Should().HaveCount(3);
        categories!.Select(category => category.Name).Should().BeEquivalentTo(
            "Телевизоры",
            "Смартфоны",
            "Ноутбуки");
    }

    [Test]
    public async Task CreateCategory_Should_Return_Created_Category()
    {
        var request = new CreateCategoryRequest
        {
            Name = "Аэрогрили"
        };

        var response = await Client.PostAsJsonAsync("/api/categories", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var category = await response.Content.ReadFromJsonAsync<Category>();
        category.Should().NotBeNull();
        category!.Id.Should().BeGreaterThan(0);
        category.Name.Should().Be("Аэрогрили");
    }

    [Test]
    public async Task CreateCategory_Should_Return_BadRequest_When_Name_Is_Too_Short()
    {
        var request = new CreateCategoryRequest
        {
            Name = "AB"
        };

        var response = await Client.PostAsJsonAsync("/api/categories", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
