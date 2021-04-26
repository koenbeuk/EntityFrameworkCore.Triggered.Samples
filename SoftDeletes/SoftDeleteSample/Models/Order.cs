using SoftDeleteSample.Models.Traits;
using System;
using System.Collections.Generic;

namespace SoftDeleteSample.Models
{
    public class Order : ISoftDeletable
    {
        public int Id { get; set; }

        public string ReferenceName { get; set; }

        public DateTime? DeletedOn { get; set; }
        
        public User User { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

    }
}
