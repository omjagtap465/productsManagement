using Microsoft.AspNetCore.Http.HttpResults;
using productsManagement.Models;
using productsManagement.Repository;
using productsManagement.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddTransient<IProductService, ProductService>();
var app = builder.Build();
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

app.MapGet("/products", async (IProductService service) =>
{
    var result = await service.GetAllProducts();
    return result;

});
app.MapPost("/products", async (IProductService service, Product product) =>
{
    bool result = await service.AddProduct(product);
    return result ? Results.Ok() : Results.BadRequest("Error Adding Products ");
});


app.Run();

