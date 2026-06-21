using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using TestProjectMTech.Api.Data;
using TestProjectMTech.Api.Middleware;
using TestProjectMTech.Api.Repositories;
using TestProjectMTech.Api.Repositories.Interfaces;
using TestProjectMTech.Api.Services;
using TestProjectMTech.Api.Services.Interfaces;
using TestProjectMTech.Api.Services.Policies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TestProjectMTech", Version = "v1" });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("DefaultConnection is not configured");

builder.Services.AddDbContextFactory<WarehouseDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSingleton<IProductStatusPolicy, ProductStatusPolicy>();

var app = builder.Build();

await MigrateDatabaseAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TestProjectMTech v1");
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

await app.RunAsync();

async Task MigrateDatabaseAsync(IServiceProvider services)
{
    const int retryCount = 10;
    var logger = services
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger("DatabaseMigration");

    for (var attempt = 1; attempt <= retryCount; attempt++)
    {
        try
        {
            var dbContextFactory = services.GetRequiredService<IDbContextFactory<WarehouseDbContext>>();
            await using var dbContext = dbContextFactory.CreateDbContext();
            await dbContext.Database.MigrateAsync();
            return;
        }
        catch (Exception exception) when (attempt < retryCount)
        {
            logger.LogWarning(
                exception,
                "Database migration failed on attempt {Attempt} of {RetryCount}",
                attempt,
                retryCount);

            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}
