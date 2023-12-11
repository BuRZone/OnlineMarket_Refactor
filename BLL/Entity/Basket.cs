using System.ComponentModel.DataAnnotations;

namespace OnlineMarket.Infrastructure.Entity
{
    public class Basket
    {
        [Key]
        public int Id { get; set; }
        public IEnumerable<Order>? Order { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
