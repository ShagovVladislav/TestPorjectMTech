using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Test.TestProjectMTech.TestInfrastructure;
using TestProjectMTech.Api.DTO.Requests;
using TestProjectMTech.Api.DTO.Responses;

namespace Test.TestProjectMTech.Functional;

public class CategoriesApiTests : FunctionalTestBase
{
    [Test]
    public async Task GetCategories_Should_Return_All_Categories()
    {
        var response = await Client.GetAsync("/api/categories");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var categories = await response.Content.ReadFromJsonAsync<List<CategoryResponse>>();
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

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var category = await response.Content.ReadFromJsonAsync<CategoryResponse>();
        category.Should().NotBeNull();
        category!.Id.Should().BeGreaterThan(0);
        category.Name.Should().Be("Аэрогрили");
    }

    [Test]
    public async Task CreateCategory_Should_Return_BadRequest_When_Name_Is_Empty()
    {
        var request = new CreateCategoryRequest
        {
            Name = string.Empty
        };

        var response = await Client.PostAsJsonAsync("/api/categories", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
