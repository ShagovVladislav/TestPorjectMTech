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

builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSingleton<IProductStatusPolicy, ProductStatusPolicy>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WarehouseDbContext>();
    await dbContext.Database.MigrateAsync();
}

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
