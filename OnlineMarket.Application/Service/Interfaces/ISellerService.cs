﻿using OnlineMarket.BLL.ViewModels.Seller;
using OnlineMarket.Infrastructure.Entity;

namespace OnlineMarket.BLL.Service.Interfaces
{
    public interface ISellerService
    {
        IQueryable<Seller> GetAsync();
        Task CreateAsync(SellerVM seller);
        Task UpdateAsync(int? id);
        Task Delete(int? id);
    }
}
