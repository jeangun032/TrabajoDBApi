using MongoDB.Driver;
using DDDExample.Domain.Entities;
using DDDExample.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDExample.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _collection;

        public ProductRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Product>("Products");
        }

        public async Task AddAsync(Product product) =>
            await _collection.InsertOneAsync(product);

        public async Task<Product?> GetByIdAsync(Guid id) =>
            await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task UpdateAsync(Product product) =>
            await _collection.ReplaceOneAsync(x => x.Id == product.Id, product);

        public async Task DeleteAsync(Guid id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}





