using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using productsManagement.Models;
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
            return products.ToList();
        }
        public async Task<bool> AddProduct(Product product)
        {
            Connection.Open();
            string query = @"
    INSERT INTO Products (ProductName, Category, Price, StockQuantity)
    VALUES (@ProductName, @Category, @Price, @StockQuantity)";
            await Connection.ExecuteAsync(query, product);
            return true;

        }
    }
}

