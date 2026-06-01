using webbangiay.Models;

namespace webbangiay.Repository
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);
        Task AddAsync(OrderItem orderItem);
        Task DeleteAsync(int id);
    }
}