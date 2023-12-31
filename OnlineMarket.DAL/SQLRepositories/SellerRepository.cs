﻿using OnlineMarket.Infrastructure.Entity;
using OnlineMarket.Infrastructure.Interfaces;

namespace OnlineMarket.Infrastructure.SQLRepositories
{
    public class SellerRepository : IBaseRepository<Seller>
    {
        private readonly ApplicationDbContext _context;
        public SellerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Seller seller)
        {
            _context.Sellers.Add(seller);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Seller seller)
        {
            _context.Sellers.Remove(seller);
            await _context.SaveChangesAsync();

        }

        private bool _disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IQueryable<Seller> GetAsync()
        {
            return _context.Sellers;
        }

        public async Task UpdateAsync(Seller seller)
        {
            _context.Sellers.Update(seller);
            await _context.SaveChangesAsync();
        }
    }
}
