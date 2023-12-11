using OnlineMarket.BLL.ViewModels.Order;
using OnlineMarket.Infrastructure.Entity;

namespace OnlineMarket.BLL.Service.Interfaces
{
    public interface IOrderService
    {
        IQueryable<Order> GetAsync();
        Task CreateAsync(OrderVM orderVM);
        Task UpdateAsync(int? id);
        Task Delete(Order order);
    }
}
