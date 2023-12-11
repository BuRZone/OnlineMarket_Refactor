using System.ComponentModel.DataAnnotations;

namespace OnlineMarket.Infrastructure.Entity
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public IEnumerable<Product>? Product { get; set; }
        public IEnumerable<Seller>? Seller { get; set; }
    }
}
