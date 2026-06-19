using Microsoft.EntityFrameworkCore;
using TestProjectMTech.api.Data.Models;

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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
}