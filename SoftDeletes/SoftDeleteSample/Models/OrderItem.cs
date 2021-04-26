using SoftDeleteSample.Models.Traits;
using System;

namespace SoftDeleteSample.Models
{
    public class OrderItem : ISoftDeletable
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public DateTime? DeletedOn { get; set; }
        
        public Order Order { get; set; }
    }
}
