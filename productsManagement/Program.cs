using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using productsManagement.Models;
using productsManagement.Repository;
using productsManagement.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
               .AllowAnyMethod() // Allow all HTTP methods
              .AllowAnyHeader(); // Allow all headers
    });
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();
app.UseCors("AllowFrontend");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapGet("/", () =>
{
    return "Hello from Minimal API!";
});

app.MapGet("/product", async (IProductService service) =>
{
    var result = await service.GetAllProducts();
    return result;

});
app.MapPost("/product", async (IProductService service, Product product) =>
{
    bool result = await service.AddProduct(product);
    return result ? Results.Ok(true) : Results.BadRequest("Error Adding Products ");
});
app.MapPut("/product", async (IProductService service, Product product) =>
{   
    bool result = await service.UpdateProduct(product);
    return result ? Results.Ok(true) : Results.BadRequest("Error while Updating the Product");
});
app.MapDelete("/product", async (IProductService service, [FromBody]Product product) =>
{
    bool result = await service.DeleteProduct(product.ProductID);
    return result ? Results.Ok(true) : Results.BadRequest("Error while Deleting the Product");
});

app.Run();

