using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.TestProjectMTech.TestInfrastructure;
using TestProjectMTech.api.Domain;
using TestProjectMTech.api.DTO.Requests;
using TestProjectMTech.api.Exceptions;
using TestProjectMTech.api.Repositories;

namespace Test.TestProjectMTech.Integration;

public class ProductRepositoryTests : RepositoryTestBase
{
    [Test]
    public async Task GetProducts_Should_Return_All_Products()
    {
        await using var context = CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetProducts(new GetProductsFilters(), CancellationToken.None);

        result.Should().HaveCount(5);
        result.Select(product => product.Sku).Should().BeEquivalentTo(
            "TV-SAMSUNG-001",
            "TV-LG-001",
            "PHONE-XIAOMI-001",
            "PHONE-SAMSUNG-001",
            "LAPTOP-LENOVO-001");
    }

    [Test]
    public async Task GetProducts_Should_Filter_By_Category()
    {
        await using var context = CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetProducts(
            new GetProductsFilters
            {
                categoryId = 1
            },
            CancellationToken.None);

        result.Should().HaveCount(2);
        result.Should().OnlyContain(product => product.CategoryId == 1);
    }

    [Test]
    public async Task GetProducts_Should_Filter_By_Status()
    {
        await using var context = CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetProducts(
            new GetProductsFilters
            {
                status = Status.Active
            },
            CancellationToken.None);

        result.Should().HaveCount(2);
        result.Should().OnlyContain(product => product.Status == Status.Active);
    }

    [Test]
    public async Task GetProducts_Should_Return_Requested_Page()
    {
        await using var context = CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetProducts(
            new GetProductsFilters
            {
                page = 2,
                pageSize = 2
            },
            CancellationToken.None);

        result.Should().HaveCount(2);
        result.Select(product => product.Sku).Should().ContainInOrder(
            "PHONE-XIAOMI-001",
            "PHONE-SAMSUNG-001");
    }

    [Test]
    public async Task GetProducts_Should_Filter_And_Paginate()
    {
        await using var context = CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetProducts(
            new GetProductsFilters
            {
                categoryId = 1,
                page = 2,
                pageSize = 1
            },
            CancellationToken.None);

        result.Should().ContainSingle();
        result[0].Sku.Should().Be("TV-LG-001");
    }

    [Test]
    public async Task GetProductById_Should_Return_Product_When_Product_Exists()
    {
        await using var context = CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetProductById(1, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Телевизор Samsung");
        result.Sku.Should().Be("TV-SAMSUNG-001");
        result.CategoryId.Should().Be(1);
        result.Status.Should().Be(Status.Active);
    }

    [Test]
    public async Task GetProductById_Should_Return_Null_When_Product_Does_Not_Exist()
    {
        await using var context = CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetProductById(999, CancellationToken.None);

        result.Should().BeNull();
    }

    [Test]
    public async Task CreateProduct_Should_Save_Product()
    {
        await using var context = CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.CreateProduct(
            new Product
            {
                Name = "Планшет Huawei",
                Sku = "TABLET-HUAWEI-001",
                CategoryId = 2,
                Status = Status.Active
            },
            CancellationToken.None);

        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be("Планшет Huawei");
        result.Sku.Should().Be("TABLET-HUAWEI-001");
        result.CategoryId.Should().Be(2);
        result.Status.Should().Be(Status.Active);

        await using var verificationContext = CreateContext();
        var savedProduct = await verificationContext.Products
            .AsNoTracking()
            .SingleAsync(product => product.Id == result.Id);

        savedProduct.Name.Should().Be("Планшет Huawei");
        savedProduct.Sku.Should().Be("TABLET-HUAWEI-001");
        savedProduct.CategoryId.Should().Be(2);
        savedProduct.Status.Should().Be(Status.Active);
    }

    [Test]
    public async Task CreateProduct_Should_Throw_NotFoundException_When_Category_Does_Not_Exist()
    {
        await using var context = CreateContext();
        var repository = new ProductRepository(context);

        Func<Task> act = () => repository.CreateProduct(
            new Product
            {
                Name = "Планшет Huawei",
                Sku = "TABLET-HUAWEI-001",
                CategoryId = 999,
                Status = Status.Active
            },
            CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Category with id 999 was not found");
    }

    [Test]
    public async Task CreateProduct_Should_Throw_ConflictException_When_Sku_Already_Exists()
    {
        await using var context = CreateContext();
        var repository = new ProductRepository(context);

        Func<Task> act = () => repository.CreateProduct(
            new Product
            {
                Name = "Другой телевизор",
                Sku = "TV-SAMSUNG-001",
                CategoryId = 1,
                Status = Status.Active
            },
            CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("Product with SKU 'TV-SAMSUNG-001' already exists");
    }

    [Test]
    public async Task ChangeStatus_Should_Update_Product_Status()
    {
        await using var context = CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.ChangeStatus(1, Status.Defective, CancellationToken.None);

        result.Id.Should().Be(1);
        result.Status.Should().Be(Status.Defective);

        await using var verificationContext = CreateContext();
        var savedProduct = await verificationContext.Products
            .AsNoTracking()
            .SingleAsync(product => product.Id == 1);

        savedProduct.Status.Should().Be(Status.Defective);
    }

    [Test]
    public async Task ChangeStatus_Should_Not_Change_Product_When_Status_Is_The_Same()
    {
        await using var context = CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.ChangeStatus(1, Status.Active, CancellationToken.None);

        result.Id.Should().Be(1);
        result.Status.Should().Be(Status.Active);
    }

    [Test]
    public async Task ChangeStatus_Should_Throw_NotFoundException_When_Product_Does_Not_Exist()
    {
        await using var context = CreateContext();
        var repository = new ProductRepository(context);

        Func<Task> act = () => repository.ChangeStatus(999, Status.Defective, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Product with id 999 was not found");
    }

    [Test]
    public async Task ChangeStatus_Should_Throw_InvalidStatusTransitionException_When_Transition_Is_Not_Allowed()
    {
        await using var context = CreateContext();
        var repository = new ProductRepository(context);

        Func<Task> act = () => repository.ChangeStatus(1, Status.WriteOff, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidStatusTransitionException>()
            .WithMessage("Cannot change product status from Active to WriteOff");
    }
}
