using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDExample.Domain.Entities;

namespace DDDExample.Domain.Repositories
{
 public interface IProductRepository
  {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Guid id);  }
}