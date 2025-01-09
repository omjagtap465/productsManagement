using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using productsManagement.Models;
using System.Data.Common;
namespace productsManagement.Repository
{
    public class ProductRepository
    {
        private readonly string _connectionString;
        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }
        public async Task<List<Product>> GetAllProducts()
        {
            Connection.Open();
            string query = @"select * from products";
            var products = await Connection.QueryAsync<Product>(query);
            Connection.Close();
            return products.ToList();
        }
        public async Task<bool> AddProduct(Product product)
        {
            Connection.Open();
            string query = @"
    INSERT INTO Products (ProductName, Category, Price, StockQuantity)
    VALUES (@ProductName, @Category, @Price, @StockQuantity)";
            await Connection.ExecuteAsync(query, product);

            Connection.Close(); 
            return true;

        }
        public async Task<bool> UpdateProduct(Product product)
        {
            Connection.Open();
            string selectQuery = "SELECT * FROM Products WHERE ProductID = @ProductID";
            var entity = await Connection.QuerySingleOrDefaultAsync<Product>(
                selectQuery,
                new { ProductID = product.ProductID }
            );

            if (entity is null)
            {
                return false; // Product does not exist
            }

            // Update the product
            string updateQuery = @"
UPDATE Products
SET 
    ProductName = @ProductName,
    Category = @Category,
    Price = @Price,
    StockQuantity = @StockQuantity,
    UpdatedAt = @UpdatedAt
WHERE ProductID = @ProductID";

            await Connection.ExecuteAsync(updateQuery, product);
            Connection.Close();
            return true;
        }

        public async Task<bool> DeleteProduct(int ProductID)
        {
            Connection.Open();

            string selectQuery = "SELECT * FROM Products WHERE ProductID = @ProductID";
            var productDetail = await Connection.QuerySingleOrDefaultAsync(selectQuery, new { ProductID });

            if (productDetail is null)
            {
                return false;
            }

          
            string deleteQuery = "DELETE FROM Products WHERE ProductID = @ProductID";
            await Connection.ExecuteAsync(deleteQuery, new { ProductID });

            Connection.Close();
            return true;
        }


    }
}