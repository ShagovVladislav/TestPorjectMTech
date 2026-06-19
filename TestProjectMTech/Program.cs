using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using TestProjectMTech.api.Data;
using TestProjectMTech.api.Repositories;
using TestProjectMTech.api.Repositories.Interfaces;
using TestProjectMTech.api.Services;
using TestProjectMTech.api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TestProjectMTech", Version = "v1" });
});

builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TestProjectMTech v1");
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
