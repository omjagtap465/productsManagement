using productsManagement.Models;

namespace productsManagement.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<bool> AddProduct(Product product);
    }
}
