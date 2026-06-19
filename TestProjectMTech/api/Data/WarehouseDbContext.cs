using Microsoft.EntityFrameworkCore;
using TestProjectMTech.api.Data.Models;
using TestProjectMTech.api.Domain;

namespace TestProjectMTech.api.Data;

public class WarehouseDbContext : DbContext
{
    public WarehouseDbContext(
        DbContextOptions<WarehouseDbContext> options) 
        : base(options)
    {}
    
    public DbSet<CategoryModel>  Categories => Set<CategoryModel>();
    
    public DbSet<ProductModel>  Products => Set<ProductModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        ConfigureCategory(modelBuilder);
        ConfigureProduct(modelBuilder);
        SeedData(modelBuilder);
    }
    
    private static void ConfigureProduct(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductModel>(builder =>
        {
            builder.ToTable("products");

            builder.HasKey(product => product.Id);

            builder.Property(product => product.Id)
                .ValueGeneratedOnAdd();

            builder.Property(product => product.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(product => product.Sku)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(product => product.Sku)
                .IsUnique();

            builder.Property(product => product.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
            
            builder.HasOne(product => product.Category)
                .WithMany(category => category.Products)
                .HasForeignKey(product => product.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
    }

    private static void ConfigureCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryModel>(builder =>
        {
            builder.ToTable("categories");

            builder.HasKey(category => category.Id);

            builder.Property(category => category.Id)
                .ValueGeneratedOnAdd();

            builder.Property(category => category.Name)
                .IsRequired()
                .HasMaxLength(100);
        });    
    }
    
    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryModel>().HasData(
            new
            {
                Id = 1,
                Name = "Телевизоры"
            },
            new
            {
                Id = 2,
                Name = "Смартфоны"
            },
            new
            {
                Id = 3,
                Name = "Ноутбуки"
            });

        modelBuilder.Entity<ProductModel>().HasData(
            new
            {
                Id = 1,
                Name = "Телевизор Samsung",
                Sku = "TV-SAMSUNG-001",
                CategoryId = 1,
                Status = Status.Active
            },
            new
            {
                Id = 2,
                Name = "Телевизор LG",
                Sku = "TV-LG-001",
                CategoryId = 1,
                Status = Status.Defective
            },
            new
            {
                Id = 3,
                Name = "Смартфон Xiaomi",
                Sku = "PHONE-XIAOMI-001",
                CategoryId = 2,
                Status = Status.Active
            },
            new
            {
                Id = 4,
                Name = "Смартфон Samsung",
                Sku = "PHONE-SAMSUNG-001",
                CategoryId = 2,
                Status = Status.WriteOff
            },
            new
            {
                Id = 5,
                Name = "Ноутбук Lenovo",
                Sku = "LAPTOP-LENOVO-001",
                CategoryId = 3,
                Status = Status.Defective
            });    
    }
}