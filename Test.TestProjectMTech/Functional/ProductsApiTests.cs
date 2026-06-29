using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Test.TestProjectMTech.TestInfrastructure;
using TestProjectMTech.Api.Domain;
using TestProjectMTech.Api.DTO.Requests;
using TestProjectMTech.Api.DTO.Responses;

namespace Test.TestProjectMTech.Functional;

public class ProductsApiTests : FunctionalTestBase
{
    [Test]
    public async Task GetProducts_Should_Return_All_Products()
    {
        var response = await Client.GetAsync("/api/products");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await ReadJsonAsync<PagedResult<ProductResponse>>(response);
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(5);
        result.TotalCount.Should().Be(5);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(20);
        result.TotalPages.Should().Be(1);
        result.Items.Select(product => product.Sku).Should().BeEquivalentTo(
            "TV-SAMSUNG-001",
            "TV-LG-001",
            "PHONE-XIAOMI-001",
            "PHONE-SAMSUNG-001",
            "LAPTOP-LENOVO-001");
    }

    [Test]
    public async Task GetProducts_Should_Filter_By_Category()
    {
        var response = await Client.GetAsync("/api/products?categoryId=1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await ReadJsonAsync<PagedResult<ProductResponse>>(response);
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Items.Should().OnlyContain(product => product.CategoryId == 1);
    }

    [Test]
    public async Task GetProducts_Should_Filter_By_Status()
    {
        var response = await Client.GetAsync("/api/products?status=Active");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await ReadJsonAsync<PagedResult<ProductResponse>>(response);
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Items.Should().OnlyContain(product => product.Status == Status.Active);
    }

    [Test]
    public async Task GetProducts_Should_Return_Requested_Page()
    {
        var response = await Client.GetAsync("/api/products?page=2&pageSize=2");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await ReadJsonAsync<PagedResult<ProductResponse>>(response);
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Page.Should().Be(2);
        result.PageSize.Should().Be(2);
        result.TotalCount.Should().Be(5);
        result.TotalPages.Should().Be(3);
        result.Items.Select(product => product.Sku).Should().ContainInOrder(
            "PHONE-XIAOMI-001",
            "PHONE-SAMSUNG-001");
    }

    [Test]
    public async Task GetProducts_Should_Return_BadRequest_When_Pagination_Is_Invalid()
    {
        var response = await Client.GetAsync("/api/products?page=0&pageSize=2");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task GetProductById_Should_Return_Product_When_Product_Exists()
    {
        var response = await Client.GetAsync("/api/products/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var product = await ReadJsonAsync<ProductResponse>(response);
        product.Should().NotBeNull();
        product.Id.Should().Be(1);
        product.Name.Should().Be("Телевизор Samsung");
        product.Sku.Should().Be("TV-SAMSUNG-001");
        product.Status.Should().Be(Status.Active);
    }

    [Test]
    public async Task GetProductById_Should_Return_NotFound_When_Product_Does_Not_Exist()
    {
        var response = await Client.GetAsync("/api/products/999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task CreateProduct_Should_Return_Created_Product()
    {
        var request = new CreateProductRequest
        {
            Name = "Планшет Huawei",
            Sku = "TABLET-HUAWEI-001",
            CategoryId = 2
        };

        var response = await Client.PostAsJsonAsync("/api/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var product = await ReadJsonAsync<ProductResponse>(response);
        product.Should().NotBeNull();
        product.Id.Should().BeGreaterThan(0);
        product.Name.Should().Be("Планшет Huawei");
        product.Sku.Should().Be("TABLET-HUAWEI-001");
        product.CategoryId.Should().Be(2);
        product.Status.Should().Be(Status.Active);
    }

    [Test]
    public async Task CreateProduct_Should_Return_BadRequest_When_Request_Is_Invalid()
    {
        var request = new CreateProductRequest
        {
            Name = string.Empty,
            Sku = string.Empty,
            CategoryId = 0
        };

        var response = await Client.PostAsJsonAsync("/api/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task CreateProduct_Should_Return_NotFound_When_Category_Does_Not_Exist()
    {
        var request = new CreateProductRequest
        {
            Name = "Планшет Huawei",
            Sku = "TABLET-HUAWEI-001",
            CategoryId = 999
        };

        var response = await Client.PostAsJsonAsync("/api/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task CreateProduct_Should_Return_Conflict_When_Sku_Already_Exists()
    {
        var request = new CreateProductRequest
        {
            Name = "Другой телевизор",
            Sku = "TV-SAMSUNG-001",
            CategoryId = 1
        };

        var response = await Client.PostAsJsonAsync("/api/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Test]
    public async Task ChangeProductStatus_Should_Return_Updated_Product()
    {
        var response = await Client.PatchAsync("/api/products/1/status?status=Defective", null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var product = await ReadJsonAsync<ProductResponse>(response);
        product.Should().NotBeNull();
        product.Id.Should().Be(1);
        product.Status.Should().Be(Status.Defective);
    }

    [Test]
    public async Task ChangeProductStatus_Should_Return_NotFound_When_Product_Does_Not_Exist()
    {
        var response = await Client.PatchAsync("/api/products/999/status?status=Defective", null);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task ChangeProductStatus_Should_Return_BadRequest_When_Status_Is_Missing()
    {
        var response = await Client.PatchAsync("/api/products/1/status", null);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task ChangeProductStatus_Should_Return_Conflict_When_Transition_Is_Not_Allowed()
    {
        var response = await Client.PatchAsync("/api/products/1/status?status=WriteOff", null);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
