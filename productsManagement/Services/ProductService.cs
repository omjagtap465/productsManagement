﻿using productsManagement.Models;
using productsManagement.Repository;

namespace productsManagement.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _repository;
        public ProductService(ProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<Product>> GetAllProducts() => await _repository.GetAllProducts();
        public async Task<bool> AddProduct(Product product) => await _repository.AddProduct(product);

        
    }
}