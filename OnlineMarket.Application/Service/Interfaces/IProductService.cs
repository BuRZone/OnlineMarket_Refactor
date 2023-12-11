using OnlineMarket.BLL.ViewModels.Product;
using OnlineMarket.Infrastructure.Entity;

namespace OnlineMarket.BLL.Service.Interfaces
{
    public interface IProductService
    {
        IQueryable<Product> GetAsync();
        Task CreateAsync(ProductVM product);
        Task UpdateAsync(int? id);
        Task Delete(int? id);
    }
}
