using OnlineMarket.BLL.ViewModels.Basket;
using OnlineMarket.Infrastructure.Entity;


namespace OnlineMarket.BLL.Service.Interfaces
{
    public interface IBasketService
    {
        IQueryable<Basket> GetAsync();
        Task Update(AddToBasketVM toBasketVM);

    }
}
