﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMarket.Infrastructure.Entity
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public string? PaymentDescription { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? PaymentPrice { get; set; }
        public int SellerId { get; set; }
        public Seller Seller { get; set; }
        public bool? IsApproved { get; set; }
    }
}
