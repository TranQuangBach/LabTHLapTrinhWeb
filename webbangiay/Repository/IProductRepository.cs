using System.Collections.Generic;
using webbangiay.Models;

namespace webbangiay.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);

        Task<List<ProductImage>> GetImagesByProductIdAsync(int productId);
    }
}
